using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
	public GameObject explosionVisualEffect;

	public float radius;
	public float force;
	public float damage;

	public void Detonate()
	{
		Vector3 origin = transform.position;
		var affected = Physics.OverlapSphere(origin, radius);
		foreach (var collider in affected)
		{
			if (collider.gameObject == gameObject)
				continue;

			var damageable = collider.GetComponent<Damageable>();
			if (damageable == null)
				continue;

			float dist = Vector3.Distance(collider.attachedRigidbody.position, origin);
			float distNormalised = Mathf.Clamp01(dist / radius);
			float strength = 1f - Mathf.Sqrt(distNormalised);

			Vector3 doForce = (collider.attachedRigidbody.position - origin).normalized * (strength * force);
			collider.attachedRigidbody.AddForce(doForce, ForceMode.Force);

			float doDamage = strength * damage;
			damageable.TakeDamage(doDamage);
		}

		if (explosionVisualEffect != null)
			Instantiate(explosionVisualEffect, transform.position, Quaternion.identity);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
