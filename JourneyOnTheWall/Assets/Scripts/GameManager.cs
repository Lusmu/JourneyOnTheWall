using UnityEngine;
using System.Collections;

namespace JourneyOnTheWall
{
	public class GameManager : MonoBehaviour 
	{
		public static GameManager Instance { get; private set; }

		public SelectableController SelectedCharacter { get; private set; }

		private Transform helperTransform;

		void Awake()
		{
			Instance = this;

			var go = new GameObject("HelperTransform");
			helperTransform = go.GetComponent<Transform>();
			helperTransform.position = Vector3.zero;
		}

		public void SelectCharacter(SelectableController characterController)
		{
			Debug.Log("Selected", characterController.gameObject);
			if (SelectedCharacter != null) SelectedCharacter.Deselect();

			SelectedCharacter = characterController;

			SelectedCharacter.Select();
		}

		public void BackgroundTouched()
		{
			if (SelectedCharacter != null)
			{
				var mover = SelectedCharacter.GetComponent<MoveController>();

				if (mover != null)
				{
					var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100);

					helperTransform.LookAt(Camera.main.ScreenToWorldPoint(mousePos));

					mover.Move(helperTransform.rotation);
				}
			}
		}
	}
}