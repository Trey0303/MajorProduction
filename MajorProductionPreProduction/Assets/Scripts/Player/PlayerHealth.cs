using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private GameObject player;

    [Header("Sound Effects")]
    public AudioSource deathAudio;

    //health
    public Slider healthbar;
    public int gameoverScene;
    public static float curHealth { get; set; }
    public static float maxHealth { get; set; }

    //stamina
    public Slider staminabar;


    public static float maxStamina { get; set; }
    public float RegenBy;
    private bool deathAudioPlayed;

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
                else if (curHealth > healthbar.maxValue)
                {
                    curHealth = healthbar.maxValue;
                    healthbar.value = curHealth;
                }
                else
                {
                    curHealth = healthbar.value;
                }
            }

            //GAME OVER
            if(healthbar.value <= 0)//if player dies
            {
                if (!deathAudioPlayed)
                {
                    Time.timeScale = 0;
                    deathAudio.Play();
                    deathAudioPlayed = true;
                }
                else if(!deathAudio.isPlaying)
                {
                    ////move to gameover scene
                    DebugEx.Log("Player dead");
                    SceneManager.LoadScene(gameoverScene);

                }

                //play death animation

                //StartCoroutine(Death());
                //yield return new WaitWhile(() => deathAudio.isPlaying);

                //while (deathAudio.isPlaying)
                //{
                //    Time.timeScale = 0;
                //    yield return null;
                //}

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
                //staminabar.value = PlayerControllerIsometric.stamina;

                if (curHealth <= healthbar.maxValue)//limits healing to no more than max health
                {
                    //healthbar.value = curHealth;//update health ui

                    staminabar.value = PlayerControllerIsometric.stamina;

                }
                else if (curHealth > healthbar.maxValue)
                {
                    //curHealth = healthbar.maxValue;
                    //healthbar.value = curHealth;

                    PlayerControllerIsometric.stamina = staminabar.maxValue;
                    staminabar.value = PlayerControllerIsometric.stamina;
                }
                else
                {
                    //curHealth = healthbar.value;

                    PlayerControllerIsometric.stamina = staminabar.value;
                }
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

    //IEnumerator Death()
    //{
    //    //deathAudio.Play();

    //    //play death animation

    //    //StartCoroutine(Death());
    //    while (deathAudio.isPlaying)
    //    {
    //        Time.timeScale = 0;
    //        yield return null;
    //    }
    //    //yield return new WaitWhile(() => deathAudio.isPlaying);
    //    //move to gameover scene
    //    DebugEx.Log("Player dead");
    //    SceneManager.LoadScene(gameoverScene);
    //}
}
