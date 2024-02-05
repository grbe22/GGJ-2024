using Godot;
using System;

public partial class GrabStuff : StaticBody3D
{
	private StaticBody3D heldItem;
	private Vector3 offset;

	/// <summary>
	/// Whether or not object is hovered over
	/// </summary>
	public bool Hovered { get; private set; } = false;

	/// <summary>
	/// Whether or not object is on the back view or not
	/// </summary>
	public bool InBack { get; private set; } = false;

	/// <summary>
	/// Whether or not object is grabbed by cursor
	/// </summary>
	public bool IsGrabbed { get; private set; } = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		offset = this.Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		IsGrabbed = heldItem != null;

		if (Input.IsActionJustPressed("click"))
		{
			if (IsGrabbed)
			{
				GD.Print("drop", heldItem.Position);
				heldItem = null;
			}
			else if (Hovered)
			{
				heldItem = this;
			}
		}
		else if (Input.IsActionJustPressed("Switch Camera") && Input.IsActionPressed("Switch Camera"))
		{
			if (InBack)
			{
				InBack = false;
				if (IsGrabbed)
				{
					heldItem.Position = new Vector3((float)9.7, (float)1.6, 0);
				}
			}
			else
			{
				InBack = true;
				if (IsGrabbed)
				{
					heldItem.Position = new Vector3((float)9.7, (float)1.6, (float)28.6);
				}
			}

		}
		else if (IsGrabbed)
		{
			Vector2 mousePos = GetViewport().GetMousePosition();
			mousePos.Y = (mousePos.Y / 648f) * 24.5f;
			mousePos.X = (mousePos.X / 1152f) * 39f;
			Vector3 newVector;
			if (InBack)
			{
				// the object should stay exactly 30 z away from the camera.
				// for this, 30z is 30.
				// every 31.6 mouse pixels is 1 y pixel.
				newVector = new Vector3(18f - mousePos.X, 16f - mousePos.Y, 30f);
				heldItem.Position = newVector;
			}
			else
			{
				// 30z in this is instead 28.5
				newVector = new Vector3(-1 * (22f - mousePos.X), 18f - mousePos.Y, 0f);
			}
			heldItem.Position = newVector;
		}
	}

	private void _on_self_mouse_entered()
	{
		Hovered = true;
	}

	private void _on_self_mouse_exited()
	{
		Hovered = false;
	}
}
