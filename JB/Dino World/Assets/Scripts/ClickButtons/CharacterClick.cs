using UnityEngine;

public class CharacterClick : MonoBehaviour {

    private CharacterState CS;

    public GameObject CharacterMenu;
    public GameObject CharacterInfo;

    public GameObject PetMenu;
    public GameObject PetInfo;

    private void Start()
    {
        CS = GameObject.Find("Character").GetComponent<CharacterState>();
    }

    private void OnMouseEnter()
    {
        CS.TextSetting();
        CharacterInfo.SetActive(true);
        PetMenu.SetActive(false);
        PetInfo.SetActive(false);
    }

    private void OnMouseExit()
    {
        CharacterInfo.SetActive(false);
        PetInfo.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (!CharacterMenu.activeSelf)
        {
            CharacterMenu.SetActive(true);
            PetMenu.SetActive(false);
            PetInfo.SetActive(false);
        }
        else
        {
            CharacterMenu.SetActive(false);
        }
    }
}
