using Godot;
using System;

public partial class MovementTesting : CharacterBody3D
{
	[Export]
	public float Speed { get; set; } = 10f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Movement testing controller ready");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_down", "move_up");
		Vector3 vel = new(inputDir.X, inputDir.Y, 0);
		vel *= Speed * (float)delta;

		Position += vel;

		base._PhysicsProcess(delta);
	}
}
