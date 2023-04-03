using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateApproach : State
{
	public override void Execute(AIController character)
	{
		if (character.IsDead)											//If shot, die
		{
			Debug.Log("Going from PlayingObjective to Dying");
			character.ChangeState(new StateDie());
		}
		else if (character.EnemySeen() && character.EnemyInRange())     //If see and in range, attack
		{
			Debug.Log("Going from PlayingObjective to Shooting");
			character.ChangeState(new StateShoot());
		}
		else if (!character.EnemySeen())								//If lost sight of enemy, go back to objective
		{
			Debug.Log("Going from Approaching to PlayingObjective");
			character.ChangeState(new StateObjective());
		}
		else
		{
			character.BeApproaching();
		}
	}
}
