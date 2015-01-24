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

		void Awake()
		{
			tr = GetComponent<Transform>();
		}

		public void Move(Quaternion target)
		{
			this.target = target;
		}

		void Update()
		{
			tr.rotation = Quaternion.RotateTowards(tr.rotation, target, Time.deltaTime * speed);

			// TODO flip when changing direction
		}
	}
}