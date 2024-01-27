using Godot;
using System;
namespace MouseFollow {
	public partial class MouseFollow : Node2D
	{
		public bool follow = true;
		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			
		}
		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			if (follow) 
			{
				if (Input.IsActionJustPressed("click")) {
					follow = false;
				}
				Position = GetViewport().GetMousePosition();
			}
		}
	}
}
