using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PetState : MonoBehaviour {

    private BattleState BS;
    public PetMain pet;

    public enum TurnState { PROCESSING, CHOOSEACTION, STOP, WAITING, ACTION, REVERSE, DEAD }

    public TurnState currentState;

    private float cur_cooldown;
    private float max_cooldown;

    private Vector3 startPosition;

    private bool actionStarted = false;
    public GameObject MonsterToAttack;
    public GameObject SelectedTarget;
    private float animSpeed = 20.0f;

    private bool alive = true;

    public Image ProgressBar;
    public Image ProgressBarEnd;
    private Vector3 startBarPosition;
    public Text DamageText;

    public GameObject CharacterMenu;
    public GameObject CharacterInfo;

    public GameObject PetMenu;
    public GameObject PetInfo;

    public Text PetName;

    public Text PetHPText;
    public Text PetMPText;

    public int attack_or_skill, which_attack;
    int calc_damage;

    // Use this for initialization
    void Start()
    {
        FirstSetting();
        TextSetting();
        currentState = TurnState.PROCESSING;
        BS = GameObject.Find("BattleManager").GetComponent<BattleState>();
        startPosition = transform.position;
        startBarPosition = ProgressBar.transform.position;
        max_cooldown = ProgressBarEnd.transform.position.x - 1.5f;
        PetMenu.SetActive(false);
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
                BS.battleStates = BattleState.PerformAction.STOP;
                ChooseAction();
                break;

            case (TurnState.STOP):
                break;

            case (TurnState.WAITING):
                break;

            case (TurnState.ACTION):
                CharacterMenu.SetActive(false);
                PetMenu.SetActive(false);
                PetInfo.SetActive(false);
                StartCoroutine(TimeForAction());
                break;

            case (TurnState.REVERSE):
                SearchNextPerformer();
                break;

            case (TurnState.DEAD):
                if (!alive)
                {
                    return;
                }
                else
                {
                    this.gameObject.tag = "DeadPet";

                    BS.ForcesInBattle.Remove(this.gameObject);

                    for (int i = 0; i < BS.PerformList.Count; i++)
                    {
                        if (BS.PerformList[i].AttackersGameObject == this.gameObject)
                        {
                            BS.PerformList.Remove(BS.PerformList[i]);
                        }
                    }

                    gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105, 105, 105, 255);

                    alive = false;
                }
                break;
        }
    }

    void FirstSetting()
    {
        pet.baseHP = pet.curHP = 10 + pet.vital * 5;
        pet.baseMP = pet.curMP = 5 + pet.intellect * 2;
        pet.baseATK = pet.curATK = 1.0f + pet.strength + (pet.dexterity / 2.0f) + (pet.intellect / 2.0f);
        pet.baseDEF = pet.curDEF = 1.0f + (pet.strength + pet.vital) / 2.0f;
        pet.baseSPD = pet.curSPD = 1.0f + pet.dexterity * 0.1f;

        cur_cooldown = pet.baseSPD / 10.0f;
    }

    public void TextSetting()
    {
        PetName.text = pet.PetName;

        PetHPText.text = pet.curHP + " / " + pet.baseHP;
        PetHPText.fontStyle = FontStyle.Bold;
        PetMPText.text = pet.curMP + " / " + pet.baseMP;
        PetMPText.fontStyle = FontStyle.Bold;
    }

    void UpgradeProgressBar()
    {
        DamageText.text = "";
        cur_cooldown = cur_cooldown + (pet.curSPD / 10 * Time.deltaTime);
        ProgressBar.transform.Translate(Vector3.right * cur_cooldown);

        if (ProgressBar.transform.position.x >= max_cooldown)
        {
            cur_cooldown = pet.curSPD / 10;

            currentState = TurnState.CHOOSEACTION;
        }
    }

    void ChooseAction()
    {
        CharacterMenu.SetActive(false);
        CharacterInfo.SetActive(false);
        PetMenu.SetActive(true);
        PetInfo.SetActive(true);

        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = pet.PetName;
        myAttack.Type = "Pet";
        myAttack.AttackersGameObject = this.gameObject;
        myAttack.AttackersTarget = SelectedTarget;

        // attack_or_skill = Random.Range(0, 2);
        if (attack_or_skill == 1)
        {
            which_attack = Random.Range(0, pet.attacks.Count);
            myAttack.choosenAttack = pet.attacks[which_attack];
        }
        else
        {
            which_attack = Random.Range(0, pet.skills.Count);
            myAttack.choosenSkill = pet.skills[which_attack];
        }
        BS.CollectActions(myAttack);

        currentState = TurnState.STOP;
    }

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        Vector3 CharacterPosition = new Vector3(MonsterToAttack.transform.position.x - 2.0f,
                                                MonsterToAttack.transform.position.y,
                                                MonsterToAttack.transform.position.z);

        while (MoveTowardsMonster(CharacterPosition))
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        if (BS.PerformList[0].choosenAttack)
        {
            AttackDamage();
            Debug.Log(this.gameObject.name + " has choosen " + BS.PerformList[0].choosenAttack.attackName +
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

        cur_cooldown = 0f;
        SelectedTarget = null;
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
        calc_damage = (int)(pet.curATK * BS.PerformList[0].choosenAttack.attackDamage);
        MonsterToAttack.GetComponent<MonsterState>().TakeDamage(calc_damage);
    }

    void SkillDamage()
    {
        calc_damage = (int)(pet.curATK * BS.PerformList[0].choosenSkill.skillDamage);
        MonsterToAttack.GetComponent<MonsterState>().TakeDamage(calc_damage);
    }

    public void TakeDamage(int getDamageAmount)
    {
        pet.curHP -= getDamageAmount;
        if (pet.curHP <= 0)
        {
            pet.curHP = 0;
            currentState = TurnState.DEAD;
        }
    }

    void SearchNextPerformer()
    {
        currentState = TurnState.PROCESSING;

        CharacterState CS = BS.notPerformer.GetComponent<CharacterState>();
        CS.currentState = CharacterState.TurnState.PROCESSING;

        for (int i = 0; i < BS.MonstersInBattle.Count; i++)
        {
            BS.notPerformers = GameObject.Find(BS.MonstersInBattle[i].name);
            MonsterState MS = BS.notPerformers.GetComponent<MonsterState>();
            MS.currentState = MonsterState.TurnState.PROCESSING;
        }
    }
}
