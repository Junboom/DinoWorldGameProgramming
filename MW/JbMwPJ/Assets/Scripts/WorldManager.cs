using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {


    MonNav MonsterAmount;
    public static WorldManager instance;
    public float MonAmount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartBattle()
    {
        MonAmount = MonsterAmount.monsterAmount;
    }

}
