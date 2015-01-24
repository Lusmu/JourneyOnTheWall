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

		private Quaternion targetRotation;

		private Transform tr;

		private bool isMoving = false;

		[SerializeField]
		private float foodGatherTime = 1;

		[SerializeField]
		private float toolsCreateTime = 10;

		private float lastGathered = 0;

		[SerializeField]
		private Animator anim;

		private Quaternion lastPosition;

		private Transform target;

		private float timeIdle = 0;

		void Awake()
		{
			tr = GetComponent<Transform>();
			if (anim != null) anim.SetBool("Moving", false);
		}

		public void Move(Quaternion target)
		{
			this.targetRotation = target;

			isMoving = true;
		}

		public void Move(Transform target)
		{
			this.target = target;
			this.targetRotation = target.rotation;
			
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
			var plant = other.GetComponent<Plant>();
			if (plant != null && plant.RewardType == ResourceType.Food && foodGatherTime > 0 && Time.time > lastGathered + foodGatherTime)
			{
				if (plant != null)
				{
					lastGathered = Time.time;

					plant.Gather(1);
				}
			}
			else if (plant != null && plant.RewardType == ResourceType.Tool && toolsCreateTime > 0 && Time.time > lastGathered + toolsCreateTime)
			{
				if (plant != null)
				{
					lastGathered = Time.time;
					
					plant.Gather(1);
				}
			}
		}

		void Update()
		{
			if (target != null)
			{
				isMoving = true;
				targetRotation = target.rotation;
			}

			if (isMoving)
			{
				var moveTo = Quaternion.RotateTowards(tr.rotation, targetRotation, Time.deltaTime * speed).eulerAngles;
				moveTo.x = Mathf.Clamp(moveTo.x, 300, 355);
				tr.rotation = Quaternion.Euler(moveTo);
				if (Quaternion.Angle(tr.rotation, targetRotation) < 0.1f) isMoving = false;
			
				var shouldFaceRight = true;

				if (Mathf.Abs(targetRotation.eulerAngles.y - tr.eulerAngles.y) > 180)
				{
					if (targetRotation.eulerAngles.y < tr.eulerAngles.y) shouldFaceRight = false;
				}
				else
				{
					if (targetRotation.eulerAngles.y > tr.eulerAngles.y) shouldFaceRight = false;
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