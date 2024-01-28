using Godot;
using System;

public partial class GrabStuff : StaticBody3D
{
	private StaticBody3D heldItem;
	private bool hoverCup = false;
	private Vector3 offset;
	private bool back = false;
	
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
		else if (Input.IsActionJustPressed("Switch Camera") && Input.IsActionPressed("Switch Camera")) {
			if (back) {
				back = false;
				if (heldItem != null) {
				heldItem.Position = new Vector3((float) 9.7, (float) 1.6, 0);
			}
			}
			else {
				back = true;
				if (heldItem != null) {
				heldItem.Position = new Vector3((float) 9.7, (float) 1.6, (float) 28.6);
			}
			}
			
			
		}
		else if (heldItem != null) {
			if (back)
			{
				Vector2 mousePos = GetViewport().GetMousePosition();
				mousePos.Y /= 60f;
				mousePos.X /= 30f;
				mousePos *= -1f;
				Vector3 newVector = new Vector3(mousePos.X + 18, mousePos.Y + 8, heldItem.Position.Z);
				
				heldItem.Position = newVector;
			}
			else {
				Vector2 mousePos = GetViewport().GetMousePosition();
				mousePos.Y /= -50f;
				mousePos.X /= 30f;
				Vector3 newVector = new Vector3(mousePos.X - 23, mousePos.Y + 8, heldItem.Position.Z);
				
				heldItem.Position = newVector;
			}
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
