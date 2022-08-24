using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    private GameObject _player;
    // Start is called before the first frame update
    private void Awake() {
    }
    void Start()
    {
        _player = GameManager.Instance.getPlayer();
        _player.GetComponent<Player>().onTakeDamage.AddListener(ActiveDamageIndicator);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy() {
        StopAllCoroutines();
    }

    private void ActiveDamageIndicator(float damage) {
        gameObject.SetActive(true);
        StartCoroutine(DamageIndicatoyYield());
    }

    private IEnumerator DamageIndicatoyYield() {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
    
}
