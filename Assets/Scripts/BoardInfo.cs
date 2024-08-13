using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInfo
{
    public const int ROW = 8;
    public const int COL = 11;
    public const int HROW = 4;
    public const int HCOL = 6;

    public int _boardNum = 0;
    public DIR _dir = DIR.RANDOM;
    public TILE_SET _tileSet = TILE_SET.RANDOM;
    public int _tileCount = 0;

    public BoardInfo()
    {
        LoadInfo();
    }

    private void LoadInfo()
    {
        // TODO : Load options from PlayerPrefs

        // 1. board shape (random or int)
        // 2. tile move direction (random enum)
        // 3. tile image (random enum)
        // 4. ??

        _boardNum = 0;
        _dir = DIR.NONE;
        _tileSet = TILE_SET.CARTOGRAPHY;
        _tileCount = GetTileCount(_tileSet);
    }

    private void SaveInfo()
    {
        // TODO : Save infos to the PlayerPrefs
    }

    private int GetTileCount(TILE_SET tile)
    {
        int count = 0;
        switch (tile)
        {
            case TILE_SET.ALCHEMY:      count = 100;    break;
            case TILE_SET.CARTOGRAPHY:  count = 54;     break;
            case TILE_SET.CROSSHAIR:    count = 49;     break;
            case TILE_SET.FANTASY:      count = 60;     break;
            case TILE_SET.FANTASY2:     count = 45;     break;  
            case TILE_SET.FOOD:         count = 100;    break;      
            case TILE_SET.LEAF:         count = 50;     break;
            case TILE_SET.PUMPKIN:      count = 71;     break;
            case TILE_SET.RANK:         count = 156;    break;
            case TILE_SET.TREASURE:     count = 51;     break;

            default:                break;
        }
        return count;
    }
}
