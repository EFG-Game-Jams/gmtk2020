using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseInteractionHandler : TransientSingleton<FuseInteractionHandler>
{
	private Fuse currentTarget;
	private Vector3 prevMousePos;

	public void BeginInteraction(Fuse fuse)
	{
		if (SimulationState.Instance.CurrentMode != SimulationState.Mode.Edit)
			return;
		
		if (currentTarget != null)
			return;

		if (fuse.timeToDetonate <= 0 && !SimulationState.Instance.CanFuseBomb)
		{
			UiSoundFx.GetOrCreate().PlayAdjustFuseDenied();
			return;
		}

		currentTarget = fuse;
		prevMousePos = Input.mousePosition;

		fuse.GetComponent<Bomb>()?.PlaySelectFx();
	}

	private void Update()
	{
		const float distanceThreshold = 5;
		const float fuseIncrement = .5f;
		const float fuseTimeMax = 10f;

		if (currentTarget == null)
			return; // not active

		if (!Input.GetMouseButton(0))
		{
			currentTarget = null;
			return; // released LMB
		}

		// get mouse movement
		Vector3 newMousePos = Input.mousePosition;
		Vector3 mouseDelta = newMousePos - prevMousePos;
		float delta = mouseDelta.x + mouseDelta.y + mouseDelta.z;
		float distance = Mathf.Abs(delta);

		if (distance > distanceThreshold)
		{
			// calculate fuse time change
			int increments = Mathf.FloorToInt(distance / distanceThreshold);
			float change = increments * fuseIncrement * Mathf.Sign(delta);

			// update fuse
			float prevFuseTime = currentTarget.timeToDetonate;
			float newFuseTime = Mathf.Clamp(prevFuseTime + change, 0, fuseTimeMax);
			if (newFuseTime != prevFuseTime)
			{
				if (prevFuseTime <= 0)
					SimulationState.Instance.OnBombFused();
				else if (newFuseTime <= 0)
					SimulationState.Instance.OnBombDefused();

				UiSoundFx.GetOrCreate().PlayAdjustFuse();
				currentTarget.SetTimeToDetonate(newFuseTime);
			}

			// update recorded mouse position, carrying over any remainder
			prevMousePos = newMousePos;
			prevMousePos.x -= (distance % distanceThreshold) * Mathf.Sign(delta);
		}
	}
}
