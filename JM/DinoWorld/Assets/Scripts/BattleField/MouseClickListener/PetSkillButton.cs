using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetSkillButton : MonoBehaviour
{
    public Button yourButton;
    public List<GameObject> SkillButtonList = new List<GameObject>();
    public Text[] SkillNameList = new Text[4];

    private PetState PS;

    public GameObject PetMenu;
    public GameObject PetSkillMenu;

    private void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(OnMouseDown);

        PS = GameObject.FindGameObjectWithTag("Pet").GetComponent<PetState>();

        PetSkillMenu.SetActive(false);
    }

    private void OnMouseDown()
    {
        PS.attack_or_skill = 2;
        PetSkillMenu.SetActive(true);

        for (int i = 0; i < PS.pet.skills.Count; i++)
        {
            SkillButtonList[i].SetActive(true);
            SkillNameList[i].text = PS.pet.skills[i].name;
        }
        for(int i = 3; i >= PS.pet.skills.Count; i--)
        {
            SkillButtonList[i].SetActive(false);
        }
    }
}
