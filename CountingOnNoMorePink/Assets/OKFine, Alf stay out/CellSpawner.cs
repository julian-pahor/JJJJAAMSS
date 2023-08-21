using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSpawner : MonoBehaviour
{
    public static CellSpawner instance;

    public int row;
    public int column;

    public Cell cell;

    public float cellWidth;
    public float cellHeight;

    public Cell[,] cells;

    public Cell startCell;
    public Cell endCell;

    public Material startM, pathM, endM, defaultM;

    // Start is called before the first frame update
    void Start()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        cells = new Cell[row, column];
        SpawnCells();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && startCell != null && endCell != null)
        {
            AStarSearch(startCell, endCell);
        }
    }

    void SpawnCells()
    {
        for (int i = 0; i < row; i++)
        {
            for (int y = 0; y < column; y++)
            {
                Cell go;
                go = Instantiate(cell, new Vector3(i * cellWidth, 0, y * cellHeight), Quaternion.identity);
                go.name = $"X: {i}, Y: {y}";
                cells[i, y] = go;
            }
        }

        for (int i = 0; i < row; i++)
        {
            for (int y = 0; y < column; y++)
            {
                if (i < row - 1)
                {
                    cells[i, y].AddConnection(cells[i + 1, y], 1);
                }
                if (i > 0)
                {
                    cells[i, y].AddConnection(cells[i - 1, y], 1);
                }

                if (y < column - 1)
                {
                    cells[i, y].AddConnection(cells[i, y + 1], 1);
                }
                if (y > 0)
                {
                    cells[i, y].AddConnection(cells[i, y - 1], 1);
                }
            }
        }
    }

    float Heuristic(Cell c1, Cell c2)
    {
        return Vector3.Distance(c1.transform.position, c2.transform.position);
    }

    public List<Cell> AStarSearch(Cell startCell, Cell endCell)
    {
        List<Cell> path = new List<Cell>();

        if (startCell == endCell)
        {
            return path;
        }

        if (startCell == null || endCell == null)
        {
            Debug.LogError("Either start/end cell was null");
        }

        startCell.gScore = 0;

        startCell.previous = null;

        List<Cell> openList = new List<Cell>();
        List<Cell> closedList = new List<Cell>();

        openList.Add(startCell);

        Cell currentCell = null;

        while (openList.Count != 0)
        {
            openList.Sort(delegate (Cell c1, Cell c2) { return c1.fSCore.CompareTo(c2.fSCore); });

            currentCell = openList[0];

            if (currentCell == endCell)
            {
                break;
            }

            openList.Remove(currentCell);
            closedList.Insert(0, currentCell);

            foreach (Cell.Edge e in currentCell.connections)
            {
                if (!closedList.Contains(e.target))
                {
                    float gScore = currentCell.gScore + e.cost;
                    float hSCore = Heuristic(e.target, endCell);
                    float fScore = gScore + hSCore;

                    if (!openList.Contains(e.target))
                    {
                        e.target.gScore = gScore;
                        e.target.fSCore = fScore;
                        e.target.previous = currentCell;
                        openList.Insert(0, e.target);
                    }
                    else if (fScore < e.target.fSCore)
                    {
                        e.target.gScore = gScore;
                        e.target.fSCore = fScore;
                        e.target.previous = currentCell;
                    }
                }
            }
        }

        currentCell = endCell;

        while (currentCell != null)
        {
            path.Add(currentCell);
            currentCell = currentCell.previous;
        }

        path.Reverse();

        ColorPath(path);

        return path;
    }

    void ColorPath(List<Cell> path)
    {

        for (int i = 0; i < row; i++)
        {
            for (int y = 0; y < column; y++)
            {
                cells[i, y].mr.material = defaultM;
            }
        }

        for (int i = 0; i < path.Count; i++)
        {
            if (i == 0)
            {
                path[i].mr.material = startM;
            }
            else if (i == path.Count - 1)
            {
                path[i].mr.material = endM;
            }
            else
            {
                path[i].mr.material = pathM;
            }
        }
    }
}