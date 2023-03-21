using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDie : State
{
	public override void Execute(AIController character)
	{
		if (true)																//arrived at spawn
		{
			Debug.Log("Going from Dead to spawn and then actually Objective");
			//character.ChangeState(new StateObjective());
		}
		else
		{
			//character.goToSpawn();
		}
	}
}
