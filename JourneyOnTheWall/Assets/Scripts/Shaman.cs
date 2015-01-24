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

		private float lastHealed;

		public override void OnCollision (Collidable other)
		{
			base.OnCollision (other);

			var combatActor = other.GetComponent<CombatActor>();

			if (combatActor != null)
			{
				Debug.Log("Shaman hit by " + other.gameObject.name + " damage: " + combatActor.Damage + " " + Time.time + " > " + lastHealed + " + " + healingInterval);
				if (combatActor.Damage > 0 && Time.time > lastHealed + healingInterval)
				{
					lastHealed = Time.time;
					combatActor.Heal(healingPower);
					Debug.Log("Shaman healed", other.gameObject);
				}
			}
		}
	}
}