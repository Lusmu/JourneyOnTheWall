using UnityEngine;
using System.Collections;

namespace JourneyOnTheWall
{
	public class LightController : MonoBehaviour 
	{
		private Vector3 origPosition;

		private Transform tr;

		private Light targetLight;

		private float defaultIntensity;

		// Use this for initialization
		void Start () {
			targetLight = GetComponent<Light>();
			defaultIntensity = targetLight.intensity;

			tr = GetComponent<Transform>();
			origPosition = tr.localPosition;
		}
		
		// Update is called once per frame
		void Update () {
			var newPos = origPosition;

			newPos.y += Mathf.Sin(Time.time * 0.3f) * 5f;

			tr.localPosition = newPos;

			targetLight.intensity = defaultIntensity + Mathf.Sin(Time.time * 3f) * 0.1f + Mathf.Sin(Time.time * 2.12f) * 0.2f + Mathf.Sin(Time.time * 1.6662f) * 0.15f;
		}
	}
}