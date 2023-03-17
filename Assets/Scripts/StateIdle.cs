using UnityEngine;
using System.Collections;

public class StateIdle : State
{
	public override void Execute(AIController character)
	{
		if (character.EnemySeen() && character.EnemyInRange())      //If see and in range, attack
		{
			// character.ChangeState(new StateAttack());
			// launch projectile		
		}
		else if(character.EnemySeen() && !character.EnemyInRange()) //If see and out of range, approach
		{
			// character.ChangeState(new StateApproach());
			// move towards player?
		}
		else if (character.IsDead)									//If attacked till death, die
		{
			// character.ChangeState(new StateDead());
			// retreat
		}
        else
		{
			// character.BeIdle();
		}
	}
}
