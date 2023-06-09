using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDie : State
{
	public override void Execute(AIController character)
	{
		if (!character.IsDead)	// character respawns
		{
			Debug.Log("Respawned. Playing Objective");
			character.ChangeState(new StateObjective());
		}
		else
		{
			character.goToSpawn();
		}
	}
}
