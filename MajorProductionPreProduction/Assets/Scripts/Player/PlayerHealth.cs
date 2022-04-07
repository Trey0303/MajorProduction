using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private GameObject player;

    //health
    public Slider healthbar;
    public int gameoverScene;
    public static float curHealth { get; set; }

    //stamina
    public Slider staminabar;


    public float maxStamina = 100;
    public float RegenBy;

    public static float RegenWaitTimer { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        curHealth = healthbar.value;

        PlayerControllerIsometric.stamina = maxStamina;
        staminabar.maxValue = PlayerControllerIsometric.stamina;
        staminabar.value = PlayerControllerIsometric.stamina;
    }

    // Update is called once per frame
    void Update()
    {
        if(healthbar != null)
        {
            if(healthbar.value != curHealth)//if player loss or gained health
            {
                if(curHealth <= healthbar.maxValue)//limits healing to no more than max health
                {
                    healthbar.value = curHealth;//update health ui

                }
                else
                {
                    curHealth = healthbar.value;
                }
            }

            if(healthbar.value <= 0)//if player dies
            {
                //play death animation

                //move to gameover scene
                SceneManager.LoadScene(gameoverScene);
            }
        }

        if (staminabar != null)
        {
            if (staminabar.value != PlayerControllerIsometric.stamina)//if there was a change in value for stamina
            {
                staminabar.value = PlayerControllerIsometric.stamina;
            }
        }

        if (PlayerControllerIsometric.stamina < maxStamina)
        {
            if (RegenWaitTimer <= 0)
            {
                if(PlayerControllerIsometric.stamina  < staminabar.maxValue)
                {
                    PlayerControllerIsometric.stamina = PlayerControllerIsometric.stamina + RegenBy * Time.deltaTime;

                }

            }
            else
            {
                RegenWaitTimer = RegenWaitTimer - Time.deltaTime;
            }
        }
    }
}
