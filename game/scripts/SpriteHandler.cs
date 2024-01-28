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
	
	public void setCup(int state) {
		Texture2D texture;
		Switch state {
			case 0:
				texture = (Texture2D)GD.Load("res://assets/drinks/none.png");
				break;
			case 1:
				texture = (Texture2D)GD.Load("res://assets/drinks/milk.png");
				break;
			case 2:
				texture = (Texture2D)GD.Load("res://assets/drinks/coffee.png");
				break;
			case 3:
				texture = (Texture2D)GD.Load("res://assets/drinks/milk_coffee.png");
				break;
			case 4:
				texture = (Texture2D)GD.Load("res://assets/drinks/milk.png");
				break;
			case 5:
				texture = (Texture2D)GD.Load("res://assets/drinks/milk.png");
				break;
		}
		cup.Texture = texture;
	}
}
