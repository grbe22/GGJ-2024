using Godot;
using System;

public partial class Node3D : Node
{
	private bool cannonSelected = false;
	private bool hoverCup = false;
	private Camera3D currentCamera;
	private CustomerController currentCustomer;
	private PackedScene projectile;
	private PackedScene customerScene;

	private Vector3 customerSeekPos = new(2.56f, 7.5f, -5.19f);

	private PackedScene emptyCup;
	Script scrpt = new CSharpScript();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// load files
		projectile = GD.Load<PackedScene>("res://scenes/Projectile.tscn");
		customerScene = GD.Load<PackedScene>("res://scenes/customer.tscn");
		emptyCup = GD.Load<PackedScene>("res://scenes/EmptyCup.tscn");
		scrpt = GD.Load<Script>("res://scripts/MouseFollow.cs");

		// set up view and order system
		switchView();

		currentCustomer = SpawnCustomer();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		#region // Input updates

		// cannon fire event
		if (Input.IsActionJustPressed("click") && cannonSelected)
		{
			if (currentCustomer != null)
			{
				// launch customer
				Vector3 customerDir = new(0, 0.1f, -1);
				currentCustomer.Launch(30, customerDir);
				InstantiateProjectile();
				currentCustomer = null;
			}
		}

		// camera switch event
		if (Input.IsActionJustPressed("Switch Camera"))
		{
			switchView();
		}

		// cup spawning event
		if (Input.IsActionJustPressed("click") && hoverCup)
		{
			// todo: spawn cup at mouse position

			Node cup = emptyCup.Instantiate();

			//cup.position = GetViewport().GetMousePosition();
			AddChild(cup);
			GD.Print("grab cup");
		}

		#endregion

		// if not null, seek position
		currentCustomer?.SeekPosition(customerSeekPos);

		// if null, spawn new customer
		currentCustomer ??= SpawnCustomer();
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
			currentCamera = GetNode<Camera3D>("CameraSystem/Camera2");
		}
		else
		{
			currentCamera = GetNode<Camera3D>("CameraSystem/Camera1");
		}
		// Activate the new camera
		if (currentCamera != null)
		{
			currentCamera.Current = true;
		}
	}

	private void _on_cannon_mouse_entered()
	{
		cannonSelected = true;
		// Replace with function body.
	}
	private void _on_cannon_mouse_exited()
	{
		cannonSelected = false;
		// Replace with function body.
	}
	private void _on_coffee_cup_mouse_entered()
	{
		hoverCup = true;
	}
	private void _on_coffee_cup_mouse_exited()
	{
		hoverCup = false;
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
		root.AddChild(instance);
		instance.Position = GetRandomStartPos();
		return instance;
	}

	/// <summary>
	/// Gets a random position to spawn customer on edge 
	/// of large circle within a certain "view radius"
	/// </summary>
	private Vector3 GetRandomStartPos()
	{
		float radius = 50f;
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
}
