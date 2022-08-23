using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextHealth : MonoBehaviour
{
    private Text healthText;
    // Start is called before the first frame update
    private void Awake() {
        healthText = GetComponent<Text>();
    }
    void Start()
    {
        GameManager.Instance.updateHealth.AddListener(UpdateHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateHealth(float hp) {
        healthText.text = "HP:" + hp;
    }
}
