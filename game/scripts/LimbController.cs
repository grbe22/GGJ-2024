using Godot;
using System;

public partial class LimbController : RigidBody3D
{
	/// <summary>
	/// Locks y/x axis rotation
	/// </summary>
	public bool Floppy { get; set; } = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		AxisLockAngularY = !Floppy;
		AxisLockAngularX = !Floppy;
	}
}
