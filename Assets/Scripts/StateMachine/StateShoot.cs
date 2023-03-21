using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateShoot : State
{
	public override void Execute(AIController character)
	{
		if (character.IsDead)												//If shot, die
		{
			Debug.Log("Going from PlayingObjective to Dying");
			//character.ChangeState(new StateDie());
		}
		else if(character.EnemySeen() && !character.EnemyInRange())			//If see and out of range, approach
		{
			Debug.Log("Going from PlayingShoot to Apprach");
			character.ChangeState(new StateApproach());
		}
		else if (!character.EnemySeen())									//If lost sight of enemy, go back to objective
		{
			Debug.Log("Going from PlayingShoot to PlayingObjective");
			character.ChangeState(new StateObjective());
		}
		else
		{
			character.BeShooting();
		}
	}
}
