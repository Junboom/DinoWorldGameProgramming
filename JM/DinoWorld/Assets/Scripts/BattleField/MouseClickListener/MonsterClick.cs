using UnityEngine;
using UnityEngine.UI;

public class MonsterClick : MonoBehaviour {

    private BattleState BS;
    private CharacterState CS;
    private PetState PS;

    private GameObject performer;

    public Text MonsterName;

    void Start()
    {
        BS = GameObject.Find("BattleManager").GetComponent<BattleState>();
        CS = GameObject.Find("Character").GetComponent<CharacterState>();
        PS = GameObject.Find("Pet001").GetComponent<PetState>();
    }

    private void OnMouseEnter()
    {
        MonsterState MS = GetComponent<MonsterState>();
        MonsterName.text = MS.monster.MonsterName;
    }

    private void OnMouseExit()
    {
        MonsterName.text = "";
    }

    private void OnMouseDown()
    {
        GameObject performer = GameObject.Find(BS.PerformList[0].Attacker);

        if (BS.PerformList[0].Type == "Character")
        {
            if (CS.attack_or_skill != 0)
            {
                CS = performer.GetComponent<CharacterState>();
                CS.SelectedTarget = this.gameObject;
            }
        }
        else
        {
            if (PS.attack_or_skill != 0)
            {
                PS = performer.GetComponent<PetState>();
                PS.SelectedTarget = this.gameObject;
            }
        }
    }
}
