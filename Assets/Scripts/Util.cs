using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Util
{
    public static int GetMinute(float time)
    {
        return (int)(time / 60);
    }

    public static int GetSecond(float time)
    {
        return (int)(time % 60);
    }

    public static void TilePoolShuffle(ref List<int> tileList)
    {
        int count = tileList.Count;
        int check = count;

        while (1 < check)
        {
            --check;
            int idx = Random.Range(0, count - 1);
            int temp = tileList[check];
            tileList[check] = tileList[idx];
            tileList[idx] = temp;
        }
    }

    public static void BoardShuffle(ref List<Cell> cellList, ref Cell[,] arrBoard)
    {
        int count = cellList.Count;
        int check = count;

        while (1 < check)
        {
            --check;
            int idx = Random.Range(0, count - 1);
            Coord checkCoord = cellList[check]._coord;
            Coord idxCoord = cellList[idx]._coord;
            cellList[check]._coord = idxCoord;
            cellList[idx]._coord = checkCoord;
            arrBoard[checkCoord.row, checkCoord.col] = cellList[idx];
            arrBoard[idxCoord.row, idxCoord.col] = cellList[check];
        }

        foreach (Cell cell in cellList)
        {
            SetTransform(cell.transform, cell._coord);
            cell.ToggleSelect(false);
        }
    }

    public static void SetTransform(Transform transform, Coord coord)
    {
        //Vector3 pos = new Vector3((coord.row * 85f) + 15f, (coord.col * -85f) - 19f, 0f);
        Vector3 pos = new Vector3((coord.row * 85f) + 57f, (coord.col * -85f) - 61f, 0f);
        transform.localPosition = pos;
    }
}
