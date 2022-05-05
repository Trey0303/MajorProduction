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
    public static float maxHealth { get; set; }

    //stamina
    public Slider staminabar;


    public static float maxStamina { get; set; }
    public float RegenBy;

    public static float RegenWaitTimer { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        curHealth = healthbar.value;
        maxHealth = healthbar.maxValue;

        PlayerControllerIsometric.stamina = staminabar.maxValue;
        maxStamina = staminabar.maxValue;
        staminabar.value = PlayerControllerIsometric.stamina;
    }

    // Update is called once per frame
    void Update()
    {
        if(healthbar != null)
        {
            //CURRENT HEALTH
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

            //GAME OVER
            if(healthbar.value <= 0)//if player dies
            {
                //play death animation

                //move to gameover scene
                DebugEx.Log("Player dead");
                SceneManager.LoadScene(gameoverScene);
            }

            //MAX HEALTH
            if (healthbar.maxValue != maxHealth)//if player max health increases/decreases
            {
                healthbar.maxValue = maxHealth;
                curHealth = healthbar.maxValue;
            }

        }

        //MAX STAMINA
        if (staminabar.maxValue != maxStamina)//if player max health increases/decreases
        {
            staminabar.maxValue = maxStamina;
            PlayerControllerIsometric.stamina = staminabar.maxValue;
            DebugEx.Log("MAX STAMINA CHANGE");
        }

        if (staminabar != null)
        {
            if (staminabar.value != PlayerControllerIsometric.stamina)//if there was a change in value for stamina
            {
                staminabar.value = PlayerControllerIsometric.stamina;
            }
        }

        if (RegenWaitTimer <= 0)
        {
            if (PlayerControllerIsometric.stamina < staminabar.maxValue)
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
