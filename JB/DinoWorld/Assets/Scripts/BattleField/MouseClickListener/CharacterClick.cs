using UnityEngine;

public class CharacterClick : MonoBehaviour {

    private CharacterState CS;

    public GameObject CharacterMenu;
    public GameObject CharacterInfo;
    public GameObject CharacterSkillMenu;

    public GameObject PetMenu;
    public GameObject PetInfo;
    public GameObject PetSkillMenu;

    private void Start()
    {
        CS = GameObject.Find("Character").GetComponent<CharacterState>();
    }

    private void OnMouseEnter()
    {
        CS.TextSetting();
        CharacterInfo.SetActive(true);
        PetInfo.SetActive(false);
    }

    private void OnMouseExit()
    {
        CharacterInfo.SetActive(false);
        PetInfo.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (CS.attack_or_skill != 0)
        {
            if (!CharacterMenu.activeSelf)
            {
                CharacterMenu.SetActive(true);
                PetMenu.SetActive(false);
                PetInfo.SetActive(false);
                PetSkillMenu.SetActive(false);
            }
            else
            {
                CharacterMenu.SetActive(false);
                CharacterSkillMenu.SetActive(false);
            }
        }
    }
}
