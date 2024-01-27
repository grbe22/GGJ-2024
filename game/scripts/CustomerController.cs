using Godot;
using System;
using System.Diagnostics;

public partial class CustomerController : CharacterBody3D
{
	public float Speed { get; set; } = 10f;
	public bool EnableFloppy { get; set; } = false;
	private OrderHandler orders;
	private bool floppyPrevState = false;
	private int numImpulses = 0;
	private Vector3 launchDir;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetFloppy(EnableFloppy);
		SetTexturePerson("res://assets/ophie");
		orders = new OrderHandler();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (EnableFloppy != floppyPrevState)
		{
			SetFloppy(EnableFloppy);
		}

		floppyPrevState = EnableFloppy;
	}

	public override void _PhysicsProcess(double delta)
	{
		// movement testing
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_down", "move_up");
		int z = 0;
		if (Input.IsActionPressed("move_close"))
		{
			z = 1;
		}
		else if (Input.IsActionPressed("move_far"))
		{
			z = -1;
		}
		Vector3 vel = new(inputDir.X, inputDir.Y, z);
		vel *= Speed * (float)delta;

		Position += vel;

		// launching logic
		if (numImpulses > 0)
		{
			ApplyBodyImpulse(this);
			numImpulses--;
		}

		base._PhysicsProcess(delta);
	}

	/// <summary>
	/// Launches this customer at a desired direction
	/// </summary>
	/// <param name="numImpulses">Number of iterations/impulses to apply</param>
	/// <param name="direction">Direction to launch customer</param>
	public void Launch(int numImpulses, Vector3 direction)
	{
		EnableFloppy = true;
		this.numImpulses = numImpulses;
		this.launchDir = direction;
	}

	/// <summary>
	/// Sets whether customer is floppy or not 
	/// (unlock x/y rotation and make head fall)
	/// </summary>
	/// <param name="enabled">Desired floppy value</param>
	private void SetFloppy(bool enabled)
	{
		SetFloppy(this, enabled);
		GD.Print("Floppy value set to " + EnableFloppy);
	}

	/// <summary>
	/// Recursive method sets value of the floppy parameter in children nodes
	/// </summary>
	/// <param name="node">Node to check children of</param>
	private void SetFloppy(Node node, bool enabled)
	{
		LimbController controller = node as LimbController;
		if (controller != null)
		{
			controller.Floppy = enabled;
		}

		// exit condition when there are no children
		foreach (Node child in node.GetChildren())
		{
			SetFloppy(child, enabled);
		}
	}

	private void SetTexturePerson(string personFolderPath)
	{
		SetTexturePerson(this, personFolderPath);
	}

	private void SetTexturePerson(Node node, string personFolderPath)
	{
		// update texture if this is a mesh
		if (node is MeshInstance3D mesh)
		{
			string fileName = GetFilenameFromMesh(mesh);
			GD.Print("File name: " + fileName);
			ApplyTexture(mesh, $"{personFolderPath}/{fileName}");
		}

		// exit condition when there are no child nodes
		foreach (Node child in node.GetChildren())
		{
			SetTexturePerson(child, personFolderPath);
		}
	}

	private void ApplyTexture(MeshInstance3D mesh, string texturePath)
	{
		// get references 
		Image img = Image.LoadFromFile(texturePath);
		ImageTexture texture = ImageTexture.CreateFromImage(img);
		Material material = mesh.GetSurfaceOverrideMaterial(0);

		// update texture
		material.Set("albedo_texture", texture);
	}

	/// <summary>
	/// Gets a string path of the correct file from the mesh name
	/// </summary>
	/// <param name="mesh">Reference of mesh</param>
	/// <returns>String of file</returns>
	private string GetFilenameFromMesh(MeshInstance3D mesh)
	{
		string name = mesh.Name;
		if (name.Contains("MESH-"))
		{
			name = name.Substring(5);
		}

		name += ".png";

		return name;
	}

	/// <summary>
	/// Applies an impulse to only the body rigidbody
	/// </summary>
	/// <param name="node">Root node, to search recursively</param>
	private void ApplyBodyImpulse(Node node)
	{
		if (node is RigidBody3D rb && rb.Name == "body")
		{
			Vector3 impulse = launchDir * 100;
			rb.ApplyImpulse(impulse);
			return;
		}

		foreach (Node child in node.GetChildren())
		{
			ApplyBodyImpulse(child);
		}
	}
}
