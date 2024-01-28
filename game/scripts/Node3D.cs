using Godot;
using System;
using System.Collections.Generic;

public partial class Node3D : Node
{
	
	// fuck this game engine
	private static bool man = true;
	private bool woman = true;
	
	//scoring variable
	int score;
	// values for hander
	private bool cannonSelected = false;
	private bool trashHover = false;
	private bool hoverCup = false;
	private bool hoverBowl = false;
	private bool hoverWork = false;
	private bool hoverCoffee = false;
	private bool hoverMilk = false;
	private bool hoverVeganMilk = false;
	private bool hoverWhippedCream = false;
	private bool hoverMayo = false;
	private bool hoverChocolate = false;
	private bool hoverCaramel = false;
	private bool hoverBleuCheese = false;
	private bool hoverFruit = false;
	private bool hoverPotato = false;

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

	// for handling sprites
	private SpriteHandler sprites;
	private Sprite3D cup;
	private Sprite3D bowl;
	private Sprite3D addon;

	private Vector3 cupInitialPos;
	private Vector3 bowlInitialPos;

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
		if (man) {
			man = false;
			GD.Print(man);
			return;
		}
		woman = false;
		GD.Print("Ayudar");
		scoreboard = GetNode<Label>("../Node3D/Scoreboard");
		GD.Print(scoreboard);
		score = 0;
		// load Cup and start spriteHandler
		// GetNode reaches from root/camera for some reason. we need to be at root/Node3D
		// so keep the absolute path used here unless there's some issue on machines that aren't mine
		cup = GetNode<Sprite3D>("../Node3D/Cup/Cup");
		bowl = GetNode<Sprite3D>("../Node3D/Bowl/Bowl");
		addon = GetNode<Sprite3D>("../Node3D/Cup/Addon");
		sprites = new SpriteHandler(cup, bowl, addon);

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
		if (woman) {
			return;
		}
		scoreboard.Text = "" +score;
		#region // Input updates
		if (Input.IsActionJustPressed("click"))
		{
			// ~~ main drink choosing ~~

			// 0 is coffee, 1 is milk, 2 is veganmilk
			// checks if you click on coffee & updates drink
			if (hoverCoffee)
			{
				workingOrder[1] = sprites.SetCup(0, workingOrder[1]);
			}
			if (hoverMilk)
			{
				workingOrder[1] = sprites.SetCup(1, workingOrder[1]);
			}
			if (hoverVeganMilk)
			{
				workingOrder[1] = sprites.SetCup(2, workingOrder[1]);
			}
			if (trashHover)
			{
				workingOrder[1] = sprites.EmptyCup();
				workingOrder[2] = sprites.ClearTopping();
			}

			// ~~ topping/addon choosing ~~
			// if no addon exists yet, AND liquid is not empty, 
			// AND cup is on the back view, do addon shtuff
			if (workingOrder[2] == 0 && workingOrder[1] != 0 && CupGrab.InBack)
			{
				if (hoverWhippedCream)
				{
					workingOrder[2] = sprites.SetAddon(AddonType.WhippedCream);
				}
				if (hoverMayo)
				{
					workingOrder[2] = sprites.SetAddon(AddonType.Mayo);
				}
				if (hoverChocolate)
				{
					workingOrder[2] = sprites.SetAddon(AddonType.Chocolate);
				}
				if (hoverCaramel)
				{
					workingOrder[2] = sprites.SetAddon(AddonType.Caramel);
				}
			}
			
			// ~~ food choosing ~~		
			// if no addon exists yet,  consider food
			if (workingOrder[0] != 0)
			{
				if (hoverBleuCheese)
				{
					order[3] = sprites.SetBowl(0);
				}
				if (hoverFruit)
				{
					order[3] = sprites.SetBowl(1);
				}
				if (hoverPotato)
				{
					order[3] = sprites.SetBowl(2);
				}
			}
		}

			// ~~ cannon events ~~

