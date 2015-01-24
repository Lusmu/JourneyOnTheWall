using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace JourneyOnTheWall
{
	public class SelectableController : MonoBehaviour 
	{
		[SerializeField]
		private Button button;
		[SerializeField]
		private SpriteRenderer spriteRenderer;
		[SerializeField]
		private Color selectedColor;

		private Color defaultColor;

		void Start()
		{
			defaultColor = spriteRenderer.color;
			button.onClick.AddListener(() => { GameManager.Instance.SelectCharacter(this); });
		}

		public void Select()
		{
			spriteRenderer.color = selectedColor;
		}

		public void Deselect()
		{
			spriteRenderer.color = defaultColor;
		}
	}
}