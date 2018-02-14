using UnityEngine;

public class PetSkillButton : MonoBehaviour
{
    private PetState PS;

    public GameObject PetMenu;

    private void Start()
    {
        PS = GameObject.FindGameObjectWithTag("Pet").GetComponent<PetState>();
    }

    private void OnMouseDown()
    {
        PS.attack_or_skill = 2;
        PetMenu.SetActive(false);
    }
}
