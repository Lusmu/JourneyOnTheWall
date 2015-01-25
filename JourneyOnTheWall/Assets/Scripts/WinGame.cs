using UnityEngine;
using System.Collections;

public class WinGame : MonoBehaviour {

	[SerializeField]
	private Transform sun;
	
	void Start ()
	{
		StartCoroutine(WinGame_Coroutine());
	}

	void Update()
	{
		sun.Rotate(Vector3.forward * Time.deltaTime * 40);
	}
	
	IEnumerator WinGame_Coroutine()
	{
		yield return null;

		while (sun.position.y > 25)
		{
			var pos = sun.position;

			pos.y -= Time.deltaTime * 12;

			sun.position = pos;

			yield return null;
		}
	}
}
