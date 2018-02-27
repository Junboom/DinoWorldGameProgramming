using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldState : MonoBehaviour {

    public List<GameObject> monsterToBattle = new List<GameObject>();
    public List<GameObject> possibleEnemys = new List<GameObject>();
    public static WorldState instance;

    //위치
    public Vector3 nextPlayerPosition;
    public Vector3 lastPlayerPosition;

    //씬
    public string sceneToLoad;
    public string lastScene;

    //bools
    public bool isWalking = false;
    public bool canGetEncounter = false;
    public bool gotAttacked = false;

    private BattleState BS;
    private PlayerController PC;

    public enum PerformSituation { WAIT, COUNTING, ADDLIST, CaveState, Battle_state }

    public PerformSituation worldStates;

    public GameObject Player;
    public List<GameObject> Monsters = new List<GameObject>();

    private ChangeScene SceneChange;

    public float fightDurationTime;
    public int fightTimeCount;
    public int MonsterCount =0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);  //새 씬, 같은 씬에 이 게임오브잭트 존재

        if (!GameObject.Find("Player"))
        {
            GameObject player = Instantiate(Player, nextPlayerPosition, Quaternion.identity) as GameObject;

            Player.transform.position = nextPlayerPosition;
        }
    }

    // Use this for initialization
    void Start ()
    {
        BS = GetComponent<BattleState>();

        worldStates = PerformSituation.WAIT;

        // SceneChange = GetComponent<ChangeScene>();

        Player = GameObject.Find("Player");
        Monsters.AddRange(GameObject.FindGameObjectsWithTag("mon"));
    }

    // Update is called once per frame
    void Update()
    {
        switch (worldStates)
        {
            case (PerformSituation.WAIT):
                if (isWalking)
                {
                    RandomEncounter();
                }
                if (gotAttacked)
                {
                    worldStates = PerformSituation.Battle_state;
                }
                break;

            case (PerformSituation.COUNTING):
                fightDurationTime -= Time.deltaTime;

                if (fightDurationTime < fightTimeCount)
                {
                    Debug.Log(fightTimeCount + "초 뒤에 " + MonsterCount + "명의 몬스터와 전투가 시작됩니다.");
                    fightTimeCount--;
                }

                if (fightDurationTime <= 0)
                {
                    fightDurationTime = 5f;

                    worldStates = PerformSituation.WAIT;

                    Debug.Log(MonsterCount + " 몇마리냐");

                    StartBattle();
                    Debug.Log(monsterToBattle);
                    /* GameObject RemovedMonster = BS.GetComponent<BattleState>()
                    //("Monster00" + Convert.ToString(i)); //컨버트: 숫자를 문자열로 바꾸는거
                    BS.MonstersInBattle.Remove(RemovedMonster);
                    Destroy(RemovedMonster); */

                    //SceneManager.LoadScene("BattleField");
                }
                break;

            case (PerformSituation.ADDLIST):
                MonsterCount++;
                fightDurationTime = 5f;
                fightTimeCount = 5;
                worldStates = PerformSituation.COUNTING;
                break;

            case (PerformSituation.CaveState):
                if (isWalking)
                {
                    RandomEncounter();
                }
                if (gotAttacked)
                {
                    worldStates = PerformSituation.Battle_state;
                }
                break;

            case (PerformSituation.Battle_state):
                break;
        }
    }

    void StartBattle()
    {
        for(int i=0; i<MonsterCount ; i++)
        {
            monsterToBattle.Add(possibleEnemys[0]);
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    void RandomEncounter()
    {
        if (isWalking && canGetEncounter)
        {
            if(Random.Range(0,1000) < 10)
            {
                Debug.Log("걸림");
                gotAttacked = true;
            }
        }
    }
}
