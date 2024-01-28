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
			}
		}
		else if (heldItem != null) {
			Vector2 mousePos = GetViewport().GetMousePosition();
			Vector3 self = new Vector3(mousePos[0] / 100, mousePos[1] / 100, offset[2]);
			mousePos.Y /= 60f;
			mousePos.X /= 30f;
			mousePos *= -1f;
			Vector3 newVector = new Vector3(mousePos.X + 18, mousePos.Y + 8, heldItem.Position.Z);
			
			heldItem.Position = newVector;
		}
		else {
		}
	}
	
	private void _on_self_mouse_entered() {
		hoverCup = true;
	}
	
	private void _on_self_mouse_exited() {
		hoverCup = false;
	}
}
