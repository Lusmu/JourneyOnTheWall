using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JourneyOnTheWall
{
	public enum CreatureFaction
	{
		Unknown = 0,
		PlayerClan = 1,
		Predator = 2,
		PassiveAnimal = 3
	}

	public class MoveController : MonoBehaviour 
	{
		[SerializeField]
		private float speed = 10;

		private bool facingRight = true;

		private Quaternion target;

		private Transform tr;

		private bool isMoving = false;

		[SerializeField]
		private float gatherTime = 1;

		private float lastGathered = 0;

		[SerializeField]
		private Animator anim;

		private Quaternion lastPosition;

		private float timeIdle = 0;

		void Awake()
		{
			tr = GetComponent<Transform>();
			anim.SetBool("Moving", false);
		}

		public void Move(Quaternion target)
		{
			this.target = target;

			isMoving = true;
		}

		void Flip() 
		{
			facingRight = !facingRight;
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
		}

		public void OnCollision(Collidable other)
		{
			if (gatherTime > 0 && Time.time > lastGathered + gatherTime)
			{
				var plant = other.GetComponent<Plant>();

				if (plant != null)
				{
					lastGathered = Time.time;

					plant.Gather(1);
				}
			}
		}

		void Update()
		{
			if (isMoving )
			{
				var moveTo = Quaternion.RotateTowards(tr.rotation, target, Time.deltaTime * speed).eulerAngles;
				moveTo.x = Mathf.Clamp(moveTo.x, 300, 355);
				tr.rotation = Quaternion.Euler(moveTo);
				if (Quaternion.Angle(tr.rotation, target) < 0.1f) isMoving = false;
			
				var shouldFaceRight = true;

				if (Mathf.Abs(target.eulerAngles.y - tr.eulerAngles.y) > 180)
				{
					if (target.eulerAngles.y < tr.eulerAngles.y) shouldFaceRight = false;
				}
				else
				{
					if (target.eulerAngles.y > tr.eulerAngles.y) shouldFaceRight = false;
				}

				if (shouldFaceRight != facingRight) Flip();

				anim.SetBool("Moving", true);

				timeIdle = 0;
			}
			else if (Quaternion.Angle(lastPosition, tr.rotation) > 0.05f)
			{				
				anim.SetBool("Moving", true);

				timeIdle = 0;
			}
			else
			{
				timeIdle += Time.deltaTime;
				if (timeIdle > 0.5f) anim.SetBool("Moving", false);
			}

			lastPosition = tr.rotation;
		}
	}
}