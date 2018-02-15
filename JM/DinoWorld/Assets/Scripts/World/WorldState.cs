using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour {

    public enum PerformSituation { WAIT, COUNTING, ADDLIST }

    public PerformSituation worldStates;

    public GameObject Player;
    public List<GameObject> Monsters = new List<GameObject>();

    private ChangeScene SceneChange;

    public float fightDurationTime;
    public int fightTimeCount;
    public int MonsterCount = 0;

	// Use this for initialization
	void Start ()
    {
        worldStates = PerformSituation.WAIT;

        SceneChange = GameObject.Find("ChangeSceneManager").GetComponent<ChangeScene>();

        Player = GameObject.Find("Player");
        Monsters.AddRange(GameObject.FindGameObjectsWithTag("mon"));
    }

    // Update is called once per frame
    void Update()
    {
        switch (worldStates)
        {
            case (PerformSituation.WAIT):
                break;

            case (PerformSituation.COUNTING):
                fightDurationTime -= Time.deltaTime;

                if (fightDurationTime < fightTimeCount)
                {
                    Debug.Log(fightTimeCount + "초 뒤에 " + MonsterCount + "명의 몬스터와 전투가 시작됩니다.");
                    fightTimeCount--;
                }

                if (fightDurationTime < 0)
                {
                    fightDurationTime = 5f;

                    worldStates = PerformSituation.WAIT;
                    SceneChange.WorldToBattle();
                }
                break;

            case (PerformSituation.ADDLIST):
                MonsterCount++;
                fightDurationTime = 5f;
                fightTimeCount = 5;
                worldStates = PerformSituation.COUNTING;
                break;
        }
    }
}
