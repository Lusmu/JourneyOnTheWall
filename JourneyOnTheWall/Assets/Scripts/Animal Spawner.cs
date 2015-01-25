using UnityEngine;
using System.Collections;

namespace JourneyOnTheWall
{
	public class AnimalSpawner : MonoBehaviour 
	{
		[SerializeField]
		private GameObject animalPrefab1;
		[SerializeField]
		private float animalChange1 = 0.7f;

		[SerializeField]
		private GameObject animalPrefab2;
		[SerializeField]
		private float animalChange2 = 0.3f;

		void Start ()
		{
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}