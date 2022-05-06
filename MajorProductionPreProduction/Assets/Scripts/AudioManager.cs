using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject audioManagerPrefab;

    public List<AudioClip> soundEffects = new List<AudioClip>();

    public static List<AudioClip> Sfx { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(audioManagerPrefab);
        
        //for(int i = 0; i < soundEffects.Count; i++)
        //{
        //    if(soundEffects[i] != null)
        //    {
        //        Sfx.Add(soundEffects[i]);
        //        DebugEx.Log(soundEffects[i].name);
        //    }

        //}
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
