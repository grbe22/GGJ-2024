using Godot;
using System;
using System.Diagnostics;

public partial class OrderScript : Node
{
	// none, milk, coffee, and milk coffee
	// I'm going to randomize between 1 and Max, inclusively
	private const int drinks = 6;
	private static readonly string[] stringDrinks = { "None", "milk", "coffee", "milk coffee", "vegan milk", "vegan milk coffee" };
	// none, froth, sugar
	private const int drinkAddons = 5;
	private static readonly string[] stringAddons = { "None", "whipped cream", "mayo", "chocolate", "caramel" };
	// none, bleu cheese, fries 
	private const int foods = 4;
	private static readonly string[] stringFoods = { "None", "bleu cheese", "small fry", "fruit" };

	public string PrintOrder(int[] deep) {
		int drink = deep[1];
		int food = deep[0];
		int addon = deep[2];
		string outString = "Can I get a ";
		if (drink != 0) {
			outString = outString + stringDrinks[drink];	
			if (addon != 0) {
				outString = outString + " with " + stringAddons[addon];
			}
			if (food != 0) {
				outString = outString + " and a side of " + stringFoods[food];
			}
			outString = outString + "?";
		} else {
			outString = outString + stringFoods[food] + "?";
		}
		return outString;
	}

	public int[] GenerateOrder() {
		Random gen = new Random();
		// 40% chance to have a drink
		// 40% to have a drink and a food
		// 40% of orders with a drink will have a topping
		// 20% to just have a food
		int order_qualities = gen.Next(1, 11);
		int food = 0;
		int drink = 0;
		int addon = 0;
		if (order_qualities < 8) {
			drink = gen.Next(1, drinks);
			if (gen.Next(1, 3) == 1) {
				addon = gen.Next(1, drinkAddons);
			}
		} 
		if (order_qualities > 4) {
			food = gen.Next(1, foods);
		}
		int[] order = new int[3];
		order[0] = food;
		order[1] = drink;
		order[2] = addon;
		return order;
	}
}
