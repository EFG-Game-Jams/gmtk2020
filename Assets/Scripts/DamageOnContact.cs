using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
	public float damage;

	private void OnCollisionEnter(Collider other)
	{
		other.GetComponent<Damageable>()?.TakeDamage(damage);
	}
}
