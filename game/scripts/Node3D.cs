using GGJloserteam.scripts;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;

public partial class Node3D : Node
{
	// values for detecting what's selected & click handling
	private MouseLocator mouseHandler;

	// objects/instances
	private Camera3D currentCamera;
	private CustomerController currentCustomer;
	private PackedScene projectile;
	private PackedScene customerScene;

	// useful data things
	private Vector3 customerSeekPos = new(-5f, 7.5f, -5.19f);
	private readonly int spawnFrameOffset = 10;
	private int frameCounter = 0;

	int[] workingOrder;         // order that player is working on
	int[] cannonContents;       // what's in the cannon

	private PackedScene emptyCup;
	Script scrpt = new CSharpScript();
	private Node2D heldItem;
	private List<Node2D> items = new List<Node2D>();
	private Label scoreboard;
	private Label lastScore;

	// for handling sprites
	private SpriteHandler sprites;
	private Sprite3D cup;
	private Sprite3D bowl;
	private Sprite3D addon;

	private Vector3 cupInitialPos;
	private Vector3 bowlInitialPos;

	// audio
	private AudioStreamPlayer fxPlayer;

	private int incorrectOrders = 0;
	private const int MaxIncorrectOrders = 3;

	/// <summary>
	/// Reference to cup static body's GrabStuff script
	/// </summary>
	private GrabStuff CupGrab
	{
		get
		{
			return GetNode<GrabStuff>("../Node3D/Cup");
		}
	}

