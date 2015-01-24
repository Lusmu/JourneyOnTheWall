using UnityEngine;
using System.Collections;

namespace JourneyOnTheWall
{
	public class ClanMember : MonoBehaviour 
	{
		void OnEnable()
		{
			Clan.PlayerClan.AddMember(gameObject);
		}

		public virtual void OnCollision(Collidable other)
		{

		}
	}
}