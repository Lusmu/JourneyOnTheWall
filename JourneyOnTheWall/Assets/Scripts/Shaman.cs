using UnityEngine;
using System.Collections;

namespace JourneyOnTheWall
{
	public class Shaman : ClanMember 
	{
		[SerializeField]
		private float healingPower = 10;
		[SerializeField]
		private float healingInterval = 30;
		[SerializeField]
		private Animator anim;

		private float lastHealed;

		public override void OnCollision (Collidable other)
		{
			base.OnCollision (other);

			var combatActor = other.GetComponent<CombatActor>();

			if (combatActor != null)
			{
				if (combatActor.Damage > 0 && Time.time > lastHealed + healingInterval)
				{
					lastHealed = Time.time;
					combatActor.Heal(healingPower);
					Debug.Log("Shaman healed", other.gameObject);
					anim.SetTrigger("Magic");
				}
			}
		}
	}
}