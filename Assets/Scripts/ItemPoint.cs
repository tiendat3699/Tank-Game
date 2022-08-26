using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SetItemPoint(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
