﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JourneyOnTheWall
{
	public enum ResourceType
	{
		Undefined = 0,
		Food = 1,
		Leather = 2,
		Tool = 3
	}

	public class Clan : MonoBehaviour
	{
		public List<GameObject> People { get; private set; }

		public float Food { get; private set; }
		public float Leather { get; private set; }
		public float Tools { get; private set; }
	
		[SerializeField]
		private ParticleSystem foodParticles;
		[SerializeField]
		private ParticleSystem leatherParticles;
		[SerializeField]
		private ParticleSystem toolParticles;

		private static GameObject playerClanManagerPrefab;
		private static Clan playerClan;
		public static Clan PlayerClan 
		{ 
			get 
			{
				if (playerClanManagerPrefab == null)
				{
					playerClanManagerPrefab = Resources.Load("Player Clan Manager") as GameObject;
				}

				if (playerClan == null)
				{
					GameObject go = Instantiate(playerClanManagerPrefab) as GameObject;
					playerClan = go.GetComponent<Clan>();
				}
				return playerClan;
			}
		}

		public void AddMember(GameObject member)
		{
			if (People == null) People = new List<GameObject>();

			People.Add(member);
		}

		public void RemoveMember(GameObject member)
		{
			People.Remove(member);
		}

		public void AddResource(ResourceType resource, float amount, Quaternion rotation)
		{
			if (resource == ResourceType.Food) 
			{
				Food += amount;
				foodParticles.GetComponent<Transform>().parent.rotation = rotation;
				foodParticles.Emit(Mathf.RoundToInt(amount));
			}
			else if (resource == ResourceType.Leather)
			{
				Leather += amount;
				leatherParticles.GetComponent<Transform>().parent.rotation = rotation;
				leatherParticles.Emit(Mathf.RoundToInt(amount));
			}
			else if (resource == ResourceType.Tool) 
			{
				Tools += amount;
				toolParticles.GetComponent<Transform>().parent.rotation = rotation;
				toolParticles.Emit(Mathf.RoundToInt(amount));
			}

			Debug.Log("Player now has " + Food + " food, " + Leather + " leather and " + Tools + " tools");
		}
	}
}