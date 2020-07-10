using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupAttractor : Powerup
{
	public float radius;
	public float force;

	protected override void ApplyPowerup(GameObject receiver)
	{
		var attractor = receiver.AddComponent<PowerupEffectAttractor>();
		attractor.radius = radius;
		attractor.force = force;
	}
}
