using Godot;
using System;

public partial class SpriteHandler : Node
{
	private static Sprite3D cup;
	private static Sprite3D bowl;
	private static Sprite3D addon;
	public SpriteHandler(Sprite3D cuppa, Sprite3D bowlla, Sprite3D addonna)
	{
		cup = cuppa;
		bowl = bowlla;
		addon = addonna;
	}

	public int SetCup(int state, int curSprite)
	{
		Texture2D texture = cup.Texture;
		int outp = curSprite;
		// state is 0 for coffee, 1 for milk, 2 for veganmilk
		if (state == 0)
		{
			if (curSprite == 0)
			{
				texture = (Texture2D)GD.Load("res://assets/drinks/coffee.png");
				outp = 2;
			}
			if (curSprite == 1)
			{
				texture = (Texture2D)GD.Load("res://assets/drinks/milk_coffee.png");
				outp = 3;
			}
			if (curSprite == 4)
			{
				texture = (Texture2D)GD.Load("res://assets/drinks/veganmilk_coffee.png");
				outp = 5;
			}
		}
		if (state == 1)
		{
			if (curSprite == 0)
			{
				texture = (Texture2D)GD.Load("res://assets/drinks/milk.png");
				outp = 1;
			}
			if (curSprite == 2)
			{
				texture = (Texture2D)GD.Load("res://assets/drinks/milk_coffee.png");
				outp = 3;
			}
		}
		if (state == 2)
		{
			if (curSprite == 0)
			{
				texture = (Texture2D)GD.Load("res://assets/drinks/veganmilk.png");
				outp = 4;
			}
			if (curSprite == 2)
			{
				// todo: replace with veganmilkcoffee
				texture = (Texture2D)GD.Load("res://assets/drinks/veganmilk_coffee.png");
				outp = 5;
			}
		}
		cup.Texture = texture;
		return outp;
	}

	public int SetAddon(AddonType type)
	{
		// create string from enum
		string addonString = type.ToString().ToLower();
		if (type == AddonType.None) addonString = "notopping";

		// load texture based on string
		Texture2D loadedTex = GD.Load<Texture2D>("res://assets/drinks/" + addonString + ".png");
		addon.Texture = loadedTex;

		// return enum parsed to int
		return (int)type;
	}
	
	public int SetBowl(int state)
	{
		Texture2D texture = bowl.Texture;
		
		// 0 for BleuCheese, 1 for Fruit, 2 for Fry
		if (state == 0)
		{
			texture = (Texture2D)GD.Load("res://assets/foods/bleuCheese.png");
		}
		if (state == 1)
		{
			texture = (Texture2D)GD.Load("res://assets/foods/fruit.png");
		}
		if (state == 2)
		{
			texture = (Texture2D)GD.Load("res://assets/foods/fry.png");
		}
		
		return state;
	}

	// only for clearing the cup
	public int EmptyCup()
	{
		cup.Texture = (Texture2D)GD.Load("res://assets/drinks/empty.png");
		return 0;
	}

	public int ClearTopping()
	{
		addon.Texture = (Texture2D)GD.Load("res://assets/drinks/notopping.png");
		return 0;
	}

	public void ResetCup()
	{
		EmptyCup();
		ClearTopping();
	}
}
