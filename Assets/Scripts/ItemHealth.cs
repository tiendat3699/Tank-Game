using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    public float healing;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            SoundManager.Instance.playSound(Sounds.healspell);
            float currentHealth = GameManager.Instance.getHealth();
            float initHealth = GameManager.Instance.getInitHealth();
            float newHealth = currentHealth + healing >= initHealth ? initHealth : currentHealth + healing;
            GameManager.Instance.getPlayer().GetComponent<Player>().healing(newHealth);
            GameManager.Instance.ResetItem();
            Destroy(gameObject);
        }
    }
}
