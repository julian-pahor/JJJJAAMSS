using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackEvent/SpreadShot")]
public class SpreadShot : AttackEvent
{
    
    public int shots;
    public float firingArc;
    public float arcOffset;
    public float bulletSpeed;
    public Bullet bulletType;
    public ShotType shotType;
    public enum ShotType { Standard, TargetPlayer}
    public override void Fire()
    {
        base.Fire();

        if (shots <= 0) { shots = 1; } //stop it dividing by 0
        float angleStep = (firingArc / shots); //divide total arc of fire by number of shots

        Transform origin = Wobbit.instance.bossOrigin;

       
        Vector3 launchvector = Utilities.PointWithPolarOffset(origin.position, 1, arcOffset) - origin.position;
        launchvector = launchvector.normalized;
 
        //override launchvector if targeting player
        if(shotType == ShotType.TargetPlayer)
        {
            launchvector = Wobbit.instance.player.position - origin.position;
       
            float angle = (float)Mathf.Atan2(launchvector.x, launchvector.z) * 180 / Mathf.PI;
            launchvector = Utilities.PointWithPolarOffset(origin.position, 1, angle) - origin.position;
            launchvector = launchvector.normalized;
      
        }


        float rotation = (float)Mathf.Atan2(launchvector.x, launchvector.z) * 180 / Mathf.PI;

        if (rotation < 0) { rotation = 360 + rotation; } //fixes negative degrees

        for (var i = 0; i < shots; ++i)
        {
            Bullet b;
            //TODO: grab this from pool instead of instantiating
            if (bulletType == null)
            {
                 b = Instantiate(Wobbit.instance.bulletFab, origin.position, Quaternion.identity);
                   
            }
            else
            {
                b = Instantiate(bulletType, origin.position, Quaternion.identity);
            }

            b.speed = bulletSpeed > 0f ? bulletSpeed : 1;

            float rotationO = rotation;
            rotationO = rotation - firingArc / 2 + angleStep / 2;
            rotationO += angleStep * i;


            Vector3 bulletDir = Utilities.PointWithPolarOffset(origin.position, 1f, rotationO);


            b.Initialise(bulletDir);
        }
    }

    public override void HookUp(EventEditor ee)
    {
        
        //Set Up Shots 
        ValueEditor ve;
        ve = ee.CreateEditor();
        ve.SetListener((float f) => { shots = (int)f; }, shots, "Shots", 1, 360, true);

        //set up bullet speed
        ve = ee.CreateEditor();
        ve.SetListener((float f) => { bulletSpeed = (int)f; }, bulletSpeed, "Speed", 1, 100);



        //set Up Firing Arc
        ve = ee.CreateEditor();
        ve.SetListener((float f) => { firingArc = f; }, firingArc, "Firing Arc", 0, 360);


        //Set Up Arc Offset
        ve = ee.CreateEditor();
        ve.SetListener((float f) => { arcOffset = f; }, arcOffset, "Arc Offset", 0, 360);

    }
}
