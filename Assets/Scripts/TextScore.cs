using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextScore : MonoBehaviour
{
    private Text scoreText;
    // Start is called before the first frame update
    private void Awake() {
        scoreText = GetComponent<Text>();
        GameManager.Instance.onUpdateScore.AddListener(UpdateScore);
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateScore(int score) {
        scoreText.text = "Score:" + score;
    }
}
