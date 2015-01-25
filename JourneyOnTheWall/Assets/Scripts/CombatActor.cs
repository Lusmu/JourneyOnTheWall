using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace JourneyOnTheWall
{
	[RequireComponent(typeof(AudioSource))]
	public class CombatActor : MonoBehaviour
	{
		[SerializeField]
		private AudioClip[] attackSounds;

		[SerializeField]
		private AudioClip[] deathSounds;

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

		[SerializeField]
		private ParticleSystem healParticles;

		[SerializeField]
		private GameObject deathEffectPrefab;

		public float Damage { get; private set; }
		
		private float lastAttack;
		
		private bool isDead = false;

		private Transform tr;

		private Color col;

		[SerializeField]
		private SpriteRenderer mainRenderer;

		void Awake()
		{
			tr = GetComponent<Transform>();
			gameObject.AddComponent<AudioSource> ();

		}

		void Update()
		{
			Damage = Mathf.Clamp(Damage - Time.deltaTime * healRate, 0, maxHealth);

			if (mainRenderer != null)
			{
				col = mainRenderer.color;
				
				if (Damage > 0) col.a = 0.5f;
				else col.a = 0.9f;
				
				mainRenderer.color = col;
			}
		}

		public void OnCollision(Collidable other)
		{
			var otherFighter = other.GetComponent<CombatActor>();
			if (otherFighter == null) return;
			
			if (attackPower > 0 && hostileToFactions.Contains(otherFighter.Faction) && Time.time > lastAttack + attackDelay)
			{
				lastAttack = Time.time;
				otherFighter.TakeDamage(attackPower);
				anim.SetTrigger("Attack");
				if (attackSounds.Length > 0) 
					audio.PlayOneShot (attackSounds[Random.Range(0,attackSounds.Length)], 0.6F);
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


			if (deathSounds.Length > 0) 
				audio.PlayOneShot (deathSounds[Random.Range(0,deathSounds.Length)], 0.6F);

			if (deathEffectPrefab != null)
			{
				var go = Instantiate(deathEffectPrefab) as GameObject;
				go.GetComponent<Transform>().rotation = tr.rotation;
				Destroy(go, 5);
			}


			Destroy(gameObject);
		}

		public void Heal(float amount)
		{
			if (healParticles != null) healParticles.Play();

			Damage = Mathf.Clamp(Damage - amount, 0, maxHealth);
		}
	}
}