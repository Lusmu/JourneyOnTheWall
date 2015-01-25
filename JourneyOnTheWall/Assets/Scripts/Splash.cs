using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {
	[SerializeField]
	private SpriteRenderer mainImage;

	[SerializeField]
	private float time = 3;
	
	void Start ()
	{
		StartCoroutine(Splash_Coroutine());
	}
	
	IEnumerator Splash_Coroutine()
	{
		yield return new WaitForSeconds(time);

		var col = mainImage.color;

		while (col.a > 0.1f)
		{
			col.a -= Time.deltaTime;
			mainImage.color = col;
			yield return null;
		}

		yield return new WaitForSeconds(5);

		Application.LoadLevel(1);
	}
}
