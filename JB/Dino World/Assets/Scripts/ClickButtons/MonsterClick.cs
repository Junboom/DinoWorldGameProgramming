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
        if(BS.PerformList[0].Type == "Character")
        {
            CS = performer.GetComponent<CharacterState>();
            CS.SelectedTarget = this.gameObject;
        }
        else
        {
            PS = performer.GetComponent<PetState>();
            PS.SelectedTarget = this.gameObject;
        }
    }
}
