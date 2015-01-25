using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace JourneyOnTheWall 
{
	public class ResourcesDisplay : MonoBehaviour
	{
		[SerializeField]
		private UnityEngine.UI.Text foodLabel;
		[SerializeField]
		private UnityEngine.UI.Text leatherLabel;
		[SerializeField]
		private UnityEngine.UI.Text toolsLabel;

		void Update () 
		{
			foodLabel.text = Clan.PlayerClan.Food + "/" + GameManager.Instance.foodLimit;
			leatherLabel.text = Clan.PlayerClan.Leather + "/" + GameManager.Instance.leatherLimit;
			toolsLabel.text = Clan.PlayerClan.Tools + "/" + GameManager.Instance.toolsLimit;
		}
	}
}