using UnityEngine;
using System.Collections;




namespace JourneyOnTheWall 
{
	public class ResroucesDisplay : MonoBehaviour
	{
		[SerializeField]
		private Sprite[] countBar;

	
		[SerializeField]
		private int foodNeeded;
		[SerializeField]
		private int toolsNeeded;
		[SerializeField]
		private int leatherNeeded;


		void Awake () 
		{
			/*
			GameObject foodParent = new GameObject ("foodParent");
			GameObject leatherParent = new GameObject ("leatherParent");
			GameObject toolsParent = new GameObject ("toolsParent");


			GameObject food = new GameObject ();
			GameObject leather = new GameObject ();
			GameObject tools = new GameObject ();

			food.AddComponent<SpriteRenderer> ();
			food.GetComponent<Transform> ().SetParent (foodParent);

			leather.AddComponent<SpriteRenderer> ();
			leather.GetComponent<Transform> ().SetParent (leatherParent);

			tools.AddComponent<SpriteRenderer> ();
			tools.GetComponent<Transform> ().SetParent (toolsParent);

			food.GetComponent<SpriteRenderer> ().sprite = countBar [2];
			*/

		}


		// Update is called once per frame
		void Update () 
		{
			// Asetetaan ikoneille Clan.PlayerClan.food / 5 -> ikonSprite[n]
			//Debug.Log (Time.time);

		//	if(Time.time > 5)

			//if(Clan.PlayerClan.food
		}


	


	}
}