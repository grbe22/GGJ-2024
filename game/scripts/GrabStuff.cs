using Godot;
using System;

public partial class GrabStuff : Node
{
	public Node heldItem;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button cups = GetNode<Button>("CameraSystem/TextureButton/CupButton");
		cups.Pressed += GetCup;
	}
	
	private void GetCup()
	{ 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
