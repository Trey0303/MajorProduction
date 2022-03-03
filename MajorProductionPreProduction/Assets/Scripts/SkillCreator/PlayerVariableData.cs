using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVariableData : MonoBehaviour
{
    public static int money {get; set;}

    public static bool inShopRange { get; set; }

    public static bool skillToAdd { get; set; }

    public static int mana { get; set; }

    public static Dictionary<SkillObj, Button> skillDictionary = new Dictionary<SkillObj, Button>();
}
