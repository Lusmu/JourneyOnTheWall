﻿using UnityEngine;
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
		public bool collideOnBorders = true;

		[SerializeField]
		private bool defaultIsFacingRight = false;

		[SerializeField]
		private float speed = 10;

		private bool facingRight = true;

		public Quaternion TargetRotation { get; private set; }

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

		private Collidable col;

		[SerializeField]
		private GameObject targetIndicatorPrefab;

		private Transform targetIndicator;
		private bool hasTarget;
		void Awake()
		{
			tr = GetComponent<Transform>();
			if (anim != null) anim.SetBool("Moving", false);
			col = GetComponent<Collidable>();

			if (targetIndicatorPrefab != null)
			{
				var go = Instantiate(targetIndicatorPrefab) as GameObject;
				targetIndicator = go.GetComponent<Transform>();
				targetIndicator.SetParent(tr.parent);
				go.SetActive(false);
				targetIndicator.position = Vector3.zero;
			}

			TargetRotation = tr.rotation;
		}

		public void Move(Quaternion target)
		{
			this.TargetRotation = target;
			this.target = null;
			hasTarget = true;
			isMoving = true;
		}

		public void Move(Transform target)
		{
			this.target = target;
			this.TargetRotation = target.rotation;
			hasTarget = true;
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
				if (plant != null && plant.HasYield)
				{
					lastGathered = Time.time;

					anim.SetTrigger("Gather");

					plant.Gather(1);
				}
			}
			else if (plant != null && plant.RewardType == ResourceType.Tool && toolsCreateTime > 0 && Time.time > lastGathered + toolsCreateTime)
			{
				if (plant != null && plant.HasYield)
				{
					lastGathered = Time.time;

					anim.SetTrigger("Gather");
					
					plant.Gather(1);
				}
			}
		}

		void Update()
		{
			if (GameManager.Instance.IsGameOver) return;

			if (target != null)
			{
				isMoving = true;
				TargetRotation = target.rotation;
			}

			if (isMoving)
			{
				if (TargetRotation == Quaternion.identity) TargetRotation = tr.rotation;

				var moveTo = Quaternion.RotateTowards(tr.rotation, TargetRotation, Time.deltaTime * speed).eulerAngles;
				if (collideOnBorders) moveTo.x = Mathf.Clamp(moveTo.x, 300, 355);
				tr.rotation = Quaternion.Euler(moveTo);

				var dist = 0.1f;
				if (col != null) dist = col.size * 0.5f;
				if (Quaternion.Angle(tr.rotation, TargetRotation) < dist) isMoving = false;
			
				var shouldFaceRight = true;

				if (Mathf.Abs(TargetRotation.eulerAngles.y - tr.eulerAngles.y) > 180)
				{
					if (TargetRotation.eulerAngles.y < tr.eulerAngles.y) shouldFaceRight = false;
				}
				else
				{
					if (TargetRotation.eulerAngles.y > tr.eulerAngles.y) shouldFaceRight = false;
				}

				if (defaultIsFacingRight) shouldFaceRight = !shouldFaceRight;

				if (shouldFaceRight != facingRight) Flip();

				anim.SetBool("Moving", true);

				if (targetIndicator != null && targetIndicator.gameObject.activeInHierarchy) targetIndicator.rotation = TargetRotation;

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

		public void SetTargetIndicator (bool b)
		{
			if (targetIndicator != null) targetIndicator.gameObject.SetActive(b);
		}
	}
}