using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
	public float damage;

	private void OnCollisionEnter(Collision collision)
	{
		collision.collider.GetComponent<Damageable>()?.TakeDamage(damage);
	}
}
