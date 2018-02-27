using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterState : MonoBehaviour {

    private BattleState BS;
    private ChangeScene SceneChange;
    public CharacterMain character;

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

    public Image CharacterHP;
    public Image CharacterMP;
    public Image ProgressBar;
    public Image ProgressBarEnd;
    private Vector3 startBarPosition;
    public Text DamageText;

    public GameObject CharacterMenu;
    public GameObject CharacterInfo;

    public GameObject PetMenu;
    public GameObject PetInfo;

    public Text CharacterName;

    public Text CharacterHPText;
    public Text CharacterMPText;

    public int attack_or_skill;
    public int which_skill;
    int calc_damage;

    // Use this for initialization
    void Start ()
    {
        FirstSetting();
        TextSetting();
        currentState = TurnState.PROCESSING;
        BS = GameObject.Find("BattleManager").GetComponent<BattleState>();
        startPosition = transform.position;
        startBarPosition = ProgressBar.transform.position;
        max_cooldown = ProgressBarEnd.transform.position.x - 1.5f;
        CharacterMenu.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		switch (currentState)
        {
            case (TurnState.PROCESSING):
                UpgradeProgressBar ();
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
                CharacterInfo.SetActive(false);
                PetMenu.SetActive(false);
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
                    this.gameObject.tag = "DeadCharacter";

                    BS.ForcesInBattle.Remove(this.gameObject);

                    for (int i = 0; i < BS.PerformList.Count; i++)
                    {
                        if (BS.PerformList[i].AttackersGameObject == this.gameObject)
                        {
                            BS.PerformList.Remove(BS.PerformList[i]);
                        }
                    }

                    SceneChange = GameObject.Find("ChangeSceneManager").GetComponent<ChangeScene>();
                    SceneChange.BattleToWorld();

                    alive = false;
                }
                break;
        }
    }

    void FirstSetting()
    {
        character.baseHP = character.curHP = 20 + character.vital * 5;
        character.baseMP = character.curMP = 10 + character.intellect * 2;
        character.baseATK = character.curATK = 2.0f + character.strength + (character.dexterity / 2.0f) + (character.intellect / 2.0f);
        character.baseDEF = character.curDEF = 2.0f + (character.strength + character.vital) / 2.0f;
        character.baseSPD = character.curSPD = 1.0f + character.dexterity * 0.1f;

        cur_cooldown = character.baseSPD / 10.0f;
    }

    public void TextSetting()
    {
        CharacterName.text = character.CharaterName;

        if (character.curHP > character.baseHP)
        {
            character.curHP = character.baseHP;
        }
        if (character.curMP > character.baseMP)
        {
            character.curMP = character.baseMP;
        }

        CharacterHPText.text = character.curHP + " / " + character.baseHP;
        CharacterHPText.fontStyle = FontStyle.Bold;
        CharacterMPText.text = character.curMP + " / " + character.baseMP;
        CharacterMPText.fontStyle = FontStyle.Bold;

        float remainHP = (character.curHP+0.01f) / (character.baseHP+0.01f);
        float remainMP = (character.curMP+0.01f) / (character.baseMP+0.01f);
        
        CharacterHP.transform.localScale = new Vector3(remainHP, CharacterHP.transform.localScale.y, CharacterHP.transform.localScale.z);
        CharacterMP.transform.localScale = new Vector3(remainMP, CharacterMP.transform.localScale.y, CharacterMP.transform.localScale.z);
    }

    void UpgradeProgressBar()
    {
        DamageText.text = "";
        cur_cooldown = cur_cooldown + (character.curSPD / 10 * Time.deltaTime);
        ProgressBar.transform.Translate(Vector3.right * cur_cooldown);

        if (ProgressBar.transform.position.x >= max_cooldown)
        {
            cur_cooldown = character.curSPD / 10;

            currentState = TurnState.CHOOSEACTION;
        }
    }

    void ChooseAction()
    {
        CharacterMenu.SetActive(true);
        CharacterInfo.SetActive(true);
        PetMenu.SetActive(false);
        PetInfo.SetActive(false);

        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = character.CharaterName;
        myAttack.Type = "Character";
        myAttack.AttackersGameObject = this.gameObject;
        myAttack.AttackersTarget = SelectedTarget;
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
            if(calc_damage == 0)
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

        BS.battleStates = BattleState.PerformAction.PERFORMATION;

        actionStarted = false;

        attack_or_skill = 0;
        which_skill = 0;

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
        calc_damage = (int)(character.curATK * BS.PerformList[0].choosenAttack.attackDamage);
        MonsterToAttack.GetComponent<MonsterState>().TakeDamage(calc_damage);
    }

    void SkillDamage()
    {
        calc_damage = (int)(character.curATK * BS.PerformList[0].choosenSkill.skillDamage);
        MonsterToAttack.GetComponent<MonsterState>().TakeDamage(calc_damage);
    }

    public void TakeDamage(int getDamageAmount)
    {
        character.curHP -= getDamageAmount;
        if(character.curHP <= 0)
        {
            character.curHP = 0;
            currentState = TurnState.DEAD;
        }
    }

    void SearchNextPerformer()
    {
        currentState = TurnState.PROCESSING;

        PetState PS = BS.notPerformer.GetComponent<PetState>();
        PS.currentState = PetState.TurnState.PROCESSING;

        for (int i = 0; i < BS.MonstersInBattle.Count; i++)
        {
            BS.notPerformers = GameObject.Find(BS.MonstersInBattle[i].name);
            MonsterState MS = BS.notPerformers.GetComponent<MonsterState>();
            MS.currentState = MonsterState.TurnState.PROCESSING;
        }
    }
}
