using UnityEngine;
using System.Collections;

namespace JourneyOnTheWall
{
	public class MoveController : MonoBehaviour 
	{
		[SerializeField]
		private float speed = 10;
		
		private bool facingRight = true;

		private Quaternion target;

		private Transform tr;

		private bool isMoving = false;

		[SerializeField]
		private Animator anim;

		void Awake()
		{
			tr = GetComponent<Transform>();
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
			if (isMoving)
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
			}

			anim.SetBool("Moving", isMoving);
		}
	}
}