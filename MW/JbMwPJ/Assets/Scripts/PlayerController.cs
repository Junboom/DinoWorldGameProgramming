using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    float movespeed = 10f;
    private string state = "idle";

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0.0f, moveZ);
        GetComponent<Rigidbody>().velocity = movement * movespeed; //* Time.deltaTime;


    }
}
