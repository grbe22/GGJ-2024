using Godot;
using System;

public partial class CameraSystem : Node
{
	private Camera3D currentCamera;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Switch Camera"))
		{
			if (currentCamera != null) {
				currentCamera.Current = false;
			}

			// Get the next camera
			if (currentCamera == null || currentCamera.Name == "Camera1") {
				currentCamera = GetNode<Camera3D>("Camera2");
			} else {
				currentCamera = GetNode<Camera3D>("Camera1");
			}
			// Activate the new camera
			if (currentCamera != null) {
				currentCamera.Current = true;
			}
		}
	}
}
