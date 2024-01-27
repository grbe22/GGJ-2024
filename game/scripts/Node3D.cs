using Godot;
using System;

public partial class Node3D : Node
{
	private bool cannonSelected = false;
	private bool hoverCup = false;
	private Camera3D currentCamera;
	private CustomerController currentCustomer;
	private PackedScene projectile;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// load projectile
		projectile = GD.Load<PackedScene>("res://scenes/Projectile.tscn");

		// set up view and order system
		switchView();
		OrderScript test = new OrderScript();
		for (int i = 0; i < 100; i++)
		{
			int[] order = test.GenerateOrder();
			GD.Print(test.PrintOrder(order));
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// cannon fire event
		if (Input.IsActionJustPressed("click") && cannonSelected)
		{
			GD.Print("Cannon clicked");

			UpdateCustomer();
			if (currentCustomer != null && !currentCustomer.EnableFloppy)
			{
				// launch customer
				Vector3 customerDir = new(0, 0.1f, -1);
				currentCustomer.Launch(30, customerDir);
			}

			InstantiateProjectile();
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

			Sprite2D cup = new Sprite2D();
			Resource img = new Texture2D();
			CSharpScript scrpt = new CSharpScript();
			img.ResourcePath = "res://assets/drinks/empty.png";
			scrpt.SourceCode = "res://scripts/MouseFollow.cs";

			cup.Texture = (Texture2D)img;
			cup.SetScript(scrpt);
			cup.Position = GetViewport().GetMousePosition();
			AddChild(cup);

			GD.Print("grab cup");
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

	private void UpdateCustomer()
	{
		// exit condition when there are no children
		foreach (Node child in GetChildren())
		{
			if (child is CustomerController controller)
			{
				currentCustomer = controller;
			}
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
}
