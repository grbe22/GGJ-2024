using Godot;
using System;

public partial class GrabStuff : StaticBody3D
{
	private StaticBody3D heldItem;
	private bool hoverCup = false;
	private Vector3 offset;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		offset = GetNode<StaticBody3D>("%Cup").Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("click")) {
			if (heldItem != null)
			{
				GD.Print("drop", heldItem.Position);
				heldItem = null;
			}
			else if (hoverCup)
			{
				heldItem = GetNode<StaticBody3D>("%Cup");
				GD.Print("pickup");
			}
		}
		else if (heldItem != null) {
			Vector2 mousePos = GetViewport().GetMousePosition();
			Vector3 self = new Vector3(mousePos[0] / 100, mousePos[1] / 100, offset[2]);
			
			heldItem.Position = self;
			GD.Print("move", mousePos);
		}
		else {
			GD.Print(offset);
		}
	}
	
	private void _on_self_mouse_entered() {
		hoverCup = true;
	}
	
	private void _on_self_mouse_exited() {
		hoverCup = false;
	}
}
