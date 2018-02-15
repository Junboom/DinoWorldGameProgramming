﻿using UnityEngine;

[System.Serializable]
public class HandleTurn {

    public string Attacker;
    public string Type;
    public GameObject AttackersGameObject;
    public GameObject AttackersTarget;

    public BaseAttack choosenAttack;
    public SkillAttack choosenSkill;
}
