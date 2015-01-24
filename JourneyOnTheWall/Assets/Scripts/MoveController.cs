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

		[SerializeField]
		private Animator anim;

		void Awake()
		{
			tr = GetComponent<Transform>();
		}

		public void Move(Quaternion target)
		{
			this.target = target;

			anim.SetBool ("Moving", true);
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

			if(tr.rotation.x > target.x && !facingRight)
				Flip();
			else if(tr.rotation.x < target.x && facingRight)
				Flip();

			tr.rotation = Quaternion.RotateTowards(tr.rotation, target, Time.deltaTime * speed);

			if (Quaternion.Angle (tr.rotation, target) < 0.1)
								anim.SetBool ("Moving", false);


		}

	}
}