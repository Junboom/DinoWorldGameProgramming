using UnityEngine;
using UnityEngine.UI;

public class PetAttackButton : MonoBehaviour
{
    public Button yourButton;

    private PetState PS;

    public GameObject PetMenu;

    private void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(OnMouseDown);

        PS = GameObject.FindGameObjectWithTag("Pet").GetComponent<PetState>();
    }

    private void OnMouseDown()
    {
        PS.attack_or_skill = 1;
        PetMenu.SetActive(false);
    }
}
