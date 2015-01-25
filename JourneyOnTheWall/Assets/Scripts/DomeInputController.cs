using UnityEngine;
using System.Collections;

namespace JourneyOnTheWall
{
	public class DomeInputController : MonoBehaviour
	{
		int selectedIndex = -1;
		void Update ()
		{
			if (Input.GetKeyUp(KeyCode.Tab))
			{
				selectedIndex ++;

				if (selectedIndex >= Clan.PlayerClan.People.Count) selectedIndex = 0; 

				var oldSelected = GameManager.Instance.SelectedCharacter;

				if (selectedIndex >= 0 && selectedIndex < Clan.PlayerClan.People.Count)
				{
					GameManager.Instance.SelectCharacter(Clan.PlayerClan.People[selectedIndex].GetComponent<SelectableController>());
					var newSelected = GameManager.Instance.SelectedCharacter;
					if (oldSelected != null) oldSelected.GetComponent<MoveController>().SetTargetIndicator(false);
					if (newSelected != null) newSelected.GetComponent<MoveController>().SetTargetIndicator(true);
				}
			}

			if (GameManager.Instance.SelectedCharacter != null)
			{
				if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.001f || Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.001f)
				{
					float x = - Input.GetAxisRaw("Vertical") * Time.deltaTime * 30f;
					float y = Input.GetAxisRaw("Horizontal") * Time.deltaTime * 30f;
					
					Vector3 targetPos = GameManager.Instance.SelectedCharacter.GetComponent<MoveController>().TargetRotation.eulerAngles;
					targetPos.x += x;
					targetPos.y += y;
					GameManager.Instance.SelectedCharacter.GetComponent<MoveController>().Move(Quaternion.Euler(targetPos));
				}
			}
		}
	}
}