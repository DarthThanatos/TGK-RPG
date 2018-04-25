using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour {

    public delegate void MusicEvent();
    public static event MusicEvent OnPlayWarMusic;

    public static void PlayWarMusic()
    {
        if(OnPlayWarMusic != null)
        {
            OnPlayWarMusic();
        }
    }

}
