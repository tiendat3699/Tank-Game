using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Sounds {
    shootSound,
    hitSound,
    explosionSound,
    engineIdle,
    TireScreeching,
    engineRunning,
    Notification,
    StartSound01,
    StartSound02,
    healspell
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {get; private set;}
    private AudioSource audioFx;

    // Start is called before the first frame update
    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }

    void Start()
    {
        audioFx = GetComponent<AudioSource>();
        audioFx.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void playSound(Sounds sound) {
        AudioClip audio = Resources.Load<AudioClip>($"Sounds/{sound.ToString()}");
        audioFx.PlayOneShot(audio);
    }

    public AudioClip getAudioInResources(Sounds sound) {
        return Resources.Load<AudioClip>($"Sounds/{sound.ToString()}");
    }

    public AudioSource add3DSound(GameObject gameObj, bool multip = false) {
        AudioSource audioComp = null;
        foreach(Transform child in gameObj.transform) {
            if(child.tag == "Audio3D") {
                audioComp = child.GetComponent<AudioSource>();
                break;
            }
        }
        if(audioComp == null || multip) {
            GameObject audioResource = Resources.Load<GameObject>("GameObjects/AudioSource3D");
            GameObject audioCompClone = Instantiate(audioResource,gameObj.transform.position, gameObj.transform.rotation);
            audioCompClone.transform.SetParent(gameObj.transform);
            audioComp = audioCompClone.GetComponent<AudioSource>();
        }

        return audioComp;

        // AudioClip audio = Resources.Load<AudioClip>($"Sounds/{sound.ToString()}");
        // audioComp.clip = audio;
    }

    public void play3DSound(GameObject gameObj, Sounds sound) {
        AudioSource audioComp = add3DSound(gameObj);
        AudioClip audio = getAudioInResources(sound);
        audioComp.PlayOneShot(audio);
    }

}
