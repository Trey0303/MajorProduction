using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PhysicalSkillData", menuName = "SkillObjects/ScriptablePhysicalSkill", order = 1)]
public class PhysicalSkill : SkillObj
{
    protected GameObject activePhysicalHitbox;

    public float rangeX;
    public float rangeY;

    //public AnimationClip physAnimation;

    public override void Use(float skillProgDamage, GameObject curWielder)
    {
        wielder = curWielder;

        DisplayHitBox(skillProgDamage);
    }


    public override void Use(float skillProgDamage, GameObject curWielder, float knockbackAmount, float knockbackTimeSet)
    {
        wielder = curWielder;

        knockbackStrength = knockbackAmount;

        knockbackTime = knockbackTimeSet;

        DisplayHitBox(skillProgDamage);
    }

    //enemy use() for knockback

    void DisplayHitBox(float skillProgDamage)
    {
        var box = Instantiate(hurtboxPrefab, new Vector3(wielder.transform.position.x, wielder.transform.position.y + rangeY, wielder.transform.position.z) + wielder.transform.forward * rangeX, wielder.transform.rotation);

        //get target from FindTarget
        var targetTemp = box.GetComponent<FindTarget>();

        //give FindTarget current skill
        targetTemp.skill = this;

        int tempDamage = (int)skillProgDamage;

        targetTemp.damage = tempDamage;

        box.transform.parent = wielder.transform;
        
        Destroy(box, activeHitBoxTimer);
        //if (wielder.tag == "Enemy")
        //{
        //    if(wielder.GetComponent<EnemyAi>().attackDelay <= 0)
        //    {
        //        wielder.GetComponent<EnemyAi>().midAttack = false;

        //    }


        //}
    }

}
