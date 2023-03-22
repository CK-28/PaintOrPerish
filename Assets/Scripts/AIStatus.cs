using UnityEngine;
using System.Collections;

public class AIStatus : MonoBehaviour
{
	public static float maxHealth = 1.0f;
	private float health = maxHealth;

	private bool dead = false;
	private AIController aiController;

	void Start()
	{
		aiController = GetComponent<AIController>();
	}

	public bool isAlive()
	{
		return !dead;
	}

	public void onHit()
	{
		dead = true;
	}

	public void onStart()
    {
		dead = false;
    }
}
