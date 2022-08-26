using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultStatistics : MonoBehaviour
{
    public Text WinText;
    public Text scoreText;
    public Text detroyedText;
    public Button restartButton;
    public Button backButton;

    // Start is called before the first frame update
    private void Awake() {
        restartButton.onClick.AddListener(()=>{
            SoundManager.Instance.playSound(Sounds.Notification);
            StartCoroutine(delayLoadScene("tankScene/tankScene"));
        });

        backButton.onClick.AddListener(()=>{
            SoundManager.Instance.playSound(Sounds.Notification);
            StartCoroutine(delayLoadScene("tankScene/StartScene"));
        });
    }
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        GameManager gameManager = GameManager.Instance;
        
        bool isWin = GameManager.Instance.IsWin();
        WinText.text = isWin?"You Win!!":"You Lose!!";
        WinText.color = isWin? new Color32(1, 84, 7, 255): Color.red;
        scoreText.text = "Score: " + gameManager.getScore();
        detroyedText.text = "Detroyed: " + gameManager.GetEnemiesDetroyed()+"/"+gameManager.maxEnemiesAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator delayLoadScene(string path) {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(path);
        GameManager.Instance.ResetGame();
    }
}
