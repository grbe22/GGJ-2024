using Godot;
using System;
using System.Collections.Generic;

public partial class Node3D : Node
{
	private bool cannonSelected = false;
	private bool hoverCup = false;
	private bool hoverWork = false;
	private Camera3D currentCamera;
	private CustomerController currentCustomer;
	private PackedScene projectile;

	private PackedScene emptyCup;
	Script scrpt = new CSharpScript();
	private Node2D heldItem;
	private List<Node2D> items = new List<Node2D>();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// load projectile
		projectile = GD.Load<PackedScene>("res://scenes/Projectile.tscn");

		// set up view and order system
		switchView();
		
		scrpt = GD.Load<Script>("res://scripts/MouseFollow.cs");
		
		emptyCup = GD.Load<PackedScene>("res://scenes/EmptyCup.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// cannon fire event
		if (Input.IsActionJustPressed("click") && cannonSelected)
		{
			GD.Print("Cannon clicked");
			
			UpdateCustomer();
			if (currentCustomer != null && !currentCustomer.EnableFloppy && heldItem != null)
			{
				// launch customer
				Vector3 customerDir = new(0, 0.1f, -1);
				currentCustomer.Launch(30, customerDir);
				
				items.Remove(heldItem);
				heldItem.QueueFree();
				heldItem = null;
			}

			InstantiateProjectile();
		}

		// camera switch event
		if (Input.IsActionJustPressed("Switch Camera"))
		{
			switchView();
		}
		
		// cup spawning event
		else if (Input.IsActionJustPressed("click") && hoverCup)
		{
			// If the user is already holding an item, remove that item
			if (heldItem != null) 
			{
				items.Remove(heldItem);
				heldItem.QueueFree();
			}
			heldItem = emptyCup.Instantiate() as Node2D;
			items.Add(heldItem);
			
			AddChild(heldItem);
			GD.Print("grab cup");
			hoverCup = false;
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
	private void _on_work_area_mouse_entered()
	{
		hoverWork = true;
	}
	private void _on_work_area_mouse_exited()
	{
		hoverWork = false;
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
