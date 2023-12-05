using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private Collider waterToLand;
    void Start()
    {
        waterToLand = GetComponent<Collider>();
        waterToLand.enabled = false;
    }

    void Update()
    {
        if (KokomiController.instance.ultimateActive)
        {
            waterToLand.enabled = true;
        }
        else
        {
            waterToLand.enabled = false;
        }
    }
}
