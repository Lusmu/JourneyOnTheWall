using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JourneyOnTheWall
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField]
		private float speed = 10;
		[SerializeField]
		private float damping = 0.05f;
		[SerializeField]
		private float minFov = 20;
		[SerializeField]
		private float maxFov = 80;
		[SerializeField]
		private float zoomSpeedScroll = 10;
		[SerializeField]
		private float zoomSpeedPinch = 10;
		[SerializeField]
		private RectTransform backCollider;

		private bool isDragging = false;

		private Transform tr;

		private float momentum;

		private Vector3 lastMousePosition;

		private List<Vector3> lastTouches = new List<Vector3>();

		private Vector3 momentumDir;

		private Camera targetCamera;

		void Awake()
		{
			tr = GetComponent<Transform>();
			targetCamera = GetComponent<Camera>();
		}

		public void BeginTouch()
		{
			isDragging = true;

			lastMousePosition = Input.mousePosition;

			lastTouches.Clear();
		}

		public void EndTouch()
		{
			isDragging = false;

			momentumDir = Vector3.zero;

			if (lastTouches.Count > 10)
			{
				for (int i = 0; i < lastTouches.Count; i ++)
				{
					momentumDir += lastTouches[i];
				}
				
				momentumDir /= lastTouches.Count;

				momentum = Mathf.Clamp(momentumDir.magnitude * damping * lastTouches.Count / 30, 0, 1);
			}
			else 
			{
				momentum = 0;
			}
		}

		void Update()
		{
			var newRotation = tr.eulerAngles;

			if (isDragging)
			{
				var diff = Input.mousePosition - lastMousePosition;

				newRotation = tr.eulerAngles - diff.x * Vector3.up * speed + diff.y * Vector3.right * speed;

				lastTouches.Add(Input.mousePosition - lastMousePosition);

				if (lastTouches.Count > 15) lastTouches.RemoveAt(0);
			}
			else if (momentum > 0)
			{
				newRotation = tr.eulerAngles - momentum * momentumDir.x * Vector3.up * speed + momentum * momentumDir.y * Vector3.right * speed;

				momentum -= Time.deltaTime;
			}

			if (Input.touchCount == 2)
			{
				// Store both touches.
				Touch touchZero = Input.GetTouch(0);
				Touch touchOne = Input.GetTouch(1);
				
				// Find the position in the previous frame of each touch.
				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
				
				// Find the magnitude of the vector (the distance) between the touches in each frame.
				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
				
				// Find the difference in the distances between each frame.
				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

				// Otherwise change the field of view based on the change in distance between the touches.
				targetCamera.fieldOfView += deltaMagnitudeDiff * zoomSpeedPinch;

				targetCamera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFov, maxFov);
			}
			else
			{
				var zoomSpeed = Input.mouseScrollDelta.y * zoomSpeedScroll;
				targetCamera.fieldOfView = Mathf.Clamp(targetCamera.fieldOfView - zoomSpeed, minFov, maxFov);
			}
			
			newRotation.x = Mathf.Clamp(newRotation.x, 270 + targetCamera.fieldOfView * 0.5f, 360 - targetCamera.fieldOfView * 0.5f);

			tr.eulerAngles = newRotation;

			lastMousePosition = Input.mousePosition;

			backCollider.sizeDelta = new Vector2(Screen.width, Screen.height);
		}
	}
}