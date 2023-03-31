using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : State
{
    public override void Execute(AIController character)
    {
		Debug.Log("Pickles");
		var gameObject = GameObject.Find("TheGame");
		TheGame gameScript = gameObject.GetComponent<TheGame>();

		// If the game started, start plying objective
		if (gameScript.isGameStarted())
		{
			Debug.Log("Game is started. Going from Idle to playing objective");
			character.ChangeState(new StateObjective());
		} else if(character.IsDead)
        {
			Debug.Log("Dead. Going back to spawn");
			character.ChangeState(new StateDie());
        }
		else
		{
			character.BeIdle();
		}
	}
}
