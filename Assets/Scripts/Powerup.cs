using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
	private bool applied;

	protected abstract void ApplyPowerup(GameObject receiver);

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!applied)
		{
			applied = true;
			ApplyPowerup(collision.gameObject);
			Destroy(gameObject);
		}
	}
}
