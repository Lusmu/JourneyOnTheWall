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


			}

			lastMousePosition = Input.mousePosition;
		}
	}
}