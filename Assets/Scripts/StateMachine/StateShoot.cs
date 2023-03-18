using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateShoot : State
{
	public override void Execute(AIController character)
	{
		if (true)//character.EnemySeen()
		{
			Debug.Log("Going from PlayingShoot to PlayingObjective");
			//character.ChangeState(new StateObjective());
		}
		else if (false)
		{
			Debug.Log("Going from PlayingShoot to PlayingODie");
			//character.ChangeState(new StateDie());
		}
		else
		{
			//character.BeShooting();
		}
	}
}
