using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountText : MonoBehaviour
{
    private Text countText;
    // Start is called before the first frame update
    private void Awake() {
        GameManager.Instance.onCounttime.AddListener(UpdateCountTime);
        countText = GetComponent<Text>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateCountTime(int time) {
        gameObject.SetActive(time >= 0);
        if(time > 0) {
            SoundManager.Instance.playSound(Sounds.StartSound01);
        }
        countText.text = time.ToString();
        if(time == 0) {
            countText.text = "GO!";
            SoundManager.Instance.playSound(Sounds.StartSound02);
        }
    }

}
