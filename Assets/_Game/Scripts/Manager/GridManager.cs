using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    public Vector2 offsetGrid = new Vector2(2f, 2f);
    public int[,] gridData;
    public GridUnit[,] gridObject;
    public GridUnit gridUnitPrefabs;
    public Transform grid;
    public void LoadGrid(int cols, int rows, List<int> _gridData)
    {
        gridData = new int[rows, cols];
        gridObject = new GridUnit[rows, cols];

        // 1. Tính kích thước thực tế của grid
        float totalWidth = (cols - 1) * offsetGrid.x;
        float totalHeight = (rows - 1) * offsetGrid.y;

        // 2. Tính tọa độ bắt đầu (góc trên trái)
        Vector3 topCenter = Vector3.zero; // gốc là center-top của "grid"
        Vector3 origin = topCenter - new Vector3(totalWidth / 2f, 0, 0); // dịch trái và xuống

        for (int x = 0; x < _gridData.Count; x++)
        {
            int i = x / cols;
            int j = x % cols;

            gridData[i, j] = _gridData[x];

            Vector3 spawnPos = origin + new Vector3(j * offsetGrid.x, 0, -i * offsetGrid.y);
            GridUnit gridUnit = Instantiate(gridUnitPrefabs, grid);
            gridUnit.transform.localPosition = spawnPos;
            gridObject[i, j] = gridUnit;
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (gridData[i, j] == 0)
                    gridObject[i, j].Init();
                else
                    gridObject[i, j].Init(0);
            }
        }
    }

}
