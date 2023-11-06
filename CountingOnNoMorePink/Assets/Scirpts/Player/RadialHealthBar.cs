using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialHealthBar : MonoBehaviour
{
    public FreeFormOrbitalMove player;
    public Transform canvas;
    public Transform rotTarget;
    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if(player == null)
            return;

        image.fillAmount = player.currentHP / player.maxHP;

        RotateTowardsTarget();
    }

    void RotateTowardsTarget()
    {
        if (rotTarget == null || canvas == null)
            return;

        Vector2 ourPosition = new Vector2(canvas.transform.position.x,canvas.transform.position.z);
        Vector2 theirPosition = new Vector2(rotTarget.transform.position.x,rotTarget.transform.position.z);

        Vector2 direction = theirPosition - ourPosition;

        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        canvas.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, angle));

    }
}
