using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(WebAudioManager))]
public class GlobeSceneManager : MonoBehaviour
{
    public float FaceAwayTolerance;

    private WebAudioManager WebAudioManager;
    public float CutoffMax = 2f;
    public float CutoffMin = 0.4f;
    //public float NormalCutoff = 5000f;
    //public float FaceAwayCutoff = 180f;

    private Transform sceneParent;
    private bool sceneChanged;

    private Transform activeScene;

    // Use this for initialization
    void Start()
    {
        sceneParent = transform.FindChild("Scenes");

        // setup web audio manager
        WebAudioManager = GetComponent<WebAudioManager>();

        activeScene = getActiveScene();
        if (!activeScene)
            setRandomActiveScene();
        else
        {
            WebAudioManager.SetScene(activeScene);
        }


    }

    // Update is called once per frame
    void Update()
    {
        // get camera forward
        Vector3 camForward = Camera.main.transform.forward;

        // get delta between forward and up
        Vector3 delta = camForward - transform.up;
        float dMagnitude = Vector3.Magnitude(delta);

        // change scene if magnitude is <= tolerance
        if (!sceneChanged && dMagnitude <= FaceAwayTolerance)
        {
            // change scene
            Debug.Log("Change scene!");
            setRandomActiveScene();
            sceneChanged = true;
        }
        else if (sceneChanged && dMagnitude > FaceAwayTolerance) // reset flag if no longer facing away from camera
            sceneChanged = false;

        // update audio
        updateAudio(dMagnitude);
    }

    private void updateAudio(float dMag)
    {
        float vol = Mathf.Lerp(WebAudioManager.MasterVolumeMin, WebAudioManager.MasterVolumeMax, (dMag - CutoffMin) / (CutoffMax - CutoffMin));
        WebAudioManager.SetMasterVolume(vol);
        //float cutoff = Mathf.Lerp(FaceAwayCutoff, NormalCutoff, (dMag - CutoffMin) / (CutoffMax - CutoffMin));
        //Mixer.SetFloat("Scene_Lowpass", cutoff);
    }

    /// <summary>
    /// Returns a random child of the Scene transform. Does not return the active scene.
    /// </summary>
    /// <returns></returns>
    private Transform getRandomScene()
    {
        // get a random scene number
        int sceneNumber = Random.Range(0, sceneParent.childCount - 1);
        Transform scene = sceneParent.GetChild(sceneNumber);

        // return the scene if it isn't the active scene
        if (activeScene && scene != activeScene)
            return scene;

        // return the scene after the active scene if the active scene is not the last scene
        if (sceneNumber < sceneParent.childCount - 1)
            return sceneParent.GetChild(sceneNumber + 1);

        // return the scene before the active scene
        return sceneParent.GetChild(sceneNumber - 1);
    }

    /// <summary>
    /// Returns the active scene, or null if there is none
    /// </summary>
    /// <returns></returns>
    private Transform getActiveScene()
    {
        for (int i = 0; i < sceneParent.childCount; i++)
        {
            if (sceneParent.GetChild(i).gameObject.activeInHierarchy)
                return sceneParent.GetChild(i);
        }

        return null;
    }

    /// <summary>
    /// Sets a new active scene
    /// </summary>
    private void setRandomActiveScene()
    {
        Transform nextScene = getRandomScene();

        if (activeScene)
            activeScene.gameObject.SetActive(false);

        nextScene.gameObject.SetActive(true);

        activeScene = nextScene;

        WebAudioManager.SetScene(activeScene);
    }
}
