using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [System.Serializable]
    public class Edge
    {
        Edge() { target = null; cost = 0; }
        public Edge(Cell c, float f)
        {
            target = c;
            cost = f;
        }

        public Cell target;
        public float cost;
    }

    public List<Edge> connections = new List<Edge>();
    public float gScore;
    public float hSCore;
    public float fSCore;

    [HideInInspector]
    public Cell previous;

    public MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddConnection(Cell c, float f)
    {
        connections.Add(new Edge(c, f));
    }
}