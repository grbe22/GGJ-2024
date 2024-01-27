using Godot;
using System;
using System.Diagnostics;
using System.Transactions;

public partial class CustomerController : CharacterBody3D
{
	public float Speed { get; set; } = 10f;
	public bool EnableFloppy { get; set; } = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Movement testing controller ready");
		SetFloppy(EnableFloppy);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_down", "move_up");
		int z = 0;
		if (Input.IsActionPressed("move_close")) {
			z = 1;
		} else if (Input.IsActionPressed("move_far")) {
			z = -1;
		}
		Vector3 vel = new(inputDir.X, inputDir.Y, z);
		vel *= Speed * (float)delta;

		Position += vel;

		base._PhysicsProcess(delta);
	}

	/// <summary>
	/// Sets whether customer is floppy or not 
	/// (unlock x/y rotation and make head fall)
	/// </summary>
	/// <param name="enabled">Desired floppy value</param>
	public void SetFloppy(bool enabled)
	{
		SetFloppy(this, enabled);
		GD.Print("Floppy value set to " + enabled);
	}

	/// <summary>
	/// Recursive method sets value of the floppy parameter in children nodes
	/// </summary>
	/// <param name="node">Node to check children of</param>
	private void SetFloppy(Node node, bool enabled)
	{
		LimbController controller = node as LimbController;
		if (controller != null)
		{
			controller.Floppy = enabled;
		}

		// exit condition when there are no children
		foreach (Node child in node.GetChildren())
		{
			SetFloppy(child, enabled);
		}
	}
}
