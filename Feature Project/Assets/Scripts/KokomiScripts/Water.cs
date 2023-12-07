using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* [Nava, Elizeo]
 * [December 7, 2023]
 * [This is the code for the Water. This is mostly used for the Ultimate Ability/Elemental Burst.]
 */
public class Water : MonoBehaviour
{
    private Collider waterToLand;
    void Start()
    {
        //This for the Water's effect with or without Ult
        waterToLand = GetComponent<Collider>();
        waterToLand.enabled = false;
    }

    void Update()
    {
        //This will allow Kokomi to walk on water if she activates her Ultimate/Elemental Burst.
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
