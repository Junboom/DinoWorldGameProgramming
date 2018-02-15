using UnityEngine;

[System.Serializable]
public class SkillAttack : MonoBehaviour {

    public string skillName;
    public string skillDescription;
    public float skillDamage;
    public float skillCost;

    public SkillAttack chooseSkill;
}
