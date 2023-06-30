using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Playing the Objective state
public class StateObjective : State
{
	public override void Execute(AIController character)
	{
		if (character.IsDead)						//If shot, go to die
		{
			Debug.Log("Going from PlayingObjective to Dead");
			character.ChangeState(new StateDie());
		}
		else if (character.EnemySeen() && character.EnemyInRange())     //If enemy is seen and in range, go to attack them
		{
			Debug.Log("Going from PlayingObjective to Attacking");
			character.ChangeState(new StateShoot());
		}
		else if (character.EnemySeen() && !character.EnemyInRange())	//If enemy is seen and out of range, go to approach them
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
