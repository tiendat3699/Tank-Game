using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{

    public Transform FollowTarget;
    
    private void Awake() {
        gameObject.SetActive(false);
        GameManager.Instance.onStartGame.AddListener(()=> gameObject.SetActive(true));
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if(Input.GetButton("Menu")) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
