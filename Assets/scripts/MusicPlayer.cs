
using UnityEngine;

public class MusicPlayer : MonoBehaviour{

    [SerializeField] private AudioSource warMusic, vistaMusic;


    void Start()
    {
        MusicHandler.OnPlayWarMusic += OnPlayWarMusic;
        vistaMusic.Play();
    }

    private void OnPlayWarMusic()
    {
        if (vistaMusic.isPlaying) vistaMusic.Stop();
        if (!warMusic.isPlaying) warMusic.Play();

        if (!IsInvoking("FadeInWarAudioGradually"))
        {
            if (IsInvoking("StopWarMusicDelayed")) CancelInvoke("StopWarMusicDelayed");
            if (IsInvoking("FadeOutWarAudioGradually")) CancelInvoke("FadeOutWarAudioGradually");
            InvokeRepeating("FadeInWarAudioGradually", 0, .5f);
        }
    }

    private void FadeInWarAudioGradually()
    {
        if(warMusic.volume < 1)
        {
            warMusic.volume += .05f;

        }
        else
        {
            warMusic.volume = 1;
            CancelInvoke("FadeInWarAudioGradually");
            Invoke("StopWarMusicDelayed", 5f);
        }
    }

    private void StopWarMusicDelayed()
    {
        if (warMusic.isPlaying)
        {
            InvokeRepeating("FadeOutWarAudioGradually", 0, .5f);
        }

    }

    private void FadeOutWarAudioGradually()
    {

        if (warMusic.volume > 0)
        {
            warMusic.volume -= .05f;
        }
        else
        {
            warMusic.Stop();
            CancelInvoke("FadeOutWarAudioGradually");
            if (!vistaMusic.isPlaying) vistaMusic.Play();
        }
    }

    private void OnDestroy()
    {
        MusicHandler.OnPlayWarMusic -= OnPlayWarMusic;
    }


}
