using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    private Slider healthSlider;
    [SerializeField]
    private Image fill;
    [SerializeField]
    private Text text;
    private bool initialized = false;
    // Start is called before the first frame update
    private void Awake() {
        healthSlider = GetComponent<Slider>();
        GameManager.Instance.onUpdateHealth.AddListener(UpdateHealthValue);
    }
    void Start()
    {
        healthSlider.maxValue =  GameManager.Instance.getHealth();
        healthSlider.value =  GameManager.Instance.getHealth();
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        HandlechangeColor();

        text.text = healthSlider.value +"/"+healthSlider.maxValue;
    }

    private void UpdateHealthValue(float hp) {
        healthSlider.value = hp;
    }

    private void HandlechangeColor() {
        if(initialized) {
            if(healthSlider.value <= healthSlider.maxValue/4) {
                fill.color = Color.red;
            } else if(healthSlider.value <= healthSlider.maxValue/1.5f) {
                fill.color = Color.yellow;
            } else {
                fill.color = new Color32(33,197,10,255);
            }
        }

        if(healthSlider.value <= healthSlider.minValue) {
            fill.enabled = false;
        } else {
            fill.enabled = true;
        }
    } 
}
