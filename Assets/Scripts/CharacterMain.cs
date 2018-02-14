using System.Collections.Generic;

[System.Serializable]
public class CharacterMain {

    public string CharaterName;

    public int strength;
    public int dexterity;
    public int vital;
    public int intellect;

    public int baseHP;
    public int curHP;

    public int baseMP;
    public int curMP;

    public float baseATK;
    public float curATK;

    public float baseDEF;
    public float curDEF;

    public float baseSPD;
    public float curSPD;

    public List<BaseAttack> attacks = new List<BaseAttack>();
    public List<SkillAttack> skills = new List<SkillAttack>();
}
