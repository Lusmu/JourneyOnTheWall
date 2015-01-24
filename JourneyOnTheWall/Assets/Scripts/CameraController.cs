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

		private bool isDragging = false;

		private Transform tr;

		private float momentum;

		private Vector3 lastMousePosition;

		private List<Vector3> lastTouches = new List<Vector3>();

		private Vector3 momentumDir;

		void Awake()
		{
			tr = GetComponent<Transform>();
		}

		public void BeginTouch()
		{
			isDragging = true;

			lastTouches.Clear();
		}

		public void EndTouch()
		{
			isDragging = false;

			momentumDir = Vector3.zero;

			if (lastTouches.Count > 5)
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
			if (isDragging)
			{
				var diff = Input.mousePosition - lastMousePosition;

				var newRotation = tr.eulerAngles - diff.x * Vector3.up * speed + diff.y * Vector3.right * speed;
				newRotation.x = Mathf.Clamp(newRotation.x, 300, 330);

				tr.eulerAngles = newRotation;

				lastTouches.Add(Input.mousePosition - lastMousePosition);

				if (lastTouches.Count > 30) lastTouches.RemoveAt(0);
			}
			else if (momentum > 0)
			{

				var newRotation = tr.eulerAngles - momentum * momentumDir.x * Vector3.up * speed + momentum * momentumDir.y * Vector3.right * speed;
				newRotation.x = Mathf.Clamp(newRotation.x, 300, 330);
				
				tr.eulerAngles = newRotation;

				momentum -= Time.deltaTime;
			}


			lastMousePosition = Input.mousePosition;
		}
	}
}