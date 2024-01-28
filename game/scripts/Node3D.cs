using Godot;
using System;
using System.Collections.Generic;

public partial class Node3D : Node
{
	
	//scoring variable
	int score;
	// values for hander
	private bool cannonSelected = false;
	private bool trashHover = false;
	private bool hoverCup = false;
	private bool hoverWork = false;
	private bool hoverCoffee = false;
	private bool hoverMilk = false;
	private bool hoverVeganMilk = false;
	private bool hoverWhippedCream = false;
	private bool hoverMayo = false;
	private bool hoverChocolate = false;
	private bool hoverCaramel = false;

	// objects/instances
	private Camera3D currentCamera;
	private CustomerController currentCustomer;
	private PackedScene projectile;
	private PackedScene customerScene;

	// useful data things
	private Vector3 customerSeekPos = new(-5f, 7.5f, -5.19f);
	private readonly int spawnFrameOffset = 10;
	private int frameCounter = 0;

	int[] order;

	private PackedScene emptyCup;
	Script scrpt = new CSharpScript();
	private Node2D heldItem;
	private List<Node2D> items = new List<Node2D>();

	// for handling sprites
	private SpriteHandler sprites;
	private Sprite3D cup;
	private Sprite3D bowl;
	private Sprite3D addon;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		score = 0;
		// load Cup and start spriteHandler
		// GetNode reaches from root/camera for some reason. we need to be at root/Node3D
		// so keep the absolute path used here unless there's some issue on machines that aren't mine
		cup = GetNode<Sprite3D>("../Node3D/Cup/Cup");
		addon = GetNode<Sprite3D>("../Node3D/Cup/Addon");
		sprites = new SpriteHandler(cup, cup, addon);

		// starts an empty order
		order = new int[3];
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
		Label scoreboard = GetNode<Label>("../Node3D/Scoreboard");
		scoreboard.Text = "" +score;
		#region // Input updates
		if (Input.IsActionJustPressed("click"))
		{
			// ~~ main drink choosing ~~

			// 0 is coffee, 1 is milk, 2 is veganmilk
			// checks if you click on coffee & updates drink
			if (hoverCoffee)
			{
				order[1] = sprites.SetCup(0, order[1]);
			}
			if (hoverMilk)
			{
				order[1] = sprites.SetCup(1, order[1]);
			}
			if (hoverVeganMilk)
			{
				order[1] = sprites.SetCup(2, order[1]);
			}
			if (trashHover)
			{
				order[1] = sprites.EmptyCup();
				order[2] = sprites.ClearTopping();
			}

			// ~~ topping/addon choosing ~~
			// if no addon exists yet, AND liquid is not empty, do addon shtuff
			if (order[2] == 0 && order[1] != 0)
			{
				if (hoverWhippedCream)
				{
					order[2] = sprites.SetAddon(AddonType.WhippedCream);
				}
				if (hoverMayo)
				{
					order[2] = sprites.SetAddon(AddonType.Mayo);
				}
				if (hoverChocolate)
				{
					order[2] = sprites.SetAddon(AddonType.Chocolate);
				}
				if (hoverCaramel)
				{
					order[2] = sprites.SetAddon(AddonType.Caramel);
				}
			}
			
			// ~~ food choosing ~~
			// if no addon exists yet,  consider food
			if (order[0] != 0)
			{
				if (hoverWhippedCream)
				{
					order[0] = sprites.SetAddon(AddonType.WhippedCream);
				}
			}
		}


		// cannon fire event
		if (Input.IsActionJustPressed("click") && cannonSelected)
		{
			int curScore = 0;
			// if (currentCustomer != null && heldItem != null)
			if (currentCustomer != null)
			{
				// launch customer
				Vector3 customerDir = new(0, 0.2f, -1);
				curScore = currentCustomer.Launch(40, customerDir);
				InstantiateProjectile();
				currentCustomer = null;
				if (heldItem != null) {
					items.Remove(heldItem);
					heldItem.QueueFree();
					heldItem = null;
				}
			}
			score += curScore;
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

	// for the specific area around machines
	private void _on_work_area_mouse_entered() { hoverWork = true; }
	private void _on_work_area_mouse_exited() { hoverWork = false; }
}

