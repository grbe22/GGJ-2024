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
			if (InBack)
			{
				Vector2 mousePos = GetViewport().GetMousePosition();
				mousePos.Y /= 60f;
				mousePos.X /= 30f;
				mousePos *= -1f;
				Vector3 newVector = new Vector3(mousePos.X + 18, mousePos.Y + 8, heldItem.Position.Z);

				heldItem.Position = newVector;
			}
			else
			{
				Vector2 mousePos = GetViewport().GetMousePosition();
				mousePos.Y /= -50f;
				mousePos.X /= 30f;
				Vector3 newVector = new Vector3(mousePos.X - 23, mousePos.Y + 8, heldItem.Position.Z);

				heldItem.Position = newVector;
			}
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
