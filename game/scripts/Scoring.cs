using Godot;
using System;

public partial class Scoring : Node
{
	public Scoring() {}
	
	private int GradeDrink(int request, int provided) {
		// returns a plain constant for multiplication
		if (request == provided) {
			return 2;
		}
		if (request == 5 && (provided == 4 || provided == 2)) { return 1; }
		if (request == 3 && (provided == 1 || provided == 2)) { return 1; }
		return 0;
	}
	
	public int Grade(int[] order, int[] provided) {
		
		int finalScore = 0;
		// element 0 is bowl, 1 is cup, 2 is addon.
		if (order[2] == 0) {
			
			// case they want a plain drink
			int x = GradeDrink(order[1], provided[1]);
			finalScore += x * 35;
			
		} else {
			
			// case they just want a topped drink
			if (provided[2] == order[2]) {
				finalScore += 20;
			}
			int x = GradeDrink(order[1], provided[1]);
			finalScore += x * 25;
			
		}
		if (order[0] == provided[0]) {
			finalScore += 30;
		}
		if (order[0] == 0 && provided[0] == 0 && finalScore == 30) { 
			finalScore += 70;
		}
		return finalScore;
	}
}
