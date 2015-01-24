using UnityEngine;
using System.Collections;

namespace JourneyOnTheWall
{
	public class Roaming : MonoBehaviour 
	{
		[SerializeField]
		public float changeTargetInterval = 30;

		[SerializeField]
		private float goingAwayChance = 0;

		private float lastTargetTime;

		[SerializeField]
		private float roamingRange = 90;

		private Quaternion target;

		private bool isGoingAway = false;

		private MoveController mover;

		void Start()
		{
			mover = GetComponent<MoveController>();
			target = GetRandomTarget();
			mover.Move(target);
		}

		void Update ()
		{
			if (!isGoingAway && Time.time > lastTargetTime + changeTargetInterval)
			{
				if (goingAwayChance > 0 && Random.value < goingAwayChance)
				{
					mover.collideOnBorders = false;

					var targetEuler = GetComponent<Transform>().eulerAngles;
					targetEuler.y += Random.Range(-roamingRange, roamingRange);
					targetEuler.x = 370;
					isGoingAway = true;
					target = Quaternion.Euler(targetEuler);
					mover.Move(target);
				}
				else
				{
					target = GetRandomTarget();
					mover.Move(target);
					lastTargetTime = Time.time;
				}
			}
			if (isGoingAway)
			{
				var height = GetComponent<Transform>().eulerAngles.x;
				if (height > 369 || height < -9 || (height > 9 && height < 200))
				{
					Destroy(gameObject);
				}
			}
		}

		Quaternion GetRandomTarget()
		{
			var current = GetComponent<Transform>().eulerAngles;

			var newTarget = current + Vector3.up * Random.Range(-roamingRange, roamingRange)
				+ Vector3.right * Random.Range(-roamingRange, roamingRange);
			newTarget.x = Mathf.Clamp(newTarget.x, 295, 350);

			return Quaternion.Euler(newTarget);
		}
	}
}