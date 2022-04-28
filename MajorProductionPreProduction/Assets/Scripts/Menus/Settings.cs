using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public AudioSource song;
    private GameObject settings;

    public float musicVol = 0.1f;

    public static float globalMusicVol { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        //globalMusicVol = musicVol;
        DebugEx.Log(globalMusicVol);
        song.volume = globalMusicVol;
        if(GameObject.Find("Settings") != null)
        {
            settings = GameObject.Find("Settings");
            settings.SetActive(false);

        }
    }

    private void Update()
    {
        if(song.volume != globalMusicVol)
        {
            song.volume = globalMusicVol;
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

    public void decreaseMusicVol()
    {
        globalMusicVol = globalMusicVol - .1f;
        globalMusicVol = Mathf.Round(globalMusicVol * 10.0f) * 0.1f;//rounds to the nearest .x
        if (globalMusicVol < .1f)
        {
            globalMusicVol = 0;
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
