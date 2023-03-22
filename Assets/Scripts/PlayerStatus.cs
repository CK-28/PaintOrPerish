using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
	public static float maxHealth = 1.0f;
	private float health = maxHealth;

	private bool dead = false;
	private CharacterController player;

	void Start()
	{
		player = GetComponent<CharacterController>();
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
