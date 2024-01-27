using Godot;
using System;

public partial class Node3D : Node
{
	private bool cannonSelected = false;
	private bool hoverCup = false;
	private Camera3D currentCamera;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		switchView();
		OrderScript test = new OrderScript();
		for (int i = 0; i < 100; i++) {
			int[] order = test.GenerateOrder();
			GD.Print(test.PrintOrder(order));
		} 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// WHY THE HELL DO I HAVE TO USE ISACTIONPRESSED 
		// ITS SUPPOSED TO JUST NEED ISACTIONJUSTPRESSED
		if (Input.IsActionJustPressed("click") && cannonSelected) {
			// todo: build cannon fire function
			SetFloppy(this);
		}
		if (Input.IsActionJustPressed("Switch Camera") && Input.IsActionPressed("Switch Camera"))
		{
			switchView();
		}
		if (Input.IsActionJustPressed("click") && hoverCup) {
			// todo: spawn cup at mouse position
			
			Sprite2D cup = new Sprite2D();
			Resource img = new Texture2D();
			CSharpScript scrpt = new CSharpScript();
			img.ResourcePath = "res://assets/drinks/empty.png";
			scrpt.SourceCode = "res://scripts/MouseFollow.cs";
			
			cup.Texture = (Texture2D) img;
			cup.SetScript(scrpt);
			AddChild(cup);
			GD.Print("grab cup");
		}
	}
	
	public void switchView() {
		if (currentCamera != null) {
			currentCamera.Current = false;
		}

		// Get the next camera
		if (currentCamera == null || currentCamera.Name == "Camera1") {
			currentCamera = GetNode<Camera3D>("CameraSystem/Camera2");
		} else {
			currentCamera = GetNode<Camera3D>("CameraSystem/Camera1");
		}
		// Activate the new camera
		if (currentCamera != null) {
			currentCamera.Current = true;
		}
	}
	
	private void SetFloppy(Node node)
	{
		CustomerController controller = node as CustomerController;
		if (controller != null)
		{
			controller.EnableFloppy = true;
		}

		// exit condition when there are no children
		foreach (Node child in node.GetChildren())
		{
			SetFloppy(child);
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
}
