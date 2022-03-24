using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PhysicalSkillData", menuName = "SkillObjects/ScriptablePhysicalSkill", order = 1)]
public class PhysicalSkill : SkillObj
{
    protected GameObject activePhysicalHitbox;

    public float range;

    //public AnimationClip physAnimation;

    public override void Use(float skillProgDamage, GameObject curWielder)
    {
        wielder = curWielder;

        DisplayHitBox(skillProgDamage);
    }

    //enemy use() for knockback
    public override void Use(float skillProgDamage, GameObject curWielder, float knockbackAmount)
    {
        wielder = curWielder;

        knockbackStrength = knockbackAmount;

        DisplayHitBox(skillProgDamage);
    }

    void DisplayHitBox(float skillProgDamage)
    {
        var box = Instantiate(hurtboxPrefab, wielder.transform.position + wielder.transform.forward * range, wielder.transform.rotation);

        //get target from FindTarget
        var targetTemp = box.GetComponent<FindTarget>();

        //give FindTarget current skill
        targetTemp.skill = this;

        int tempDamage = (int)skillProgDamage;

        targetTemp.damage = tempDamage;

        box.transform.parent = wielder.transform;
        
        Destroy(box, .1f);
    }

}
