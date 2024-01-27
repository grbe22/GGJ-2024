using Godot;
using System;

public partial class GrabStuff : Node
{
	public Node heldItem;
	public bool hoverCup;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("click") && hoverCup) {
			// todo: spawn cup at mouse position
		}
		//if (Input.IsActionJustPressed("Switch Camera") && Input.IsActionPressed("Switch Camera"))
		//{
			//switchView();
		//}
	}
	
	private void _on_coffee_cup_mouse_entered() {
		hoverCup = true;
	}
	
	private void _on_coffee_cup_mouse_exited() {
		hoverCup = false;
	}
}
