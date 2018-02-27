using System;
using UnityEngine;
using UnityEngine.UI;

public class PetUsingSkill : MonoBehaviour
{
    public Button yourButton;

    private PetState PS;

    public GameObject PetMenu;
    public GameObject PetSkillMenu;
    public PetSkillButton PetSkillButton;

    private void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(OnMouseDown);
        
        PS = GameObject.FindGameObjectWithTag("Pet").GetComponent<PetState>();
    }

    private void OnMouseDown()
    {
        for(int i = 0; i < PS.pet.skills.Count; i++)
        {
            if(GetComponent<Button>().name == "PetSkill00" + Convert.ToString(i+1))
            {
                PS.which_skill = i;
            }
        }
        PetMenu.SetActive(false);
        PetSkillMenu.SetActive(false);
    }
}