			// cannon fire, button pressed
			if (hoverCannonButon)
			{
				GD.Print("button clicked");
				int curScore = 0;

				// if (currentCustomer != null && heldItem != null)
				if (currentCustomer != null)
				{
					// launch customer
					Vector3 customerDir = new(0, 0.2f, -1);
					currentCustomer.Launch(40, customerDir);
					InstantiateProjectile();

					bool orderCorrect =
						cannonContents[0] == currentCustomer.Order[0] &&
						cannonContents[1] == currentCustomer.Order[1] &&
						cannonContents[2] == currentCustomer.Order[2];

					if (orderCorrect)
					{
						curScore += 100;
						GD.Print("ORDER CORRECT!");
					}
					else
					{
						GD.Print("ORDER INCORRECT!");
					}

					CupGrab.Visible = true;
					BowlGrab.Visible = true;
					sprites.ResetCup();
					ResetOrders();
					currentCustomer = null;
				}

				score += curScore;

			}

			// cannon load event
			if (cannonSelected && !hoverCannonButon)
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
				}

				// make bowl invisible and put it back in 
				// initial position when loaded to cannon
				if (BowlGrab.IsGrabbed)
				{
					BowlGrab.Visible = false;
					BowlGrab.Position = bowlInitialPos;

					// save bowl info in cannon order
					cannonContents[0] = workingOrder[0];
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
		currentCustomer?.SeekPosition(customerSeekPos, 1f);
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
	}

	// ~~ drinks ~~

	// when selecting coffee
	private void _on_coffee_mouse_entered() { hoverCoffee = true; }
	private void _on_coffee_mouse_exited() { hoverCoffee = false; }

	// when selecting normal milk
	private void _on_milk_mouse_entered() { hoverMilk = true; }
	private void _on_milk_mouse_exited() { hoverMilk = false; }

	// when selecting veganmilk
	private void _on_vegan_milk_mouse_entered() { hoverVeganMilk = true; }
	private void _on_vegan_milk_mouse_exited() { hoverVeganMilk = false; }


	// ~~ toppings ~~

	// when selecting whipped cream
	private void _on_whipped_cream_mouse_entered() { hoverWhippedCream = true; }
	private void _on_whipped_cream_mouse_exited() { hoverWhippedCream = false; }

	// when selecting mayo 
	private void _on_mayo_mouse_entered() { hoverMayo = true; }
	private void _on_mayo_mouse_exited() { hoverMayo = false; }

	// when selecting chocolate
	private void _on_chocolate_mouse_entered() { hoverChocolate = true; }
	private void _on_chocolate_mouse_exited() { hoverChocolate = false; }

	// when selecting caramel
	private void _on_caramel_mouse_entered() { hoverCaramel = true; }
	private void _on_caramel_mouse_exited() { hoverCaramel = false; }
	
	
	// ~~ foods ~~
	
	// when selecting bleu cheese
	private void _on_bleu_cheese_mouse_entered() { hoverBleuCheese = true; }
	private void _on_bleu_cheese_mouse_exited() { hoverBleuCheese = false; }
	
	// when selecting fruit
	private void _on_fruit_mouse_entered() { hoverFruit = true; }
	private void _on_fruit_mouse_exited() { hoverFruit = false; }
	
	// when selecting potato
	private void _on_potato_mouse_entered() { hoverPotato = true; }
	private void _on_potato_mouse_exited() { hoverPotato = false; }

	// ~~ misc ~~ 

	// for disposing of drinks
	private void _on_trash_man_mouse_entered() { trashHover = true; }
	private void _on_trash_man_mouse_exited() { trashHover = false; }

	// for cannons
	private void _on_cannon_mouse_entered() { cannonSelected = true; }
	private void _on_cannon_mouse_exited() { cannonSelected = false; }

	// for coffee cup
	private void _on_coffee_cup_mouse_entered() { hoverCup = true; }
	private void _on_coffee_cup_mouse_exited() { hoverCup = false; }
	
	// for bowl
	private void _on_bowl_mouse_entered() { hoverBowl = true; }
	private void _on_bowl_mouse_exited() { hoverBowl = false; }

	// for the specific area around machines
	private void _on_work_area_mouse_entered() { hoverWork = true; }
	private void _on_work_area_mouse_exited() { hoverWork = false; }

	// for the button
	private void _on_button_mouse_entered() { hoverCannonButon = true; }
	private void _on_button_mouse_exited() { hoverCannonButon = false; }
}
