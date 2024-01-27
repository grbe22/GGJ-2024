using Godot;
using System;

public partial class OrderHandler : Node
{
	private OrderScript orderScript;
	private int[] order;
	public OrderHandler()
	{
		orderScript = new OrderScript();
		order = orderScript.GenerateOrder();
		GD.Print(orderScript.PrintOrder(order));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
