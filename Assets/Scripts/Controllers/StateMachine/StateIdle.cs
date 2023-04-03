using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : State
{
    public override void Execute(AIController character)
    {
		var gameObject = GameObject.Find("TheGame");
		TheGame gameScript = gameObject.GetComponent<TheGame>();

		if (gameScript.isGameStarted())					// If the game started, start plying objective
		{
			Debug.Log("Game is started. Going from Idle to playing objective");
			character.ChangeState(new StateObjective());
		} else if(character.IsDead)						// If shot, die
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
