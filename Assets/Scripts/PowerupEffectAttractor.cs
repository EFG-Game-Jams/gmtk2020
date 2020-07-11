using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupEffectAttractor : MonoBehaviour
{
	public float radius;
	public float force;

	private void FixedUpdate()
	{
		Vector3 origin = transform.position;
		var affected = Physics.OverlapSphere(origin, radius);
		foreach (var collider in affected)
		{
			if (collider.gameObject == gameObject)
				continue;

			var rb = collider.GetComponent<Rigidbody>();
			if (rb == null)
				continue;

			float dist = Vector3.Distance(collider.attachedRigidbody.position, origin);
			float distNormalised = Mathf.Clamp01(dist / radius);
			float strength = 1f - distNormalised;

			Vector3 doForce = (origin - collider.attachedRigidbody.position).normalized * (strength * force);
			collider.attachedRigidbody.AddForce(doForce, ForceMode.Force);
		}
	}
}
