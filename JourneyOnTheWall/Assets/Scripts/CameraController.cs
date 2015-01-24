using UnityEngine;
using System.Collections;

namespace JourneyOnTheWall
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField]
		private float speed = 10;

		private bool isDragging = false;

		private Transform tr;

		private Vector3 lastMousePosition;

		void Awake()
		{
			tr = GetComponent<Transform>();
		}

		public void BeginTouch()
		{
			isDragging = true;
		}

		public void EndTouch()
		{
			isDragging = false;
		}

		void Update()
		{
			if (isDragging)
			{
				var diff = Input.mousePosition - lastMousePosition;

				var newRotation = tr.eulerAngles + diff.x * Vector3.up * speed + diff.y * Vector3.right * speed;
				newRotation.y = Mathf.Clamp(newRotation.x, 300, 330);

				tr.eulerAngles = newRotation;
			}

			lastMousePosition = Input.mousePosition;
		}
	}
}