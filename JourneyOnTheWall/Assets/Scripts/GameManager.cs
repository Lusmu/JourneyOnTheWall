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

		public int foodLimit = 50;
		public int leatherLimit = 30;
		public int toolsLimit = 20;

		[SerializeField]
		private GameObject loseEffect;
		[SerializeField]
		private GameObject winEffect;

		public bool IsGameOver { get; private set; }

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
			if (characterController != null) Debug.Log("Selected", characterController.gameObject);
			if (SelectedCharacter != null) SelectedCharacter.Deselect();

			SelectedCharacter = characterController;

			if (SelectedCharacter != null) SelectedCharacter.Select();
		}

		void Update()
		{
			if (Input.GetKeyUp(KeyCode.Escape)) Application.Quit();

			if (!IsGameOver && Time.timeSinceLevelLoad > 10)
			{
				if (Clan.PlayerClan.PeopleCount <= 1) LoseGame();
				else if (Clan.PlayerClan.Food >= foodLimit &&
				         Clan.PlayerClan.Tools >= toolsLimit &&
				         Clan.PlayerClan.Leather >= leatherLimit) WinGame();
			}
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
			if (SelectedCharacter != null) 
			{
				var mover = SelectedCharacter.GetComponent<MoveController>();
				if (mover != null) mover.Move(target.GetComponent<Transform>());
			}
		}

		public void LoseGame()
		{
			StartCoroutine(LoseGame_Coroutine());
		}

		public void WinGame()
		{
			StartCoroutine(WinGame_Coroutine());
		}

		IEnumerator LoseGame_Coroutine()
		{
			IsGameOver = true;

			yield return null;

			loseEffect.SetActive(true);

			yield return new WaitForSeconds(10);

			Application.LoadLevel("Lose");
		}

		IEnumerator WinGame_Coroutine()
		{
			IsGameOver = true;
			
			yield return null;
			
			winEffect.SetActive(true);
			
			yield return new WaitForSeconds(12);
			
			Application.LoadLevel("Win");
		}
	}
}