using UnityEngine;
using System.Collections;

public class LoseScreen : MonoBehaviour 
{
	public void Replay()
	{
		Application.LoadLevel(1);
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.Space)) Replay();
	}
}
