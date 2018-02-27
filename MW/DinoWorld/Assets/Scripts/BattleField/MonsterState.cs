using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterState : MonoBehaviour {

    private BattleState BS;
    public MonsterMain monster;

    public enum TurnState { PROCESSING, CHOOSEACTION, STOP, WAITING, ACTION, REVERSE, DEAD }

    public TurnState currentState;

    private float cur_cooldown;
    private float max_cooldown;

    private Vector3 startPosition;

    private bool actionStarted = false;
    public GameObject ForceToAttack;
    private float animSpeed = 20.0f;

    private bool alive = true;

    public Image ProgressBar;
    public Image ProgressBarEnd;
    private Vector3 startBarPosition;
    public Text DamageText;

    private int attack_or_skill;
    private int which_skill;
    private int calc_damage;

    // Use this for initialization
    void Start()
    {
        FirstSetting();
        currentState = TurnState.PROCESSING;
        BS = GetComponent<BattleState>();
        startPosition = transform.position;
        startBarPosition = ProgressBar.transform.position;
        max_cooldown = ProgressBarEnd.transform.position.x - 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                UpgradeProgressBar();
                break;

            case (TurnState.CHOOSEACTION):
                ChooseAction();
                break;

            case (TurnState.STOP):
                break;

            case (TurnState.WAITING):
                break;

            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());
                break;

            case (TurnState.REVERSE):
                SearchNextPerformer();
                break;

            case (TurnState.DEAD):
                if(!alive)
                {
                    return;
                }
                else
                {
                    this.gameObject.tag = "DeadMonster";

                    BS.MonstersInBattle.Remove(this.gameObject);

                    for(int i = 0; i < BS.PerformList.Count; i++)
                    {
                        if(BS.PerformList[i].AttackersGameObject == this.gameObject)
                        {
                            BS.PerformList.Remove(BS.PerformList[i]);
                        }
                    }

                    gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105, 105, 105, 255);
                    ProgressBar.color = new Color32(105, 105, 105, 0);

                    alive = false;
                }
                break;
        }
    }

    void FirstSetting()
    {
        monster.baseHP = monster.curHP = 10 + monster.vital * 5;
        monster.baseMP = monster.curMP = 5 + monster.intellect * 2;
        monster.baseATK = monster.curATK = 1.0f + monster.strength + (monster.dexterity / 2.0f) + (monster.intellect / 2.0f);
        monster.baseDEF = monster.curDEF = 1.0f + (monster.strength + monster.vital) / 2.0f;
        monster.baseSPD = monster.curSPD = 1.0f + monster.dexterity * 0.1f;

        cur_cooldown = monster.baseSPD / 10.0f;
    }

    void UpgradeProgressBar()
    {
        DamageText.text = "";
        cur_cooldown = cur_cooldown + (monster.curSPD / 10 * Time.deltaTime);
        ProgressBar.transform.Translate(Vector3.right * cur_cooldown);

        if (ProgressBar.transform.position.x >= max_cooldown)
        {
            cur_cooldown = monster.curSPD / 10;

            currentState = TurnState.CHOOSEACTION;
        }
    }

    void ChooseAction()
    {
        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = monster.MonsterName;
        myAttack.Type = "Monster";
        myAttack.AttackersGameObject = this.gameObject;
        myAttack.AttackersTarget = BS.ForcesInBattle[Random.Range(0, BS.ForcesInBattle.Count)];

        attack_or_skill = Random.Range(1, 3);
        if (attack_or_skill == 1)
        {
            which_skill = 0;
            myAttack.choosenAttack = monster.attacks[which_skill];
        }
        else
        {
            which_skill = Random.Range(0, monster.skills.Count);
            myAttack.choosenSkill = monster.skills[which_skill];
        }
        BS.CollectActions(myAttack);
    }

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        Vector3 CharacterPosition = new Vector3(ForceToAttack.transform.position.x + 2.0f,
                                                ForceToAttack.transform.position.y,
                                                ForceToAttack.transform.position.z);

        while (MoveTowardsMonster(CharacterPosition))
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        if(BS.PerformList[0].choosenAttack)
        {
            AttackDamage();
            Debug.Log(this.gameObject.name + " has choosen " + BS.PerformList[0].choosenAttack.attackName +
                  " and do " + calc_damage + " damage to " + BS.PerformList[0].AttackersTarget.name + "!");
            DamageText = GameObject.Find(BS.PerformList[0].AttackersTarget.name + "Damage").GetComponent<Text>();
            if (calc_damage == 0)
            {
                DamageText.text = "   " + calc_damage;
            }
            else
            {
                DamageText.text = "- " + calc_damage;
            }
            DamageText.fontStyle = FontStyle.Bold;
        }
        else
        {
            SkillDamage();
            Debug.Log(this.gameObject.name + " has choosen " + BS.PerformList[0].choosenSkill.skillName +
                  " and do " + calc_damage + " damage to " + BS.PerformList[0].AttackersTarget.name + "!");
            DamageText = GameObject.Find(BS.PerformList[0].AttackersTarget.name + "Damage").GetComponent<Text>();
            if (calc_damage == 0)
            {
                DamageText.text = "  " + calc_damage;
            }
            else
            {
                DamageText.text = "- " + calc_damage;
            }
            DamageText.fontStyle = FontStyle.Bold;
        }

        Vector3 firstPosition = startPosition;
        ProgressBar.transform.position = startBarPosition;
        while (MoveTowardsStart(firstPosition))
        {
            yield return null;
        }

        BS.PerformList.RemoveAt(0);

        BS.battleStates = BattleState.PerformAction.WAIT;

        actionStarted = false;

        attack_or_skill = 0;
        which_skill = 0;

        cur_cooldown = 0f;
        currentState = TurnState.REVERSE;
    }

    private bool MoveTowardsMonster(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    void AttackDamage()
    {
        calc_damage = (int)(monster.curATK * BS.PerformList[0].choosenAttack.attackDamage);
        if (BS.PerformList[0].AttackersTarget.tag == "Character")
        {
            ForceToAttack.GetComponent<CharacterState>().TakeDamage(calc_damage);
        }
        else if (BS.PerformList[0].AttackersTarget.tag == "Pet")
        {
            ForceToAttack.GetComponent<PetState>().TakeDamage(calc_damage);
        }
    }

    void SkillDamage()
    {
        calc_damage = (int)(monster.curATK * BS.PerformList[0].choosenSkill.skillDamage);
        if (BS.PerformList[0].AttackersTarget.tag == "Character")
        {
            ForceToAttack.GetComponent<CharacterState>().TakeDamage(calc_damage);
        }
        else if (BS.PerformList[0].AttackersTarget.tag == "Pet")
        {
            ForceToAttack.GetComponent<PetState>().TakeDamage(calc_damage);
        }
    }

    public void TakeDamage(int getDamageAmount)
    {
        monster.curHP -= getDamageAmount;
        if (monster.curHP <= 0)
        {
            monster.curHP = 0;
            currentState = TurnState.DEAD;
        }
    }

    void SearchNextPerformer()
    {
        CharacterState CS = BS.notPerformer1.GetComponent<CharacterState>();
        CS.currentState = CharacterState.TurnState.PROCESSING;

        PetState PS = BS.notPerformer2.GetComponent<PetState>();
        PS.currentState = PetState.TurnState.PROCESSING;

        for (int i = 0; i < BS.MonstersInBattle.Count; i++)
        {
            BS.notPerformers = GameObject.Find(BS.MonstersInBattle[i].name);
            MonsterState MS = BS.notPerformers.GetComponent<MonsterState>();
            MS.currentState = MonsterState.TurnState.PROCESSING;
        }
    }
}
