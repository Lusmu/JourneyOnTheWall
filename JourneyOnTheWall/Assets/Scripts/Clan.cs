using UnityEngine;
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
	
		private static Clan playerClan;
		public static Clan PlayerClan 
		{ 
			get 
			{
				if (playerClan == null)
				{
					GameObject go = new GameObject("Clan Manager");
					playerClan = go.AddComponent<Clan>();
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

		public void AddResource(ResourceType resource, float amount)
		{
			if (resource == ResourceType.Food) Food += amount;
			else if (resource == ResourceType.Leather) Leather += amount;
			else if (resource == ResourceType.Tool) Tools += amount;

			Debug.Log("Player now has " + Food + " food, " + Leather + " leather and " + Tools + " tools");
		}
	}
}