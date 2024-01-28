using Godot;
using System;

public partial class GrabStuff : StaticBody2D
{
	public StaticBody2D heldItem;
	public bool hover;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("click") && hover && heldItem == null) {
			heldItem = GetNode<StaticBody2D>("Cup");
			GD.Print("grab");
		}
		else if (Input.IsActionJustPressed("click") && heldItem != null)
		{
			heldItem = null;
			GD.Print("Drop");
		}
		else if (heldItem != null) 
		{
			heldItem.Position = GetViewport().GetMousePosition();
			GD.Print("move");
		}
	}
	
	private void _on_self_mouse_entered() {
		hover = true;
	}
	
	private void _on_self_mouse_exited() {
		hover = false;
	}
	
	
}
