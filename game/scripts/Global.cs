using Godot;
using System;

public partial class Global : Node
{
	// Global score
	public int score = 0;

	// Singleton instance
	private static Global _instance;

	public override void _Ready()
	{
		// Ensure there's only one instance of the singleton
		if (_instance == null)
		{
			_instance = this;
			GD.Print("Global singleton initialized.");
		}
		else
		{
			// If another instance is found, remove it
			QueueFree();
		}
	}
	
	public static Global GetInstance()
	{
		return _instance;
	}
}
