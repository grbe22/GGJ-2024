using Godot;
using System;

public partial class OrderHandler : Node
{
	private OrderScript orderScript;
	private string speech;

	/// <summary>
	/// Current order of customer
	/// </summary>
	public int[] Order { get; private set; }

	public OrderHandler()
	{
		orderScript = new OrderScript();
		Order = orderScript.GenerateOrder();
		speech = orderScript.PrintOrder(Order);
	}

	public string GetSpeech()
	{
		return speech;
	}

	public void CreateSpeech(string phrase, Vector3 position)
	{
		return;
	}
}
