using Godot;
using System;

public partial class SpriteHandler : Node
{
	private static Sprite3D cup;
	private static Sprite3D bowl;
	public SpriteHandler(Sprite3D cuppa, Sprite3D bowlla) {
		cup = cuppa;
		bowl = bowlla;
	}
	
	public int SetCup(int state, int curSprite) {
		Texture2D texture = cup.Texture;
		int outp = curSprite;
		if (state == 0) {
			if (curSprite == 0) {
				texture = (Texture2D)GD.Load("res://assets/drinks/coffee.png");
				outp = 2;
			}
			if (curSprite == 1) {
				texture = (Texture2D)GD.Load("res://assets/drinks/milk_coffee.png");
				outp = 3;
			}
			if (curSprite == 4) {
				texture = (Texture2D)GD.Load("res://assets/drinks/veganmilk_coffee.png");
				outp = 5;
			}
		}
		if (state == 1) {
			if (curSprite == 0) {
				texture = (Texture2D)GD.Load("res://assets/drinks/milk.png");
				outp = 1;
			}
			if (curSprite == 2) {
				texture = (Texture2D)GD.Load("res://assets/drinks/milk_coffee.png");
				outp = 3;
			}
		}
		if (state == 2) {
			if (curSprite == 0) {
				texture = (Texture2D)GD.Load("res://assets/drinks/veganmilk.png");
				outp = 4;
			}
			if (curSprite == 2) {
				// todo: replace with veganmilkcoffee
				texture = (Texture2D)GD.Load("res://assets/drinks/veganmilk_coffee.png");
				outp = 5;
			}
		}
		cup.Texture = texture;
		return outp;
	}
	
	// only for clearing the cup
	public int EmptyCup() {
		cup.Texture = (Texture2D)GD.Load("res://assets/drinks/empty.png");
		return 0;
	}
}