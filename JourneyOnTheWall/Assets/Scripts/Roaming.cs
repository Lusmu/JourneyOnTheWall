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
					GetComponent<Collidable>().collidesWithBorders = false;

					var targetEuler = GetComponent<Transform>().eulerAngles;
					targetEuler.y += Random.Range(-roamingRange, roamingRange);
					targetEuler.x = 370;
					isGoingAway = true;
					target = Quaternion.Euler(targetEuler);
					mover.Move(target);
					lastTargetTime = Time.time;
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
				if (height > 361 || height < -1 || (height > 5 && height < 270))
				{
					Destroy(gameObject);
				}
				else if (Time.time > lastTargetTime + 12)
				{
					isGoingAway = false;
					target = GetRandomTarget();
					mover.Move(target);
					lastTargetTime = Time.time;
				}
			}
		}

		Quaternion GetRandomTarget()
		{
			Quaternion retVal = GetComponent<Transform>().rotation;

			float attemps = 100;

			while (attemps > 0)
			{
				var current = GetComponent<Transform>().eulerAngles;
				
				var newTarget = current + Vector3.up * Random.Range(-roamingRange, roamingRange)
					+ Vector3.right * Random.Range(-roamingRange, roamingRange);
				newTarget.x = Mathf.Clamp(newTarget.x, 300, 345);

				retVal = Quaternion.Euler(newTarget);

				if (CollisionManager.Instance.GetCollision(retVal, 2) == null)
				{
					Debug.Log("Found random spot at " + retVal.eulerAngles);
					break;
				}

				attemps --;

				if (attemps < 0) break;
			}

			return retVal;
		}
	}
}