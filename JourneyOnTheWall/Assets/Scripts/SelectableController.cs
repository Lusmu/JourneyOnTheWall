using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace JourneyOnTheWall
{
	[RequireComponent(typeof(AudioSource))]
	public class SelectableController : MonoBehaviour 
	{
		[SerializeField]
		private AudioClip[] selectAudioClips;


		[SerializeField]
		private Button button;
		[SerializeField]
		private SpriteRenderer spriteRenderer;
		[SerializeField]
		private Color selectedColor;

		private Color defaultColor;

		void Start()
		{
			gameObject.AddComponent<AudioSource> ();
			defaultColor = spriteRenderer.color;
			button.onClick.AddListener(() => { GameManager.Instance.SelectCharacter(this); });
		}

		public void Select()
		{
			spriteRenderer.color = selectedColor;
			//selectAudioClip.Play ();
			//clipSource.Play ();
			if (selectAudioClips.Length > 0) 
				audio.PlayOneShot (selectAudioClips[Random.Range(0,selectAudioClips.Length)], 0.5F);


		}

		public void Deselect()
		{
			spriteRenderer.color = defaultColor;
		}
	

	}
}