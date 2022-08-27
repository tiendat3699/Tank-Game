using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemDoubleDamage : MonoBehaviour
{
    public float time;
    public Material swordMaterial;
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
            GameManager.Instance.getPlayer().GetComponent<Player>().doubleDamage(time, swordMaterial);
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        GameManager.Instance.ResetItem();
    }
}
