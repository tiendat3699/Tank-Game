using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    private Slider healthSlider;
    [SerializeField]
    private Image fill;
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
    }

    private void UpdateHealthValue(float hp) {
        healthSlider.value = hp;
    }

    private void HandlechangeColor() {
        if(initialized) {
            if(healthSlider.value < healthSlider.maxValue/2) {
                fill.color = Color.yellow;
            }
            
            if(healthSlider.value < healthSlider.maxValue/3) {
                fill.color = Color.red;
            }
        }

        if(healthSlider.value <= healthSlider.minValue) {
            fill.enabled = false;
        } else {
            fill.enabled = true;
        }
    } 
}
