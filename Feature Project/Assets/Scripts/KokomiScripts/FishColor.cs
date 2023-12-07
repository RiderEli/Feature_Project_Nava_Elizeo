using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* [Nava, Elizeo]
 * [December 7, 2023]
 * [This is the code for the Color of the Skill. This is mostly used for the Ultimate Ability/Elemental Burst. More Clarification can be found in the KokomiController script.]
 */
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
