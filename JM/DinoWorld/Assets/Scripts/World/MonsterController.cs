using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour {


    public PlayerController Pstate;
    public GameObject Player;
    private NavMeshAgent nav;
    private string state = "idle";
    private bool alive = true;
    public static float fightdurationTime = 5f;
    public float monsterAmount = 0;

    private float alertness = 40f;
    public Transform eyes;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {

            if (state == "idle")
            {

                Vector3 randomPos = Random.insideUnitSphere * alertness;
                NavMeshHit navHit;
                NavMesh.SamplePosition(transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);
                nav.SetDestination(navHit.position);

            }
        }
        Debug.Log(monsterAmount);
        // Debug.Log(state);


        if (state == "fight")
        {
            fightdurationTime -= Time.deltaTime;
        }
        Debug.Log(fightdurationTime + "지속시간");
    }


    void OnMouseUp()
    {
        Debug.Log("후아웋아ㅜ항ㅁㄴ후아");
        if (gameObject.tag == "mon")
        {
            Pstate.Pstate = "fight";
            state = "fight";
            nav.SetDestination(Player.transform.position);
            monsterAmount++;



        }
    }

    public void CheckSight()
    {
        if (alive)
        {
            RaycastHit rayhit;
            if (Physics.Linecast(eyes.position, Player.transform.position, out rayhit) && fightdurationTime > 0)
            {
                Debug.Log(Pstate.Pstate + " 플레이어머냐");
                state = "fight";
                print("hit" + rayhit.collider.gameObject.name);
                nav.SetDestination(Player.transform.position);
                monsterAmount++;

            }
        }
    }


}
