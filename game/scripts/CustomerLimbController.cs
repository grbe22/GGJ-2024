using Godot;
using System;

public partial class CustomerLimbController : RigidBody3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		bool locked = true;

		// iterates until current is the root node
		Node current = this;
		while (current.Name != "Customer")
		{
			current = current.GetParent();
		}

		bool hasData = current.HasMeta("enableFloppy");
		if (hasData)
		{
			locked = !(bool)GetMeta("Enable Floppy");
			GD.Print("found floppy thing? value: " + !locked);
		}

		AxisLockAngularY = true;
		AxisLockAngularX = locked;
	}
}
