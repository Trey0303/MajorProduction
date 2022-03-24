using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public SkillProgress skill;

    // Start is called before the first frame update
    void Start()
    {
        if(skill.skillData != null)
        {
            skill.AddSkill();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerControllerIsometric.canMove)
        {
            if (Input.GetMouseButtonUp(0))//left click
            {
                if(skill.skillData != null)
                {
                    skill.Use(this.gameObject);

                }
            }

        }

    }
}
