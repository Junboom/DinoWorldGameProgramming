using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUsingSkill : MonoBehaviour
{
    public Button yourButton;

    private CharacterState CS;

    public GameObject CharacterMenu;
    public GameObject CharacterSkillMenu;
    public CharacterSkillButton CharacterSkillButton;

    private void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(OnMouseDown);

        CS = GameObject.FindGameObjectWithTag("Character").GetComponent<CharacterState>();
    }

    private void OnMouseDown()
    {
        for (int i = 0; i < CS.character.skills.Count; i++)
        {
            if (GetComponent<Button>().name == "CharacterSkill00" + Convert.ToString(i + 1))
            {
                CS.which_skill = i;
            }
        }
        CharacterMenu.SetActive(false);
        CharacterSkillMenu.SetActive(false);
    }
}
