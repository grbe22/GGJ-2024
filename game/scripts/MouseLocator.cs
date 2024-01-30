using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private void _on_deselected() { selected = Hovered.None; }

        // ~~ drinks ~~

        // when selecting coffee
        private void _on_coffee_mouse_entered() { selected = Hovered.Coffee; }

        // when selecting normal milk
        private void _on_milk_mouse_entered() { selected = Hovered.Milk; }

        // when selecting veganmilk
        private void _on_vegan_milk_mouse_entered() { selected = Hovered.VeganMilk; }


        // ~~ toppings ~~

        // when selecting whipped cream
        private void _on_whipped_cream_mouse_entered() { selected = Hovered.WhippedCream; }

        // when selecting mayo 
        private void _on_mayo_mouse_entered() { selected = Hovered.Mayo; }

        // when selecting chocolate
        private void _on_chocolate_mouse_entered() { selected = Hovered.Chocolate; }

        // when selecting caramel
        private void _on_caramel_mouse_entered() { selected = Hovered.Caramel; }


        // ~~ foods ~~

        // when selecting bleu cheese
        private void _on_bleu_cheese_mouse_entered() { selected = Hovered.BleuCheese; }

        // when selecting fruit
        private void _on_fruitbowl_mouse_entered() { selected = Hovered.Fruit; }

        // when selecting potato
        private void _on_potato_mouse_entered() { selected = Hovered.Potato; }

        // ~~ misc ~~ 

        // for disposing of drinks
        private void _on_trash_man_mouse_entered() { selected = Hovered.Trash; }

        // for cannons
        private void _on_cannon_mouse_entered() { selected = Hovered.Cannon; }

        // for the specific area around machines
        private void _on_work_area_mouse_entered() { selected = Hovered.WorkStation; }

        // for the button
        private void _on_button_mouse_entered() { selected = Hovered.CannonButton; }
    }
}
