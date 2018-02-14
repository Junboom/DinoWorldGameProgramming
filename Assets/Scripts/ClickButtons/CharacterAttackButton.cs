using UnityEngine;

public class CharacterAttackButton : MonoBehaviour
{
    private CharacterState CS;

    public GameObject CharacterMenu;

    private void Start()
    {
        CS = GameObject.Find("Character").GetComponent<CharacterState>();
    }

    private void OnMouseDown()
    {
        CS.attack_or_skill = 1;
        CharacterMenu.SetActive(false);
    }
}
