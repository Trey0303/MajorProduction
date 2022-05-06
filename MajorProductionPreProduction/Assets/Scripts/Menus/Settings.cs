using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public AudioSource song;
    public List<AudioSource> soundEffects = new List<AudioSource>();

    private GameObject settings;

    //public float musicVol = 0.1f;
    //public float sfx = 0.1f;
    private float lastSoundCheck;

    public static float globalMusicVol { get; set; }
    public static float globalSfxVol { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        //globalMusicVol = musicVol;
        //DebugEx.Log(globalMusicVol);
        if(song != null)//music
        {
            song.volume = globalMusicVol;

        }

        for (int i = 0; i < soundEffects.Count; i++)//sound effects
        {
            if(soundEffects[i] != null)
            {
                soundEffects[i].volume = globalSfxVol;

            }


        }

        if (GameObject.Find("Settings") != null)
        {
            settings = GameObject.Find("Settings");
            settings.SetActive(false);

        }
    }

    private void LateUpdate()
    {
        if(song != null)
        {
            if(song.volume != globalMusicVol)
            {
                song.volume = globalMusicVol;
            }

        }

        if (lastSoundCheck != globalSfxVol)
        {
            lastSoundCheck = globalSfxVol;
            for (int i = 0; i < soundEffects.Count; i++)
            {
                if(soundEffects[i] != null)
                {
                    soundEffects[i].volume = globalSfxVol;

                }
            }
        }


    }

    public void IncreaseMusicVol()
    {
        globalMusicVol = globalMusicVol + .1f;
        globalMusicVol = Mathf.Round(globalMusicVol * 10.0f) * 0.1f;//rounds to the nearest .x
        if (globalMusicVol > 1f)
        {
            globalMusicVol = 1;
        }
    }

    public void DecreaseMusicVol()
    {
        globalMusicVol = globalMusicVol - .1f;
        globalMusicVol = Mathf.Round(globalMusicVol * 10.0f) * 0.1f;//rounds to the nearest .x
        if (globalMusicVol < .1f)
        {
            globalMusicVol = 0;
        }
    }

    public void IncreaseSfxVol()
    {
        globalSfxVol = globalSfxVol + .1f;
        globalSfxVol = Mathf.Round(globalSfxVol * 10.0f) * 0.1f;//rounds to the nearest .x
        if (globalSfxVol > 1f)
        {
            globalSfxVol = 1;
        }
    }

    public void DecreaseSfxVol()
    {
        globalSfxVol = globalSfxVol - .1f;//decrease vol
        globalSfxVol = Mathf.Round(globalSfxVol * 10.0f) * 0.1f;//rounds to the nearest .x
        if (globalSfxVol < .1f)//if vol gets set to a number less than 0
        {
            globalSfxVol = 0;//make it 0
        }
    }

    public void OpenSettings()
    {
        settings.SetActive(true);
    }

    public void CloseSettings()
    {
        settings.SetActive(false);
    }
}