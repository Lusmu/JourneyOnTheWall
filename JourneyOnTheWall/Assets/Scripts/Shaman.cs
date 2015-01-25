using UnityEngine;
using System.Collections;

namespace JourneyOnTheWall
{
	[RequireComponent(typeof(AudioSource))]
	public class Shaman : ClanMember 
	{
		[SerializeField]
		private AudioClip healingSound;
		[SerializeField]
		private float healingPower = 10;
		[SerializeField]
		private float healingInterval = 30;
		[SerializeField]
		private Animator anim;

		private float lastHealed;


		void Awake()
		{
			gameObject.AddComponent<AudioSource> ();			
		}

		public override void OnCollision (Collidable other)
		{
			base.OnCollision (other);

			var combatActor = other.GetComponent<CombatActor>();

			if (combatActor != null)
			{
				if (combatActor.Damage > 0 && Time.time > lastHealed + healingInterval)
				{
					lastHealed = Time.time;

					if (healingSound) 
						audio.PlayOneShot (healingSound, 0.6F);
					combatActor.Heal(healingPower);
					Debug.Log("Shaman healed", other.gameObject);
					anim.SetTrigger("Magic");
				}
			}
		}
	}
}