using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour {

    public PlayerController Pstate;
    public GameObject Player;
    private NavMeshAgent nav;
    public string state = "idle";
    private bool alive = true;

    private float alertness = 40f;
    public Transform eyes;

    private WorldState WS;

    void Start()
    {
        WS = GameObject.Find("WorldManager").GetComponent<WorldState>();

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
        // Debug.Log(monsterAmount);
        // Debug.Log(state);

        if (state == "fight")
        {
            WS.worldStates = WorldState.PerformSituation.ADDLIST;

            state = "stop";
        }

        if(state == "stop")
        {
            if(WS.monAlive)
            {

            }
            else
            {
                state = "delete";
            }
        }

        if(state == "delete")
        {
            Destroy(gameObject);
        }
    }

    void OnMouseUp()
    {
        if (gameObject.tag == "mon" && state == "idle")
        {
            Pstate.Pstate = "fight";
            state = "fight";

            Vector3 PlayerPosition = new Vector3(WS.Player.transform.position.x,
                                                 WS.Player.transform.position.y,
                                                 WS.Player.transform.position.z + WS.MonsterCount + 1);
            nav.SetDestination(PlayerPosition);
        }
    }

    public void CheckSight()
    {
        if (alive)
        {
            RaycastHit rayhit;
            if (Physics.Linecast(eyes.position, Player.transform.position, out rayhit) && state == "idle")
            {
                state = "fight";
                // print("hit" + rayhit.collider.gameObject.name);
                Vector3 PlayerPosition = new Vector3(WS.Player.transform.position.x,
                                                     WS.Player.transform.position.y,
                                                     WS.Player.transform.position.z + WS.MonsterCount);
                nav.SetDestination(PlayerPosition);
            }
        }
    }
}
