using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {
	[SerializeField]
	private float time = 5;
	
	private float timer;

	void Start()
	{
		Debug.Log("created");
		timer = time;
	}

	void Update ()
	{
		timer -= Time.deltaTime;
		if (timer < 0)
		{
			Destroy(gameObject);
		}
	}
}
