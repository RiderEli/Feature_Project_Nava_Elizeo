using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* [Nava, Elizeo]
 * [December 7, 2023]
 * [This is the code for the Color of the Skill. This is mostly used for the Ultimate Ability/Elemental Burst. More Clarification can be found in the KokomiController script.]
 */
public class JellyfishColor : MonoBehaviour
{
    public int jellyMatNum;
    public Material[] jellyMat;
    private Renderer jellyRend;
    void Start()
    {
        FishMatPrep();
    }
    void Update()
    {
        jellyRend.sharedMaterial = jellyMat[jellyMatNum];
        if (KokomiController.instance.ultimateActive == true)
        {
            jellyMatNum = 1;
        }
        else
        {
            jellyMatNum = 0;
        }
    }
    public void FishMatPrep()
    {
        jellyMatNum = 0;
        jellyRend = GetComponent<Renderer>();
        jellyRend.enabled = true;
        jellyRend.sharedMaterial = jellyMat[jellyMatNum];
    }
}
