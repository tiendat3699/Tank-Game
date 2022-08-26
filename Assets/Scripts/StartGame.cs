using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake() {
        GetComponent<Button>().onClick.AddListener(OnPlaygame);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPlaygame() {
        SoundManager.Instance.playSound(Sounds.Notification);
        StartCoroutine(StartTankGame());
    }

    IEnumerator StartTankGame() {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("tankScene/tankScene");
        GameManager.Instance.ResetGame();
    }
}
