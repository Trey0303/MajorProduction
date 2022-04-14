using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCount : MonoBehaviour
{
    public Text killcount;
    private int curUINum;

    // Start is called before the first frame update
    void Start()
    {
        killcount.text = "" + PlayerControllerIsometric.killcount;
        curUINum = PlayerControllerIsometric.killcount;
    }

    // Update is called once per frame
    void Update()
    {
        if(curUINum != PlayerControllerIsometric.killcount)
        {
            curUINum = PlayerControllerIsometric.killcount;
            killcount.text = "" + PlayerControllerIsometric.killcount;
        }
    }
}
