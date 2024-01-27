using Godot;
using System;

public partial class Node3D : Node
{
	private Camera3D currentCamera;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// WHY THE HELL DO I HAVE TO USE ISACTIONPRESSED 
		// ITS SUPPOSED TO JUST NEED ISACTIONJUSTPRESSED
		if (Input.IsActionJustPressed("Switch Camera") && Input.IsActionPressed("Switch Camera"))
		{
			switchView();
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
}
