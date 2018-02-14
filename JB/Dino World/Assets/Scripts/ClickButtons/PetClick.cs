using UnityEngine;
using UnityEngine.UI;

public class PetClick : MonoBehaviour {

    private PetState PS;

    public GameObject CharacterMenu;
    public GameObject CharacterInfo;

    public GameObject PetMenu;
    public GameObject PetInfo;

    void Start()
    {
        PS = GameObject.Find("Pet001").GetComponent<PetState>();
    }

    private void OnMouseEnter()
    {
        PS.TextSetting();
        CharacterMenu.SetActive(false);
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
        if (!PetMenu.activeSelf)
        {
            CharacterMenu.SetActive(false);
            CharacterInfo.SetActive(false);
            PetMenu.SetActive(true);
        }
        else
        {
            PetMenu.SetActive(false);
        }
    }
}
