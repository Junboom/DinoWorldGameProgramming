using UnityEngine;
using UnityEngine.UI;

public class CharacterAttackButton : MonoBehaviour
{
    public Button yourButton;

    private CharacterState CS;

    public GameObject CharacterMenu;
    public GameObject CharacterSkillMenu;

    private void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(OnMouseDown);

        CS = GameObject.Find("Character").GetComponent<CharacterState>();
    }

    private void OnMouseDown()
    {
        CS.attack_or_skill = 1;
        CharacterMenu.SetActive(false);
        CharacterSkillMenu.SetActive(false);
    }
}
