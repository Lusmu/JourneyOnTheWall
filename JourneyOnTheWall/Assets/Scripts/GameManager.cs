using UnityEngine;
using System.Collections;

namespace JourneyOnTheWall
{
	public class GameManager : MonoBehaviour 
	{
		public static GameManager Instance { get; private set; }

		public SelectableController SelectedCharacter { get; private set; }

		private Transform helperTransform;

		private Vector3 touchBeginPosition;

		void Awake()
		{

			// Fixed screen orientation for mobile platforms
			Screen.orientation = ScreenOrientation.LandscapeLeft;

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

		void Update()
		{
			if (Input.GetKeyUp(KeyCode.Escape)) Application.Quit();
		}

		public void BackgroundTouched()
		{
			if (Vector3.Distance(Input.mousePosition, touchBeginPosition) > 20) return;

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

		public void BeginTouchingBackground()
		{
			touchBeginPosition = Input.mousePosition;
		}

		public void SetTarget(Target target)
		{
			// Unused for now
		}
	}
}