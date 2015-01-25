using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {
	[SerializeField]
	private float time = 5;
	
	private float createTime;

	void Start()
	{
		createTime = Time.time;
	}

	void Update ()
	{
		if (Time.time > createTime + time) Destroy(gameObject);
	}
}
