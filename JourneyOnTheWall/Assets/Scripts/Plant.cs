using UnityEngine;
using System.Collections;

namespace JourneyOnTheWall
{
	public class Plant : MonoBehaviour 
	{
		[SerializeField]
		private int foodReward = 3;
		[SerializeField]
		private int gatherAmount = 1;
		[SerializeField]
		private float regenerateTime = 30;
		[SerializeField]
		private SpriteRenderer yieldRenderer;

		private Transform tr;

		private int gathered = 0;
		private float timeEmptied;

		private Color color;

		void Awake()
		{
			tr = GetComponent<Transform>();
		}

		public void Gather(int gatherSpeed)
		{
			var gather = Mathf.Min((foodReward - gathered), gatherAmount * gatherSpeed);

			if (gather > 0)
			{
				gathered += gather;

				Clan.PlayerClan.AddResource(ResourceType.Food, gather, tr.rotation);

				if (gathered >= foodReward)
				{
					timeEmptied = Time.time;
				}
			}
		}

		void Update()
		{
			if (gathered >= foodReward)
			{
				color.a = Mathf.MoveTowards(color.a, 0, Time.deltaTime * 0.5f);
				if (Time.time > timeEmptied + regenerateTime)
				{
					gathered = 0;
				}
			}
			else
			{
				color.a = Mathf.MoveTowards(color.a, 1, Time.deltaTime * 0.5f);
			}
		}
	}
}