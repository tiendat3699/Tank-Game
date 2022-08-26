using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private Text gameOverText;
    private void Awake() {
        GameManager.Instance.onGameover.AddListener(upDateGameOver);
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        gameOverText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void upDateGameOver(bool win) {
        gameObject.SetActive(true);
        if(win) {
            gameOverText.text = "You Win!!";
        } else {
            gameOverText.text = "You Die!!";
        }
        StartCoroutine(LoadGameOverScene());
    }

    IEnumerator LoadGameOverScene() {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("tankScene/GameOverScene");
    }
}
