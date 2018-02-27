using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : MonoBehaviour {

    public enum PerformAction { WAIT, TAKEACTION, STOP, PERFORMATION }

    public PerformAction battleStates;

    public List<HandleTurn> PerformList = new List<HandleTurn>();

    public List<GameObject> ForcesInBattle = new List<GameObject>();
    public List<GameObject> MonstersInBattle = new List<GameObject>();

    public GameObject notPerformer;
    public GameObject notPerformer1;
    public GameObject notPerformer2;
    public GameObject notPerformers;

    // Use this for initialization
    void Start () {
        battleStates = PerformAction.WAIT;

        ForcesInBattle.Add(GameObject.FindGameObjectWithTag("Character"));
        ForcesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Pet"));
        MonstersInBattle.AddRange(GameObject.FindGameObjectsWithTag("Monster"));
    }
	
	// Update is called once per frame
	void Update () {
		switch (battleStates)
        {
            case (PerformAction.WAIT):
                if(PerformList.Count > 0)
                {
                    if(PerformList[0].Type == "Monster")
                    {
                        battleStates = PerformAction.TAKEACTION;
                    }
                    else
                    {
                        battleStates = PerformAction.STOP;
                    }
                }
                break;

            case (PerformAction.TAKEACTION):
                GameObject performer = GameObject.Find(PerformList[0].Attacker);

                if (PerformList[0].Type == "Character")
                {
                    CharacterState CS = performer.GetComponent<CharacterState>();
                    CS.MonsterToAttack = PerformList[0].AttackersTarget;
                    CS.currentState = CharacterState.TurnState.ACTION;

                    notPerformer = GameObject.Find(ForcesInBattle[1].name);
                    PetState PS = notPerformer.GetComponent<PetState>();
                    PS.currentState = PetState.TurnState.WAITING;

                    for(int i = 0; i < MonstersInBattle.Count; i++)
                    {
                        notPerformers = GameObject.Find(MonstersInBattle[i].name);
                        MonsterState MS = notPerformers.GetComponent<MonsterState>();
                        MS.currentState = MonsterState.TurnState.WAITING;
                    }
                }
                else if(PerformList[0].Type == "Pet")
                {
                    notPerformer = GameObject.Find(ForcesInBattle[0].name);
                    CharacterState CS = notPerformer.GetComponent<CharacterState>();
                    CS.currentState = CharacterState.TurnState.WAITING;
                    
                    PetState PS = performer.GetComponent<PetState>();
                    PS.MonsterToAttack = PerformList[0].AttackersTarget;
                    PS.currentState = PetState.TurnState.ACTION;

                    for (int i = 0; i < MonstersInBattle.Count; i++)
                    {
                        notPerformers = GameObject.Find(MonstersInBattle[i].name);
                        MonsterState MS = notPerformers.GetComponent<MonsterState>();
                        MS.currentState = MonsterState.TurnState.WAITING;
                    }
                }
                else
                {
                    notPerformer1 = GameObject.Find(ForcesInBattle[0].name);
                    CharacterState CS = notPerformer1.GetComponent<CharacterState>();
                    CS.currentState = CharacterState.TurnState.WAITING;

                    notPerformer2 = GameObject.Find(ForcesInBattle[1].name);
                    PetState PS = notPerformer2.GetComponent<PetState>();
                    PS.currentState = PetState.TurnState.WAITING;

                    for (int i = 0; i < MonstersInBattle.Count; i++)
                    {
                        notPerformers = GameObject.Find(MonstersInBattle[i].name);
                        MonsterState nMS = notPerformers.GetComponent<MonsterState>();
                        nMS.currentState = MonsterState.TurnState.WAITING;
                    }
                    
                    MonsterState MS = performer.GetComponent<MonsterState>();
                    for(int i = 0; i < ForcesInBattle.Count; i++)
                    {
                        if(PerformList[0].AttackersTarget == ForcesInBattle[i])
                        {
                            MS.ForceToAttack = PerformList[0].AttackersTarget;
                            MS.currentState = MonsterState.TurnState.ACTION;
                            break;
                        }
                    }
                }
                for(int i = 1; i < PerformList.Count; i++)
                {
                    if (PerformList[0].Attacker == PerformList[i].Attacker)
                    {
                        PerformList.RemoveAt(i);
                    }
                }
                break;

            case (PerformAction.STOP):
                notPerformer1 = GameObject.Find(ForcesInBattle[0].name);
                CharacterState nCS = notPerformer1.GetComponent<CharacterState>();
                nCS.currentState = CharacterState.TurnState.WAITING;

                notPerformer2 = GameObject.Find(ForcesInBattle[1].name);
                PetState nPS = notPerformer2.GetComponent<PetState>();
                nPS.currentState = PetState.TurnState.WAITING;

                for (int i = 0; i < MonstersInBattle.Count; i++)
                {
                    notPerformers = GameObject.Find(MonstersInBattle[i].name);
                    MonsterState nMS = notPerformers.GetComponent<MonsterState>();
                    nMS.currentState = MonsterState.TurnState.WAITING;
                }

                if (nCS.SelectedTarget)
                {
                    PerformList[0].AttackersTarget = nCS.MonsterToAttack = nCS.SelectedTarget;
                    if (nCS.attack_or_skill == 1)
                    {
                        nCS.which_skill = 0;
                        PerformList[0].choosenAttack = nCS.character.attacks[nCS.which_skill];
                        nCS.currentState = CharacterState.TurnState.ACTION;
                        battleStates = PerformAction.TAKEACTION;
                    }
                    else if (nCS.attack_or_skill == 2)
                    {
                        nCS.character.curMP -= nCS.character.skills[nCS.which_skill].skillCost;
                        if(nCS.character.curMP < 0)
                        {
                            Debug.Log("기술을 사용하기 위한 기력이 충분하지 않습니다.");
                            nCS.character.curMP += nCS.character.skills[nCS.which_skill].skillCost;
                            nCS.attack_or_skill = 3;
                            nCS.SelectedTarget = null;
                        }
                        else
                        {
                            PerformList[0].choosenSkill = nCS.character.skills[nCS.which_skill];
                            nCS.currentState = CharacterState.TurnState.ACTION;
                            battleStates = PerformAction.TAKEACTION;
                        }
                    }
                }
                else if(nPS.SelectedTarget)
                {
                    PerformList[0].AttackersTarget = nPS.MonsterToAttack = nPS.SelectedTarget;
                    if (nPS.attack_or_skill == 1)
                    {
                        nPS.which_skill = 0;
                        PerformList[0].choosenAttack = nPS.pet.attacks[nPS.which_skill];
                        nPS.currentState = PetState.TurnState.ACTION;
                        battleStates = PerformAction.TAKEACTION;
                    }
                    else if(nPS.attack_or_skill == 2)
                    {
                        nPS.pet.curMP -= nPS.pet.skills[nPS.which_skill].skillCost;
                        if (nPS.pet.curMP < 0)
                        {
                            Debug.Log("기술을 사용하기 위한 기력이 충분하지 않습니다.");
                            nPS.pet.curMP += nPS.pet.skills[nPS.which_skill].skillCost;
                            nPS.attack_or_skill = 3;
                            nPS.SelectedTarget = null;
                        }
                        else
                        {
                            PerformList[0].choosenSkill = nPS.pet.skills[nPS.which_skill];
                            nPS.currentState = PetState.TurnState.ACTION;
                            battleStates = PerformAction.TAKEACTION;
                        }
                    }
                }
                break;

            case (PerformAction.PERFORMATION):
                ReturnToWorld();
                break;
        }
	}

    public void CollectActions(HandleTurn input)
    {
        PerformList.Add(input);
    }

    public void ReturnToWorld()
    {
        for (int i = 1; i <= 4; i++)
        {
            GameObject CreatedMonster = GameObject.Find("Monster00" + Convert.ToString(i));
            CreatedMonster.SetActive(true);

            battleStates = PerformAction.WAIT;
        }
    }
}
