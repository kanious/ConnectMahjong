using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager
{
    #region Singleton
    private static BoardManager _instance = null;
    public static BoardManager Instance
    {
        get
        {
            if (null == _instance)
                _instance = new BoardManager();
            return _instance;
        }
    }
    #endregion

    public BoardInfo _info = new BoardInfo();

    public int[,] _arrBoard = null;
    public List<int> _itemPool = new List<int>();
    public DIR _dir = DIR.NONE;
    public int _cellCount = 0;

    public void ResetBoard()
    {
        _itemPool.Clear();
        BoardSetting();
    }

    private void BoardSetting()
    {
        // Board Design (sample)
        switch (_info._boardNum)
        {
            case 0:
                _arrBoard = new int[BoardInfo.ROW, BoardInfo.COL]
                {
                    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                    {1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
                    {1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
                    {1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
                    {1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
                    {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                };
                break;

            default:
                _arrBoard = new int[BoardInfo.ROW, BoardInfo.COL]
                {
                    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                    {1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
                    {1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1 },
                    {1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1 },
                    {1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
                    {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                };
                break;
        }

        // Move Direction
        switch(_info._dir)
        {
            case DIR.RANDOM:
            case DIR.END:
                _dir = DIR.NONE;  // TODO : add random
                break;

            default:
                _dir = _info._dir;
                break;
        }

        // Calculate cell count
        _cellCount = 0;
        for (int i = 0; i < BoardInfo.ROW; ++i)
        {
            for (int j = 0; j < BoardInfo.COL; ++j)
            {
                if (1 == _arrBoard[i, j])
                    ++_cellCount;
            }
        }

        // Init item pool
        List<int> tempItemPool = new List<int>();
        for (int i = 0; i < _info._tileCount; ++i)
            tempItemPool.Add(i + 1);
        if ((_cellCount / 2) > _info._tileCount)
        {
            int gap = (_cellCount / 2) - _info._tileCount;
            for (int i = 0; i < gap; ++i)
                tempItemPool.Add(Random.Range(1, _info._tileCount));
        }
        Util.TilePoolShuffle(ref tempItemPool);

        for (int i = 0; i < _cellCount / 2; ++i)
        {
            _itemPool.Add(tempItemPool[i]);
            _itemPool.Add(tempItemPool[i]);
        }
    }
}
