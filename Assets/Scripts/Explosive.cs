using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
	public float radius;
	public float force;
	public float damage;

	public void Detonate()
	{
		Vector2 origin = transform.position;
		var affected = Physics2D.OverlapCircleAll(origin, radius);
		foreach (var collider in affected)
		{
			if (collider.gameObject == gameObject)
				continue;

			var damageable = collider.GetComponent<Damageable>();
			if (damageable == null)
				continue;

			float dist = Vector3.Distance(collider.attachedRigidbody.position, origin);
			float distNormalised = Mathf.Clamp01(dist / radius);
			float strength = 1f - distNormalised;

			Vector2 doForce = (collider.attachedRigidbody.position - origin).normalized * (strength * force);
			collider.attachedRigidbody.AddForce(doForce, ForceMode2D.Force);

			float doDamage = strength * damage;
			damageable.TakeDamage(doDamage);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
