using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{

    public GameObject zone;

    public int shotsPerShot;
    public float firingArc;
    public float distance;
    public float firingAngle;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Fire(shotsPerShot, firingArc);
            firingAngle += 90;
        }
    }

    public void Fire(int shots, float arc)
    {
        if (shots <= 0) { shots = 1; } //stop it dividing by 0

        float angleStep = (arc / shots); //divide total arc of fire by number of shots





        //launch vector
        Vector3 launchvector = Utilities.PointWithPolarOffset(transform.position, 1, firingAngle) - transform.position;
        launchvector = launchvector.normalized;


        float rotation = (float)Mathf.Atan2(launchvector.x, launchvector.z) * 180 / Mathf.PI;


        if (rotation < 0) { rotation = 360 + rotation; } //fix negative degrees


        //instantiate at distance from origin

        for (var i = 0; i < shots; ++i)
        {
            
            float rotationO = rotation;
            rotationO = rotation - firingArc / 2 + angleStep / 2;
            rotationO += angleStep * i;


            Vector3 point = Utilities.PointWithPolarOffset(transform.position, distance, rotationO);

            Instantiate(zone,point, Quaternion.identity);


        }




    }
}
