using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* [Nava, Elizeo]
 * [December 7, 2023]
 * [This is an inheritance script of the Gun Enemy]
 */
public class GunEnemy : Enemy
{
    public override void Update()
    {
        EnemyShoot();
    }
}
