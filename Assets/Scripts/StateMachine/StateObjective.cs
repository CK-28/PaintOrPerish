using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateObjective : State
{
	public override void Execute(AIController character)
	{
		if (character.IsDead)											//If shot, die
		{
			Debug.Log("Going from PlayingObjective to Dead");
			character.ChangeState(new StateDie());
		}
		else if (character.EnemySeen() && character.EnemyInRange())     //If see and in range, attack
		{
			Debug.Log("Going from PlayingObjective to Attacking");
			character.ChangeState(new StateShoot());
		}
		else if (character.EnemySeen() && !character.EnemyInRange())	//If see and out of range, approach
		{
			Debug.Log("Going from PlayingObjective to Approaching");
			character.ChangeState(new StateApproach());
		}
		else
		{
			character.BeObjective();
		}
	}
}
