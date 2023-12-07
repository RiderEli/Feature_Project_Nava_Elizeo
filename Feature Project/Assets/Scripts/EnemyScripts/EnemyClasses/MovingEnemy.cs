using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* [Nava, Elizeo]
 * [December 7, 2023]
 * [This is an inheritance script of the Moving Enemy]
 */
public class MovingEnemy : Enemy
{

    public override void Update()
    {
        EnemyMove();
    }
}
