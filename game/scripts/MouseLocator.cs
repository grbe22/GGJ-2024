using GGJloserteam.scripts;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace GGJloserteam.scripts
{
	internal class MouseLocator
	{
		public enum Hovered
		{
			None,
			Cannon,
			Trash,
			WorkStation,
			Coffee,
			Milk,
			VeganMilk,
			WhippedCream,
			Mayo,
			Chocolate,
			Caramel,
			BleuCheese,
			Fruit,
			Potato,
			CannonButton
		}
		public Hovered selected;

		public MouseLocator()
		{
			selected = new Hovered();
		}

		public int UpdateSprite(int[] order, SpriteHandler sprites, Boolean addon)
		{
			switch (selected)
			{
				// drinks
				case Hovered.Coffee:
					order[1] = sprites.SetCup(0, order[1]);
					break;
				case Hovered.Milk:
					order[1] = sprites.SetCup(1, order[1]);
					break;
				case Hovered.VeganMilk:
					order[1] = sprites.SetCup(2, order[1]);
					break;

				// trash
				case Hovered.Trash:
					order[1] = sprites.EmptyCup();
					order[2] = sprites.ClearTopping();
					order[0] = sprites.EmptyBowl();
					return 1;

				// food addons
				case Hovered.BleuCheese:
					order[0] = sprites.SetBowl(0);
					break;
				case Hovered.Fruit:
					order[0] = sprites.SetBowl(1);
					break;
				case Hovered.Potato:
					order[0] = sprites.SetBowl(2);
					break;


				// drinkAddons
				case Hovered.WhippedCream:
					if (order[2] == 0 && order[1] == 0 && addon)
					{
						order[2] = sprites.SetAddon(AddonType.WhippedCream);
					}
					break;
				case Hovered.Mayo:
					if (order[2] == 0 && order[1] == 0 && addon)
					{
						order[2] = sprites.SetAddon(AddonType.Mayo);
					}
					break;
				case Hovered.Chocolate:
					if (order[2] == 0 && order[1] == 0 && addon)
					{
						order[2] = sprites.SetAddon(AddonType.Chocolate);
					}
					break;
				case Hovered.Caramel:
					if (order[2] == 0 && order[1] == 0 && addon)
					{
						order[2] = sprites.SetAddon(AddonType.Caramel);
					}
					break;

				default:
					return -1;

				
			}
			return 0;
		}
		
		public void Select(int k) {
			Hovered inp = (Hovered)k;
			selected = inp;
		}
		
		public void Deselect(int k) {
			Hovered inp = (Hovered)k;
			if (selected == inp) {
				selected = Hovered.None;
			}
		}
	}
}
