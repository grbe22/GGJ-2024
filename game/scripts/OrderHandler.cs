using Godot;
using System;

public partial class OrderHandler : Node
{
	private OrderScript orderScript;
	private int[] order;
	private string speech;
	public OrderHandler()
	{
		orderScript = new OrderScript();
		order = orderScript.GenerateOrder();
		speech = orderScript.PrintOrder(order);
	}

	public string GetOrder() {
		return speech;
	}
	
	public void CreateSpeech(string phrase, Vector3 position) {
		return;
	}
}
