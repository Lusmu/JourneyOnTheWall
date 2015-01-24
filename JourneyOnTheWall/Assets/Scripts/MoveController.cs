using UnityEngine;
using System.Collections;

namespace JourneyOnTheWall
{
	public class MoveController : MonoBehaviour 
	{
		[SerializeField]
		private float speed = 10;

		private Quaternion target;

		private Transform tr;

		private bool isMoving = false;

		void Awake()
		{
			tr = GetComponent<Transform>();
		}

		public void Move(Quaternion target)
		{
			this.target = target;
			isMoving = true;
		}

		void Update()
		{
			if (isMoving)
			{
				var moveTo = Quaternion.RotateTowards(tr.rotation, target, Time.deltaTime * speed).eulerAngles;
				moveTo.x = Mathf.Clamp(moveTo.x, 300, 355);
				tr.rotation = Quaternion.Euler(moveTo);

				if (Quaternion.Angle(tr.rotation, target) < 0.1f) isMoving = false;
			}

			// TODO flip when changing direction
		}
	}
}