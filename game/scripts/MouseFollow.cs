using Godot;
using System;

public partial class MouseFollow : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// this.translation = GetViewport().GetMousePosition();
		GD.Print(Position);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = GetViewport().GetMousePosition();
	}
}