	/// <summary>
	/// Reference to bowl static body's GrabStuff script
	/// </summary>
	private GrabStuff BowlGrab
	{
		get
		{
			return GetNode<GrabStuff>("../Node3D/Bowl");
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Label failedOrders = GetNode<Label>("../Node3D/Lives");
		failedOrders.Text = "0/" + (MaxIncorrectOrders + 1);
		mouseHandler = new MouseLocator();
		scoreboard = GetNode<Label>("../Node3D/Scoreboard");
		lastScore = GetNode<Label>("../Node3D/AddOn");
		Global.GetInstance().score = 0;
		// load Cup and start spriteHandler
		// GetNode reaches from root/camera for some reason. we need to be at root/Node3D
		// so keep the absolute path used here unless there's some issue on machines that aren't mine
		cup = GetNode<Sprite3D>("../Node3D/Cup/Cup");
		bowl = GetNode<Sprite3D>("../Node3D/Bowl/Bowl");
		addon = GetNode<Sprite3D>("../Node3D/Cup/Addon");
		sprites = new SpriteHandler(cup, bowl, addon);

		cupInitialPos = CupGrab.Position;
		bowlInitialPos = BowlGrab.Position;

		fxPlayer = GetNode<AudioStreamPlayer>("../Node3D/FxPlayer");

		// starts an empty order
		ResetOrders();

		// load files
		projectile = GD.Load<PackedScene>("res://scenes/Projectile.tscn");
		customerScene = GD.Load<PackedScene>("res://scenes/customer.tscn");
		emptyCup = GD.Load<PackedScene>("res://scenes/EmptyCup.tscn");
		scrpt = GD.Load<Script>("res://scripts/MouseFollow.cs");

		// set up view and order system
		switchView();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		scoreboard.Text = "" + Global.GetInstance().score;
		#region // Input updates
		if (Input.IsActionJustPressed("click") && mouseHandler.selected != MouseLocator.Hovered.None) 
		{

			// ~~ cannon events ~~

			// cannon fire, button pressed

			if (mouseHandler.selected == MouseLocator.Hovered.CannonButton)
			{
				int curScore = 0;

				bool cannonEmpty =
					cannonContents[0] == 0 &&
					cannonContents[1] == 0 &&
					cannonContents[2] == 0;

				// LAUNCHING
				if (currentCustomer != null && currentCustomer.AtPosition && !cannonEmpty)
				{
					PlayAudioFx("wet_clonk");

					// launch customer
					Vector3 customerDir = new(0, 0.2f, -1);
					int time = currentCustomer.Launch(40, customerDir);
					Global.GetInstance().score += time;
					InstantiateProjectile();

					Scoring scoreGen = new Scoring();
					int grade = scoreGen.Grade(currentCustomer.Order, cannonContents);
					Global.GetInstance().score += grade;
					lastScore.Text = "\n+" + (grade + time);

					bool orderCorrect =
						cannonContents[0] == currentCustomer.Order[0] &&
						cannonContents[1] == currentCustomer.Order[1] &&
						cannonContents[2] == currentCustomer.Order[2];

					if (!orderCorrect)
					{
						incorrectOrders++;
						Label failedOrders = GetNode<Label>("../Node3D/Lives");
						failedOrders.Text = incorrectOrders + " /" + (MaxIncorrectOrders + 1);
					}

					sprites.ResetCup();
					sprites.EmptyBowl();
					sprites.EmptyCup();
					ResetOrders();
					currentCustomer = null;
				}

				Global.GetInstance().score += curScore;

			}

			// cannon load event
			else if (mouseHandler.selected == MouseLocator.Hovered.Cannon)
			{
				// make cup invisible and put it back in
				// initial position when loaded to cannon
				if (CupGrab.IsGrabbed)
				{
					CupGrab.Visible = false;
					CupGrab.Position = cupInitialPos;

					// save cup info in cannon order
					cannonContents[1] = workingOrder[1];
					cannonContents[2] = workingOrder[2];

					PlayAudioFx("clonk");
				}

				// make bowl invisible and put it back in 
				// initial position when loaded to cannon
				if (BowlGrab.IsGrabbed)
				{
					BowlGrab.Visible = false;
					BowlGrab.Position = bowlInitialPos;

					// save bowl info in cannon order
					cannonContents[0] = workingOrder[0];

					PlayAudioFx("clonk");
				}
			}

			else
			{
				// ~~ main drink choosing ~~

				// 0 is coffee, 1 is milk, 2 is veganmilk
				// checks if you click on coffee & updates drink
				int handlerOut = mouseHandler.UpdateSprite(workingOrder, sprites, CupGrab.InBack);
				if (handlerOut == 0)
				{
					PlayAudioFx("bloop_mid");
				}
				if (handlerOut == 1) { 
					ResetOrders();
					PlayAudioFx("bit_1");
				}

			}
			
			

			
		}


		// camera switch event
		if (Input.IsActionJustPressed("Switch Camera"))
		{
			switchView();
		}

		#endregion

		// only spawn first customer after first few frames
		if (frameCounter > spawnFrameOffset)
		{
			// if null, spawn new customer
			currentCustomer ??= SpawnCustomer();
		}
		else
		{
			frameCounter++;
		}

		// if not null, seek position
		if (currentCustomer != null)
		{
			currentCustomer.SeekPosition(customerSeekPos, 1f);

			// only plays sound once
			if (currentCustomer.JustArrivedAtPosition)
			{
				PlayAudioFx("weedle_1");
			}
		}

		// END GAME IF TOO MANY WRONG ORDERS
		if (incorrectOrders > MaxIncorrectOrders)
		{
			incorrectOrders = 0;
			ResetOrders();
			GetTree().ChangeSceneToFile("res://scenes/lose.tscn");
		}
	}

	public void switchView()
	{
		if (currentCamera != null)
		{
			currentCamera.Current = false;
		}

		// Get the next camera
		if (currentCamera == null || currentCamera.Name == "Camera1")
		{
			currentCamera = GetNode<Camera3D>("../Node3D/CameraSystem/Camera2");
		}
		else
		{
			currentCamera = GetNode<Camera3D>("../Node3D/CameraSystem/Camera1");
		}
		// Activate the new camera
		if (currentCamera != null)
		{
			currentCamera.Current = true;
		}
	}

	private void InstantiateProjectile()
	{
		// add projectile instance
		RigidBody3D inst = projectile.Instantiate() as RigidBody3D;

		// update position to be in front of customer
		inst.Position = new Vector3(6.35f, 3f, 0.81f);
		AddChild(inst);

		// set projectile direction
		Vector3 projDir = currentCustomer.Position - inst.Position;
		projDir = projDir.Normalized();

		// grab reference and launch projectile
		ProjectileController controller = inst as ProjectileController;
		controller.Launch(projDir, 200);
	}

	private CustomerController SpawnCustomer()
	{
		Node root = this.GetParent();
		CustomerController instance = customerScene.Instantiate() as CustomerController;
		// root.AddChild(instance);
		root.CallDeferred("add_child", instance);
		instance.Position = GetRandomStartPos();
		return instance;
	}

	/// <summary>
	/// Gets a random position to spawn customer on edge 
	/// of large circle within a certain "view radius"
	/// </summary>
	private Vector3 GetRandomStartPos()
	{
		float radius = 200f;
		float angleSpread = Mathf.Pi / 1.5f;

		// calculate random angle centered forward (negative z)
		RandomNumberGenerator rng = new();
		float angle = (rng.Randf() * angleSpread) + angleSpread / 2;

		// calculate coords
		float randX = Mathf.Cos(angle) * radius;
		float randZ = -Mathf.Sin(angle) * radius;

		// return created vector
		return new Vector3(randX, 0, randZ) + customerSeekPos;
	}

	private void ResetOrders()
	{

		workingOrder = new int[3];
		cannonContents = new int[3];

		CupGrab.Position = cupInitialPos;
		BowlGrab.Position = bowlInitialPos;

		CupGrab.Visible = true;
		BowlGrab.Visible = true;
		
		sprites.EmptyBowl();
		sprites.EmptyCup();
		sprites.ClearTopping();
	}

	private void PlayAudioFx(string effectName)
	{
		AudioStream effect = GD.Load<AudioStream>("res://assets/audio/fx/" + effectName + ".wav");
		fxPlayer.Stream = effect;
		fxPlayer.Play();
	}
	
	// passes a value to mouseHandler for onClick
	private void OnSelect(int k) {
		mouseHandler.Select(k);
	}

	// passes a value to mouseHandler for onClick
	private void OnDeselect(int k) {
		mouseHandler.Deselect(k);
	}
}
