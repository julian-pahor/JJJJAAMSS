using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSpawner : MonoBehaviour
{
    public int row;
    public int column;

    public Cell cell;

    public float cellWidth;
    public float cellHeight;

    public Cell[,] cells;

    // Start is called before the first frame update
    void Start()
    {
        cells = new Cell[row, column];
        SpawnCells();

    }

    void SpawnCells()
    {
        for (int i = 0; i < row; i++)
        {
            for (int y = 0; y < column; y++)
            {
                Cell go;
                go = Instantiate(cell, new Vector3(transform.position.x + (i * cellWidth), transform.position.y, transform.position.z + (y * cellHeight)), Quaternion.identity, this.transform);
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
                    cells[i, y].AddNeighbour(cells[i + 1, y]);
                }
                if (i > 0)
                {
                    cells[i, y].AddNeighbour(cells[i - 1, y]);
                }

                if (y < column - 1)
                {
                    cells[i, y].AddNeighbour(cells[i, y + 1]);
                }
                if (y > 0)
                {
                    cells[i, y].AddNeighbour(cells[i, y - 1]);
                }
            }
        }
    }


}