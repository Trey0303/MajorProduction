using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public List<SkillProgress> skill = new List<SkillProgress>();

    // Start is called before the first frame update
    void Start()
    {
        //if(skill.skillData != null)
        //{
        //    skill.AddSkill();
        //}

        for (int i = 0; i < skill.Count; i++)
        {
            //exp[i] = skillData[i].exp; 
            if (skill.Count != 0)
            {
                skill[i].AddSkill();

            }
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    Attack();

    //}

    public void Attack()
    {
        if (skill[0].skillData != null)
        {
            skill[0].Use(this.gameObject);

        }
    }

    internal void DiveAttack()
    {
        if (skill[1].skillData != null)
        {
            skill[1].Use(this.gameObject);

        }
    }
}
