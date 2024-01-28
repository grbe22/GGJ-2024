using Godot;
using System;

public partial class Tutorial : Control
{
	public Button playButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playButton = GetNode<Button>("Button");
		playButton.Pressed += ButtonPressed;
	}

	private void ButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/tutorial.tscn");
	}
	
}
