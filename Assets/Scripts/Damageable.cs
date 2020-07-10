using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
	public float multiplier = 1f;
	public float health = 100;
	public float timeToDie = 2f;

	private bool isDetonating;

	public void TakeDamage(float amount)
	{
		if (isDetonating)
			return;

		float damage = amount * multiplier;
		health -= damage;

		if (health <= 0)
		{
			isDetonating = true;
			StartCoroutine(CoDetonate());
		}
	}

	IEnumerator CoDetonate()
	{
		yield return new WaitForSeconds(timeToDie);
		GetComponent<Explosive>()?.Detonate();
		yield return null;
		Destroy(gameObject);
	}
}
