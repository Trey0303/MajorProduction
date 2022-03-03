using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public SkillProgress skill;

    // Start is called before the first frame update
    void Start()
    {
        if(skill != null)
        {
            skill.AddSkill();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))//left click
        {
            skill.Use();
        }
    }
}
