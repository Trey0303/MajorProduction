using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeSkillData", menuName = "SkillObjects/ScriptableRangeSkill", order = 1)]
public class RangeSkill : SkillObj
{
    public int range;

    protected GameObject activeRangeHitbox;

    // Start is called before the first frame update
    public override void Use(float skillProgDamage, GameObject curWielder)
    {
        wielder = curWielder;


        DisplayHitBox(skillProgDamage);
    }

    void DisplayHitBox(float skillProgDamage)
    {
        var box = Instantiate(hurtboxPrefab, wielder.transform.position + wielder.transform.forward * range, Quaternion.identity);

        var targetTemp = box.GetComponent<FindTarget>();

        targetTemp.skill = this;

        int tempDamage = (int)skillProgDamage;

        targetTemp.damage = tempDamage;

        box.transform.parent = wielder.transform;

        Destroy(box, .1f);
    }

    
}
