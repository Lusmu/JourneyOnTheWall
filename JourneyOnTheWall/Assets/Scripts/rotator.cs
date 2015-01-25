using UnityEngine;
using System.Collections;


namespace JourneuOnTheWall 
{
	public class rotator : MonoBehaviour 
	{
		[SerializeField]
		private float speed = 0.7f;

		private Transform tr;

		void Awake()
		{
			tr = GetComponent<Transform>();
		}

		
		// Update is called once per frame
		void Update () {
			tr.RotateAround(tr.position, tr.forward, Time.deltaTime * speed * -1);
		}
	}
}
