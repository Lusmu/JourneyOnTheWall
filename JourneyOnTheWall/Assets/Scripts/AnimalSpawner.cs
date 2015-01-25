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
		[SerializeField]
		private Transform spawnRoot;
		[SerializeField]
		private float minSpawnInterval = 50;
		[SerializeField]
		private float maxSpawnInterval = 200;

		float nextSpawn;

		void Start()
		{
			nextSpawn = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
		}

		void Update ()
		{
			if (Time.time > nextSpawn)
			{
				StartCoroutine(Spawn_Coroutine());
				nextSpawn = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
			}
		}

		IEnumerator Spawn_Coroutine()
		{
			GameObject prefab = null;

			if (Random.value > animalChange1) prefab = animalPrefab1;
			else prefab = animalPrefab2;

			GameObject go = Instantiate(prefab) as GameObject;
			var tr = go.GetComponent<Transform>();
			tr.SetParent(spawnRoot);
			tr.position = Vector3.zero;
			var targetY = Random.Range(0, 360);
			tr.eulerAngles = new Vector3(290, targetY, 0);
			var mover = go.GetComponent<MoveController>();
			mover.collideOnBorders = false;
			var col = go.GetComponent<MoveController>();
			col.collideOnBorders = false;

			var roam = go.GetComponent<Roaming>();
			if (roam != null) roam.enabled = false;

			mover.Move(Quaternion.Euler(new Vector3(299, targetY, 0)));

			while (tr != null && Mathf.Abs(tr.eulerAngles.x - 299) > 0.5f) yield return null;

			if (go != null)
			{
				mover.collideOnBorders = true;
				col.collideOnBorders = true;
				if (roam != null) roam.enabled = true;
			}
		}
	}
}