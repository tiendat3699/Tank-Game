using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWalkPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SetWalkPoint(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
