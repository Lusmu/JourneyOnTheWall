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

		[SerializeField]
		private float attackDelay = 1;

		[SerializeField]
		private float attackPower = 1;

		[SerializeField]
		private float maxHealth = 1;

		[SerializeField]
		private CreatureFaction faction;
		public CreatureFaction Faction { get { return faction; }}

		[SerializeField]
		private List<CreatureFaction> hostileToFactions;

		private bool facingRight = true;

		private Quaternion target;

		private Transform tr;

		private bool isMoving = false;

		[SerializeField]
		private Animator anim;

		private Quaternion lastPosition;

		private float timeIdle = 0;

		public float Damage { get; private set; }

		private float lastAttack;

		private bool isDead = false;

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

		public void TakeDamage(float amount)
		{
			Damage += amount;

			if (Damage > maxHealth) Die ();
		}

		public void OnCollision(Collidable other)
		{
			var otherMover = other.GetComponent<MoveController>();
			if (otherMover == null) return;

			if (attackPower > 0 && hostileToFactions.Contains(otherMover.Faction) && Time.time > lastAttack + attackDelay)
			{
				lastAttack = Time.time;
				otherMover.TakeDamage(attackPower);
				// TODO animate attack
				Debug.Log(gameObject.name + " hit " + other.gameObject.name);
			}
		}

		public void Die()
		{
			if (isDead) return;

			isDead = true;

			Destroy(gameObject);
		}
	}
}