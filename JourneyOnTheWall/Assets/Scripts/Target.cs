using UnityEngine;
using System.Collections;

namespace JourneyOnTheWall
{
	public class Target : MonoBehaviour 
	{
		public void Select()
		{
			GameManager.Instance.SetTarget(this);
		}
	}
}