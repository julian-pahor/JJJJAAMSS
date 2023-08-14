using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    private Vector3 standard;
    private float lerp;

    public List<Cell> neighbours = new List<Cell>();
    // Start is called before the first frame update
    void Start()
    {
        standard = transform.position;
        lerp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(standard, standard + Vector3.up * 3f, lerp);
        lerp -= Time.deltaTime;
        lerp = Mathf.Clamp01(lerp);
    }

    public void AddNeighbour(Cell c)
    {
        neighbours.Add(c);
    }

    public void Jump(float f)
    {
        lerp = f;
    }

    public void OnMouseDown()
    {
        foreach (Cell c in neighbours)
        {
            if (c != this)
            {
                c.Jump(0.675f);
            }

            foreach (Cell b in c.neighbours)
            {
                b.Jump(0.3f);
            }
        }

        Jump(1f);
    }



}