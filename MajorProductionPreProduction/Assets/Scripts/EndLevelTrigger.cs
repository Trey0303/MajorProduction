using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelTrigger : MonoBehaviour
{
    public int selectScene;
    public int targetKills;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(PlayerControllerIsometric.killcount >= targetKills)
            {
                SceneManager.LoadScene(selectScene);

            }
        }
    }
}