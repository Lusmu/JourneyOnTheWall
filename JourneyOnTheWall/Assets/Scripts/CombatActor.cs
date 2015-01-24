using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JourneyOnTheWall
{
	public class CombatActor : MonoBehaviour
	{
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
		
		[SerializeField]
		private float healRate = 0;

		[SerializeField]
		private Animator anim;

		[SerializeField]
		private int foodReward = 0;

		[SerializeField]
		private int leatherReward = 0;

		[SerializeField]
		private ParticleSystem damagedParticles;

		public float Damage { get; private set; }
		
		private float lastAttack;
		
		private bool isDead = false;

		private Transform tr;

		void Awake()
		{
			tr = GetComponent<Transform>();
		}

		void Update()
		{
			Damage = Mathf.Clamp(Damage - Time.deltaTime * healRate, 0, maxHealth);
		}

		public void OnCollision(Collidable other)
		{
			var otherFighter = other.GetComponent<CombatActor>();
			if (otherFighter == null) return;
			
			if (attackPower > 0 && hostileToFactions.Contains(otherFighter.Faction) && Time.time > lastAttack + attackDelay)
			{
				lastAttack = Time.time;
				otherFighter.TakeDamage(attackPower);
				// TODO animate attack
			}
		}

		public void TakeDamage(float amount)
		{
			Damage += amount;

			if (damagedParticles != null) damagedParticles.Emit(Mathf.RoundToInt(amount * 10));

			if (Damage > maxHealth) Die ();
		}
		
		public void Die()
		{
			if (isDead) return;
			
			isDead = true;

			if (leatherReward > 0) Clan.PlayerClan.AddResource(ResourceType.Leather, leatherReward, tr.rotation);
			if (foodReward > 0) Clan.PlayerClan.AddResource(ResourceType.Food, foodReward, tr.rotation);

			Destroy(gameObject);
		}

		public void Heal(float amount)
		{
			Damage = Mathf.Clamp(Damage - amount, 0, maxHealth);
		}
	}
}