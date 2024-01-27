using Godot;
using System;

public partial class GrabStuff : Node
{
	public Node heldItem;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		 Node cups = GetNode("Environment/BackCounter/CoffeeCup");
	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
