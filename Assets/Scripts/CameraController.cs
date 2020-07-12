using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	new private Camera camera;
	private Bounds cameraBounds;

	const float zoomSensitivity = 5f;
	const float zoomMin = 2.5f;
	const float smoothTimeZoom = .25f;
	const float smoothTimeTranslate = .25f;

	private float targetZoom;
	private float zoomVelocity;
	private Vector3 targetPosition;
	private Vector3 translationVelocity;

	private Vector3 screenDragStart;
	private Vector3 targetDragStart;
	private bool screenDragging;

	private void Awake()
	{
		camera = GetComponent<Camera>();

		targetZoom = camera.orthographicSize;
		targetPosition = transform.position;
		
		UpdateBounds();
	}

	private void Update()
	{
		if (Input.mouseScrollDelta.y != 0)
		{
			targetZoom -= Input.mouseScrollDelta.y * zoomSensitivity;
			targetPosition = camera.ScreenToWorldPoint(Input.mousePosition);
			screenDragging = false;
		}

		targetZoom = Mathf.Max(targetZoom, zoomMin);
		camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, targetZoom, ref zoomVelocity, smoothTimeZoom);
		if (zoomVelocity > 0)
			camera.orthographicSize = Mathf.Min(targetZoom, camera.orthographicSize);

		if (Input.GetMouseButtonDown(1))
		{
			screenDragging = true;
			screenDragStart = Input.mousePosition;
			targetDragStart = targetPosition;
		}
		else if (screenDragging && !Input.GetMouseButton(1))
		{
			screenDragging = false;
		}

		if (screenDragging)
		{
			Vector3 screenDragNow = Input.mousePosition;
			Vector3 worldDragStart = camera.ScreenToWorldPoint(screenDragStart);
			Vector3 worldDragNow = camera.ScreenToWorldPoint(screenDragNow);

			Vector3 dragError = worldDragStart - worldDragNow;
			targetPosition = targetDragStart + dragError;
		}

		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref translationVelocity, smoothTimeTranslate);
		//transform.position = targetPosition;

		ClampToBounds();
	}

	private void UpdateBounds()
	{
		Bounds bounds = new Bounds();

		var env = GameObject.Find("Environment");
		var renderers = env.GetComponentsInChildren<Renderer>();
		foreach (var renderer in renderers)
			bounds.Encapsulate(renderer.bounds);

		cameraBounds = bounds;
	}

	private Vector2 GetCurrentViewSize()
	{
		return new Vector2(camera.orthographicSize * camera.aspect * 2f, camera.orthographicSize * 2f);
	}
	private Vector2 GetTargetViewSize()
	{
		//return new Vector2(camera.orthographicSize * camera.aspect * 2f, camera.orthographicSize * 2f);
		return new Vector2(targetZoom * camera.aspect * 2f, targetZoom * 2f);
	}
	private void SetTargetViewSize(Vector2 size)
	{
		Vector2 curSize = GetTargetViewSize();
		float ratio = Mathf.Min(size.x / curSize.x, size.y / curSize.y);
		targetZoom *= ratio;
		camera.orthographicSize = Mathf.Min(targetZoom, camera.orthographicSize);
	}

	private void ClampToBounds()
	{
		Vector2 prevViewSize = GetTargetViewSize();
		Vector2 viewSize = prevViewSize;
		if (viewSize.x > cameraBounds.size.x)
			viewSize *= cameraBounds.size.x / viewSize.x;
		if (viewSize.y > cameraBounds.size.y)
			viewSize *= cameraBounds.size.y / viewSize.y;

		if (viewSize != prevViewSize)
			SetTargetViewSize(viewSize);

		Vector3 position = transform.position;
		Bounds viewBounds = new Bounds(position, GetCurrentViewSize());
		position.x += Mathf.Max(0, cameraBounds.min.x - viewBounds.min.x);
		position.y += Mathf.Max(0, cameraBounds.min.y - viewBounds.min.y);
		position.x -= Mathf.Max(0, viewBounds.max.x - cameraBounds.max.x);
		position.y -= Mathf.Max(0, viewBounds.max.y - cameraBounds.max.y);
		transform.position = position;
	}

	private void OnDrawGizmos()
	{
		camera = camera ?? GetComponent<Camera>();
		UpdateBounds();

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(cameraBounds.center, cameraBounds.size);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(transform.position, GetTargetViewSize());
	}
}
