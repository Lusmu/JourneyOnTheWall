using UnityEngine;
using System.Collections;

namespace JourneyOnTheWall
{
	public class Plant : MonoBehaviour 
	{
		[SerializeField]
		private int rewardAmount = 3;
		[SerializeField]
		private ResourceType rewardType = ResourceType.Food;
		[SerializeField]
		private int gatherAmount = 1;
		[SerializeField]
		private float regenerateTime = 30;
		[SerializeField]
		private SpriteRenderer yieldRenderer;

		public ResourceType RewardType { get { return rewardType; }}

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
			var gather = Mathf.Min((rewardAmount - gathered), gatherAmount * gatherSpeed);

			if (gather > 0)
			{
				gathered += gather;

				Clan.PlayerClan.AddResource(rewardType, gather, tr.rotation);

				if (gathered >= rewardAmount)
				{
					timeEmptied = Time.time;
				}
			}
		}

		void Update()
		{
			if (gathered >= rewardAmount)
			{
				if (yieldRenderer != null)
				{
					color.a = Mathf.MoveTowards(color.a, 0, Time.deltaTime * 0.5f);
					yieldRenderer.color = color;
				}

				if (Time.time > timeEmptied + regenerateTime)
				{
					gathered = 0;
				}
			}
			else if (yieldRenderer != null)
			{
				color.a = Mathf.MoveTowards(color.a, 1, Time.deltaTime * 0.5f);
				yieldRenderer.color = color;
			}
		}
	}
}