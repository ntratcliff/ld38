using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebAudioManager : MonoBehaviour
{
    public float MasterVolume = 1f;
    public float MasterVolumeMax = 1f;
    public float MasterVolumeMin = 0.1f;
    public float MusicVolume = 0.8f;
    public float AmbientVolume = 1f;

    public float FadeInTime = 0.2f;
    private float elapsedFadeTime;
    private float fadeVolume;

    public AudioSource MusicSource;
    public AudioSource AmbientSource;

    private float initMusicVol, initAmbientVol;

    public void SetMasterVolume(float volume)
    {
        MasterVolume = volume;

        updateFade();

        if (MusicSource)
            updateSource(MusicSource, initMusicVol, MusicVolume);

        if (AmbientSource)
            updateSource(AmbientSource, initAmbientVol, AmbientVolume);
    }

    private void updateFade()
    {
        if (elapsedFadeTime > FadeInTime)
            return;

        fadeVolume = Mathf.Lerp(0f, 1f, elapsedFadeTime / FadeInTime);

        elapsedFadeTime += Time.deltaTime;
    }

    private void updateSource(AudioSource source, float initVol, float volume)
    {
        // set source volume relative to master and volume float
        source.volume = initVol * volume * fadeVolume * MasterVolume;
    }

    public void SetScene(Transform scene)
    {
        // get MusicSource
        AudioSource musicSource = scene.FindChild("MusicSource").GetComponent<AudioSource>();

        // get AmbientSource
        AudioSource ambientSource = scene.FindChild("AmbientSource").GetComponent<AudioSource>();

        // set sources
        SetSources(musicSource, ambientSource);
    }

    private void resetCurrentSources()
    {
        if (MusicSource)
            MusicSource.volume = initMusicVol;

        if (AmbientSource)
            AmbientSource.volume = initAmbientVol;
    }

    public void SetSources(AudioSource musicSource, AudioSource ambientSource)
    {
        resetCurrentSources();

        MusicSource = musicSource;
        if (MusicSource)
        {
            // get initial volume
            initMusicVol = MusicSource.volume;
        }

        AmbientSource = ambientSource;
        if (AmbientSource)
        {
            // get initial volume
            initAmbientVol = AmbientSource.volume;
        }

        // start fade
        elapsedFadeTime = 0f;
    }
}
