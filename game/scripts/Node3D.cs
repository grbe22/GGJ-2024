using Godot;
using System;

public partial class Node3D : Node
{
	private bool cannonSelected = false;
	private bool hoverCup = false;
	private Camera3D currentCamera;
	
	private Texture2D emptyImg = new Texture2D();
	Script scrpt = new CSharpScript();
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		switchView();
		OrderScript test = new OrderScript();
		for (int i = 0; i < 100; i++) {
			int[] order = test.GenerateOrder();
			GD.Print(test.PrintOrder(order));
		} 
		
		emptyImg.ResourcePath = "res://assets/drinks/empty.png";
		scrpt = GD.Load<Script>("res://scripts/MouseFollow.cs");
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
			AddChild(cup);
			cup.Texture = emptyImg;
			cup.SetScript(scrpt);
			cup.Position = GetViewport().GetMousePosition();
			//AddChild(cup);
			
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
