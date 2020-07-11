using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
	public float multiplier = 1f;
	public float health = 100;
	public float timeToDie = 2f;

	public void TakeDamage(float amount)
	{
		float damage = amount * multiplier;
		health -= damage;

		if (health <= 0)
		{
			GetComponent<Fuse>()?.LightFuse(timeToDie);
		}

		GetComponent<Bomb>()?.PlayDamageFx();
	}
}
