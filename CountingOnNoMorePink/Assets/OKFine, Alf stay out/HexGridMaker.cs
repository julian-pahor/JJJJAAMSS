using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridMaker : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2Int gridSize;

    [Header("Tile Settings")]
    public float outerSize = 1f;
    public float innerSize = 0f;
    public float height = 1f;
    public bool isFlatTopped;
    public Material material;

    public List<HexRenderer> hexes = new List<HexRenderer>();

    private void OnEnable()
    {
        LayoutGrid();
    }

    private void OnValidate()
    {
        if(Application.isPlaying)
        {
            LayoutGrid();
        }
    }

    void LayoutGrid()
    {
        //First iteration creates game objects
        if(hexes.Count <= 0)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    GameObject tile = new GameObject($"Hex {x},{y}", typeof(HexRenderer));
                    tile.transform.position = GetPositionForHexFromCoordinate(new Vector2Int(x, y));

                    HexRenderer hex = tile.GetComponent<HexRenderer>();
                    hexes.Add(hex);

                    hex.isFlatTopped = isFlatTopped;
                    hex.outerSize = outerSize;
                    hex.innerSize = innerSize;
                    hex.height = height;
                    hex.gridPos = new Vector2Int(x, y);
                    hex.SetMaterial(material);
                    hex.DrawMesh();

                    tile.transform.parent = transform;
                }
            }
        }
        else
        {
            foreach (HexRenderer hex in hexes)
            {
                hex.transform.position = GetPositionForHexFromCoordinate(hex.gridPos);
                hex.isFlatTopped = isFlatTopped;
                hex.outerSize = outerSize;
                hex.innerSize = innerSize;
                hex.height = height;
                hex.DrawMesh();
            }
        }
        
    }

    public Vector3 GetPositionForHexFromCoordinate(Vector2Int coordinate)
    {
        int column = coordinate.x;
        int row = coordinate.y;

        float width;
        float height;
        float xPosition;
        float yPosition;
        bool shouldOffset;
        float horizontalDistance;
        float verticalDistance;
        float offset;
        float size = outerSize;

        if(!isFlatTopped)
        {
            shouldOffset = (row % 2) == 0;
            width = Mathf.Sqrt(3) * size;
            height = 2f * size;

            horizontalDistance = width;
            verticalDistance = height * (3f / 4f);

            offset = (shouldOffset) ? width / 2 : 0;

            xPosition = (column * (horizontalDistance)) + offset;
            yPosition = (row * (verticalDistance));
        }
        else
        {
            shouldOffset = (column % 2) == 0;
            width = 2f * size;
            height = Mathf.Sqrt(3) * size;

            horizontalDistance = height * (3f / 4f);
            verticalDistance = width;

            offset = (shouldOffset) ? height / 2 : 0;

            xPosition = (column * (horizontalDistance));
            yPosition = (row * verticalDistance) + offset;
        }


        return new Vector3(xPosition + transform.position.x, transform.position.y, yPosition + transform.position.z);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
