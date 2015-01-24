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

				if (selectedIndex >= 0 && selectedIndex < Clan.PlayerClan.People.Count)
					GameManager.Instance.SelectCharacter(Clan.PlayerClan.People[selectedIndex].GetComponent<SelectableController>());
			}
			if (Input.GetKeyUp(KeyCode.Alpha1))
			{

			}
			if (Input.GetKeyUp(KeyCode.Alpha2))
			{
				
			}
			if (Input.GetKeyUp(KeyCode.Alpha3))
			{
				
			}
			if (GameManager.Instance.SelectedCharacter != null)
			{
				if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.001f || Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.001f)
				{
					float x = - Input.GetAxisRaw("Vertical") * Time.deltaTime * 12;
					float y = Input.GetAxisRaw("Horizontal") * Time.deltaTime * 12f;
					
					Vector3 targetPos = GameManager.Instance.SelectedCharacter.GetComponent<Transform>().eulerAngles;
					targetPos.x += x;
					targetPos.y += y;
					GameManager.Instance.SelectedCharacter.GetComponent<MoveController>().Move(Quaternion.Euler(targetPos));
				}
			}
		}
	}
}