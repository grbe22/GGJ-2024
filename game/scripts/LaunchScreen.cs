using Godot;
using System;

public partial class LaunchScreen : Control
{
	public Button playButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playButton = GetNode<Button>("Button");
		playButton.Pressed += ButtonPressed;
		if (this.Name == "EndScreen") {
			Label lab = GetNode<Label>("../EndScreen/ScoreCard");
			lab.Text = "Score: " + Global.GetInstance().score;
		}
	}

	private void ButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/CameraFlip.tscn");
	}
	
}
