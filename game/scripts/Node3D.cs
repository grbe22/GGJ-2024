using Godot;
using System;
using MouseFollow;

public partial class Node3D : Node
{
	private bool cannonSelected = false;
	private bool hoverCup = false;
	private bool hoverWork = false;
	private Camera3D currentCamera;
	private CustomerController currentCustomer;
	
	private PackedScene emptyCup;
	Script scrpt = new CSharpScript();
	private Node2D heldItem;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		switchView();
		
		scrpt = GD.Load<Script>("res://scripts/MouseFollow.cs");
		
		emptyCup = GD.Load<PackedScene>("res://scenes/EmptyCup.tscn");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// WHY THE HELL DO I HAVE TO USE ISACTIONPRESSED 
		// ITS SUPPOSED TO JUST NEED ISACTIONJUSTPRESSED
		if (Input.IsActionJustPressed("click") && cannonSelected)
		{
			// todo: build cannon fire function
			UpdateCustomer();
			if (!currentCustomer.EnableFloppy)
			{
				Vector3 direction = new(0, 0.1f, -1);
				currentCustomer.Launch(30, direction);
			}
		}
		if (Input.IsActionJustPressed("Switch Camera") && Input.IsActionPressed("Switch Camera"))
		{
			switchView();
		}
		if (Input.IsActionJustPressed("click") && hoverCup)
		{
			// If the user is already holding an item, remove that item
			if (heldItem != null) 
			{
				heldItem.QueueFree();
			}
			heldItem = emptyCup.Instantiate() as Node2D;
			
			AddChild(heldItem);
			GD.Print("grab cup");
			hoverCup = false;
		}
		if (Input.IsActionJustPressed("click") && hoverWork) {
			//FollowToggle();
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
	
	//public void FollowToggle() {
		//if (heldItem.follow) {
			//heldItem.follow = false;
		//}
		//else 
		//{
			//heldItem.follow = true;
		//}
	//}

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
	
}



