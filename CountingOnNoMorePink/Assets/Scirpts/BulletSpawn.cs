using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
    public Bullit bullet;
    public int shotsPerShot;
    public float firingArc;
    public float firingAngle;

    private void Update()
    {
        //Vector2 gunrot = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //float angle = Mathf.Atan2(gunrot.x, gunrot.y) * Mathf.Rad2Deg;
        //Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        //t.rotation = rot;


        if (Input.GetMouseButtonDown(1))
        {
            Shoot(shotsPerShot, firingArc);
            firingAngle -= 45;
        }

            if (Input.GetMouseButtonDown(0))
        {
            Shoot(shotsPerShot, firingArc);
            firingAngle += 45;
        }
    }





    public void Shoot(int shots, float arc)
    {
        if (shots <= 0) { shots = 1; } //stop it dividing by 0




        float angleStep = (arc / shots); //divide total arc of fire by number of shots





        //launch vector
        Vector3 launchvector = Utilities.PointWithPolarOffset(transform.position,1,firingAngle) - transform.position;
        launchvector = launchvector.normalized;







        float rotation = (float)Mathf.Atan2(launchvector.x, launchvector.z) * 180 / Mathf.PI;




        if (rotation < 0) { rotation = 360 + rotation; } //fix negative degrees




        for (var i = 0; i < shots; ++i)
        {
            Bullit b = Instantiate(bullet, transform.position, Quaternion.identity);
            float rotationO = rotation;
            rotationO = rotation - firingArc / 2 + angleStep / 2;
            rotationO += angleStep * i;


            Vector3 bulletDir = Utilities.PointWithPolarOffset(transform.position, 1f, rotationO);


            b.Initialise(bulletDir);


        }




    }







 





}
