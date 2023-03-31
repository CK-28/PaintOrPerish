using UnityEngine;
using System.Collections;

public abstract class AIController : MonoBehaviour
{
    public State currentState;
    public bool isDead = false;
    public GameObject[] enemies;

    // Determines objective
    public enum Role { Defend, Roam };
    public Role myRole;

    public abstract bool EnemyInRange();
    public abstract bool EnemySeen();
    public abstract void BeIdle();
    public abstract void BeApproaching();
    public abstract void BeShooting();
    public abstract void BeObjective();
    public abstract void goToSpawn();

    // Checking if character is dead
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    public void ChangeState(State newState)
    {
        currentState = newState;
    }

    public Role getRole()
    {
        return myRole;
    }
}
