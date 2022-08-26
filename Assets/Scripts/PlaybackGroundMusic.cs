using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackGroundMusic : MonoBehaviour
{
    private AudioSource audioSource;
    private void Awake() {
        GameManager.Instance.onStartGame.AddListener(StartPlayMusic);
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartPlayMusic() {
        audioSource.Play();
    }
}
