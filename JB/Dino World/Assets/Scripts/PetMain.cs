using System.Collections.Generic;

[System.Serializable]
public class PetMain
{
    public string PetName;

    public enum Type1 { NORMAL, GRASS, FIRE, WATER, ELECTRIC, WIND, ROCK }
    public enum Type2 { NONE, GRASS, FIRE, WATER, ELECTRIC, WIND, ROCK }

    public Type1 MonsterType1;
    public Type2 MonsterType2;

    public int strength;
    public int dexterity;
    public int vital;
    public int intellect;
    public int loyalty;

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
