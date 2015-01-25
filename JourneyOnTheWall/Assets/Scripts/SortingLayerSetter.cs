using UnityEngine;
using System.Collections;

public class SortingLayerSetter : MonoBehaviour {

	void Start ()
	{
		GetComponent<ParticleSystem>().renderer.sortingLayerName = "Foreground";
	}
	void Update ()
	{
		GetComponent<ParticleSystem>().renderer.sortingLayerName = "Foreground";
	}
}
