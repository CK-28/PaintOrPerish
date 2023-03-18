using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateObjective : State
{
	public override void Execute(AIController character)
	{
		if (true)//character.EnemySeen()
		{
			Debug.Log("Going from PlayingObjective to Shooting");
			//character.ChangeState(new StateShoot());
		}
		else if (false)//gotted shotted
        {
			Debug.Log("Going from PlayingObjective to Dying");
			//character.ChangeState(new StateDie());
		}
		else
		{
			//character.BeObjective();
		}
	}
}
