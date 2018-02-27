using UnityEngine;

public class PetClick : MonoBehaviour {

    private PetState PS;

    public GameObject CharacterMenu;
    public GameObject CharacterInfo;
    public GameObject CharacterSkillMenu;

    public GameObject PetMenu;
    public GameObject PetInfo;
    public GameObject PetSkillMenu;

    void Start()
    {
        PS = GameObject.Find("Pet001").GetComponent<PetState>();
    }

    private void OnMouseEnter()
    {
        PS.TextSetting();
        CharacterInfo.SetActive(false);
        PetInfo.SetActive(true);
    }

    private void OnMouseExit()
    {
        CharacterInfo.SetActive(false);
        PetInfo.SetActive(false);
    }

    public void OnMouseDown()
    {
        if (PS.attack_or_skill != 0)
        {
            if (!PetMenu.activeSelf)
            {
                CharacterMenu.SetActive(false);
                CharacterInfo.SetActive(false);
                CharacterSkillMenu.SetActive(false);
                PetMenu.SetActive(true);
            }
            else
            {
                PetMenu.SetActive(false);
                PetSkillMenu.SetActive(false);
            }
        }
    }
}
