using Godot;
using System;

public partial class ProjectileController : RigidBody3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	/// <summary>
	/// Launches this projectile (via impulse)
	/// </summary>
	/// <param name="direction">Desired direction</param>
	/// <param name="strength">Strength multiplier of direction</param>
	public void Launch(Vector3 direction, float strength)
	{
		Vector3 impulse = direction * strength;
		ApplyImpulse(impulse);
	}
}
