using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShield : MonoBehaviour
{
    public float time;
    public Material shieldMaterial;
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
            GameManager.Instance.getPlayer().GetComponent<Player>().shield(time, shieldMaterial);
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        GameManager.Instance.ResetItem();
    }
}
