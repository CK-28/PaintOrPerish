using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {
	
	private float	health = 100.0f;
	private float	maxHealth = 100.0f;
	public bool		dead = false;

	private PlayerController playerController;
	
	public void AddHealth(float addedHealth)
	{
		health = health + addedHealth;
		Debug.Log("Gained +10 Health!");
	}
	
	public float GetHealth()
	{
		return health;
	}

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public bool isAlive()
	{
		return !dead;
	}
	
	public void ApplyDamage(float damage)
	{
		health = health - damage;

		if (health <= 0)
		{
			health = 0;
			StartCoroutine(Die());
		}
	}
    
	IEnumerator Die()
	{
		dead = true;
		print("You have died!");
		//playerController.beDead();
		HideCharacter();

		yield return new WaitForSeconds(10);

		print("You have respawned!");
		//playerController.Respawn();
		ShowCharacter();

		health = maxHealth;
		dead = false;
	}
	
	void HideCharacter()
	{	
		//playerController.IsControllable = false;
	}
	
	
	
	void ShowCharacter()
	{
		//playerController.IsControllable = true;
	}
	
}
