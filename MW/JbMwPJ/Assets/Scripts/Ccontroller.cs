using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ccontroller : MonoBehaviour {

    public bool alive = true;

    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;

    public float gravity;
    public float speedZ;

    public string Pstate = "idle";

    void Start()
    {
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
            /* if (controller.isGrounded)
             {
                 moveDirection.y = 0;
             }*/

            //속도 0이상이면 달리고 있는 플레그를 true
            animator.SetBool("run", moveDirection.z > 0.0f);  // idle 과 Run 애니메이션의 제어
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "eyes")
        {
            Debug.Log("이시발새낀또왜 지랄이야");
            
            other.transform.parent.GetComponent<MonNav>().CheckSight();
        }
    }


}
