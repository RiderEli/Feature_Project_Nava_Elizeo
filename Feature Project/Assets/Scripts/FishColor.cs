using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishColor : MonoBehaviour
{
    public int fishMatNum;
    public Material[] fishMat;
    private Renderer fishRend;
    void Start()
    {
        FishMatPrep();
    }
    void Update()
    {
        fishRend.sharedMaterial = fishMat[fishMatNum];
        if (KokomiController.instance.ultimateActive == true)
        {
            fishMatNum = 1;
        }
        else
        {
            fishMatNum = 0;
        }
    }
    public void FishMatPrep()
    {
        fishMatNum = 0;
        fishRend = GetComponent<Renderer>();
        fishRend.enabled = true;
        fishRend.sharedMaterial = fishMat[fishMatNum];
    }
}
