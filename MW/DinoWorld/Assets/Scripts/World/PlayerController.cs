using UnityEngine;

public class PlayerController : MonoBehaviour {

    private WorldState WS;

    public bool alive = true;

    CharacterController controller;
    Animator animator;

    public Vector3 PlayerPosition;

    Vector3 moveDirection = Vector3.zero;

    Vector3 curPos, lastPos;

    public float gravity;
    public float speedZ;

    public string Pstate = "idle";

    void Start()
    {
        WS = GameObject.Find("WorldManager").GetComponent<WorldState>();

        transform.position = WorldState.instance.nextPlayerPosition;
        //필요한 컴퍼넌트를 자동취득
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Pstate == "idle")
        {
            if (controller.isGrounded)
            {
                moveDirection.z = Input.GetAxis("Vertical") * speedZ;
            }
            else
            {
                moveDirection.z = 0;
            }

            //방향전환
            transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);

            //중력만큼의 힘 매 프레임 추가
            moveDirection.y -= gravity * Time.deltaTime;

            //이동실행
            Vector3 globalDirection = transform.TransformDirection(moveDirection);
            controller.Move(globalDirection * Time.deltaTime);

            //이동후 접지 하고있으면 와이방향 속도 리셋
            /*
            if (controller.isGrounded)
            {
                moveDirection.y = 0;
            }
            */

            curPos = transform.position;
            if (curPos == lastPos)
            {
                WorldState.instance.isWalking = false;
            }
            else
            {
                WorldState.instance.isWalking = true;
            }
            lastPos = curPos;

            //속도 0이상이면 달리고 있는 플레그를 true
            animator.SetBool("run", moveDirection.z > 0.0f);  // idle 과 Run 애니메이션의 제어
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "eyes")
        {
            other.transform.parent.GetComponent<MonsterController>().CheckSight();
        }
        if(other.tag  == "entercave")
        {
            Debug.Log("가즈아");
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
            WorldState.instance.nextPlayerPosition = col.spwnPoint.transform.position;
            WorldState.instance.sceneToLoad = col.sceneToLoad;
            WorldState.instance.LoadNextScene();
        }
        if(other.tag == "leavecave")
        {
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
            WorldState.instance.nextPlayerPosition = col.spwnPoint.transform.position;
            WorldState.instance.sceneToLoad = col.sceneToLoad;
            WorldState.instance.LoadNextScene();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "caveRegion")
        {
            WorldState.instance.canGetEncounter = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "caveRegion")
        {
            WorldState.instance.canGetEncounter = false;
        }
    }
}
