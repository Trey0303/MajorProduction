using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillProgress
{
    // The skill we are tracking
    public SkillObj skillData;

    public string name;

    // THe current amount of exp for this level
    public int exp;
    // The EXP needed to reach next level
    public int maxExp;

    // The current level
    public int level;

    // The current amount of damage
    public float damage;
    public int cost;
     
    public void AddExp(int expGain)
    {
        exp = exp + expGain;

        if(exp >= maxExp)
        {
            //level up skill
            level = level + 1;

            //increase damage
            damage = damage + 3;

            //update current exp
            exp = exp - maxExp;

            //increase maxExp
            maxExp = maxExp + 5;
        }

    }

    public void AddSkill()
    {
        //set defaults
        name = skillData.skillName;
        maxExp = 5;
        level = 1;
        damage = skillData.damage;
        cost = skillData.cost;
    }

    public void AddSkill(SkillObj newSkill)
    {
        //set defaults
        name = newSkill.skillName;
        maxExp = 5;
        level = 1;
        damage = newSkill.damage;
        cost = newSkill.cost;
    }

    // Use? - calls the skill and passes any relevant information
    public void Use()
    {
        // take the skill that I represent

        // tell it to be used, and how much damage it would deal if it hits
        skillData.Use(damage);
    }
}
