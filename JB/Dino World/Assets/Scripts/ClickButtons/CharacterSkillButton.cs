using UnityEngine;

public class CharacterSkillButton : MonoBehaviour
{
    private CharacterState CS;

    public GameObject CharacterMenu;

    private void Start()
    {
        CS = GameObject.Find("Character").GetComponent<CharacterState>();
    }

    private void OnMouseDown()
    {
        CS.attack_or_skill = 2;
        CharacterMenu.SetActive(false);
    }
}
