using Godot;
using System;

public partial class SpriteHandler : Node
{
	private static Sprite2D cup;
	private static Sprite2D bowl;
	public SpriteHandler(Sprite2D cup, Sprite2D bowl) {
		this.cup = cup;
		this.bowl = bowl;
	}
	
	public void setCup(int state, int curSprite) {
		// texture = (Texture2D)GD.Load("res://assets/drinks/none.png");
		Texture2D texture;
		if (state == 0) {
			if (curSprite == 0) {
				texture = (Texture2D)GD.Load("res://assets/drinks/coffee.png");
			}
			if (curSprite == 1) {
				texture = (Texture2D)GD.Load("res://assets/drinks/milk_coffee.png");
			}
			if (curSprite == 4) {
				// todo: replace with veganmmilkcoffee
				texture = (Texture2D)GD.Load("res://assets/drinks/milk_coffee.png");
			}
		}
		if (state == 1) {
			if (curSprite == 0) {
				texture = (Texture2D)GD.Load("res://assets/drinks/mlik.png");
			}
			if (curSprite == 2) {
				texture = (Texture2D)GD.Load("res://assets/drinks/milk_coffee.png");
			}
		}
		if (state == 2) {
			if (curSprite == 0) {
				// todo: replace with veganmilk
				texture = (Texture2D)GD.Load("res://assets/drinks/milk.png");
			}
			if (curSprite == 2) {
				// todo: replace with veganmilkcoffee
				texture = (Texture2D)GD.Load("res://assets/drinks/milk_coffee.png");
			}
		}
		cup.Texture = texture;
	}
}
