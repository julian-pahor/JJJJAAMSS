using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackEvent/SpreadShot")]
public class SpreadShot : AttackEvent
{
    
    public int shots;
    public float firingArc;
    public float arcOffset;
    public Bullit bulletType;
    public override void Fire()
    {

        if (shots <= 0) { shots = 1; } //stop it dividing by 0
        float angleStep = (firingArc / shots); //divide total arc of fire by number of shots

        Transform origin = Wobbit.instance.bossOrigin;

        Vector3 launchvector = Utilities.PointWithPolarOffset(origin.position, 1, arcOffset) - origin.position;
        launchvector = launchvector.normalized;


        float rotation = (float)Mathf.Atan2(launchvector.x, launchvector.z) * 180 / Mathf.PI;

        if (rotation < 0) { rotation = 360 + rotation; } //fixes negative degrees

        for (var i = 0; i < shots; ++i)
        {
            Bullit b;
            //TODO: grab this from pool instead of instantiating
            if (bulletType == null)
            {
                 b = Instantiate(Wobbit.instance.bulletFab, origin.position, Quaternion.identity);
            }
            else
            {
                b = Instantiate(bulletType, origin.position, Quaternion.identity);
            }
            float rotationO = rotation;
            rotationO = rotation - firingArc / 2 + angleStep / 2;
            rotationO += angleStep * i;


            Vector3 bulletDir = Utilities.PointWithPolarOffset(origin.position, 1f, rotationO);


            b.Initialise(bulletDir);
            


        }


    }

}
