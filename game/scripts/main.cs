using Godot;
using System;

public partial class main : Node3D
{
	private Camera3D _camera3D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		_camera3D = GetNode<Camera3D>("Camera3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
