using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace JourneyOnTheWall
{
	public class SelectableController : MonoBehaviour 
	{
		[SerializeField]
		private Button button;

		void Start()
		{
			button.onClick.AddListener(() => { GameManager.Instance.SelectCharacter(this); });
		}

		public void Select()
		{

		}

		public void Deselect()
		{

		}
	}
}