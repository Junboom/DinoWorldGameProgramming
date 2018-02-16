using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSkillButton : MonoBehaviour
{
    public Button yourButton;
    public List<GameObject> SkillButtonList = new List<GameObject>();
    public Text[] SkillNameList = new Text[4];

    private CharacterState CS;

    public GameObject CharacterMenu;
    public GameObject CharacterSkillMenu;

    private void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(OnMouseDown);

        CS = GameObject.Find("Character").GetComponent<CharacterState>();
        
        CharacterSkillMenu.SetActive(false);
    }

    private void OnMouseDown()
    {
        CS.attack_or_skill = 2;
        CharacterSkillMenu.SetActive(true);

        for (int i = 0; i < CS.character.skills.Count; i++)
        {
            SkillButtonList[i].SetActive(true);
            SkillNameList[i].text = CS.character.skills[i].name;
        }
        for (int i = 3; i >= CS.character.skills.Count; i--)
        {
            SkillButtonList[i].SetActive(false);
        }
    }
}
