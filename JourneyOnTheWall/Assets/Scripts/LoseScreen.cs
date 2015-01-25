using UnityEngine;
using System.Collections;

public class LoseScreen : MonoBehaviour 
{
	public void Replay()
	{
		Application.LoadLevel(1);
	}
}
