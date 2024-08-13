using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get { return _instance; }
    }
    #endregion
    #region Variables
    // GameObject
    [SerializeField] UILabel _labelTimer = null;
    [SerializeField] GameObject _goBoard = null;
    [SerializeField] GameObject _goTable = null;
    [SerializeField] GameObject _goLine = null;
    [SerializeField] GameObject _goPopupVictory = null;
    [SerializeField] GameObject _goPopupConfig = null;

    // Timer
    bool bTimerOn = false;
    bool bTimerStop = false;
    float fTimer = 0f;
    Coroutine _corTimer = null;

    // Cell
    [HideInInspector] public Cell[,] _gameBoard = null;
    [HideInInspector] public int[,] _tempBoard = null;
    List<Cell> _cellList = new List<Cell>();
    List<Coord> _bestRoute = new List<Coord>();
    List<Coord> _route = new List<Coord>();
    Cell _firstCell = null;
    Cell _hintSrcCell = null;
    Cell _hintDestCell = null;

    // For pathfinding
    bool _boardCheck = false;
    #endregion

    private void Awake()
    {
        if (null == _instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (this != _instance)
            Destroy(gameObject);

        // Setting cells
        for (int i = 0; i < BoardInfo.ROW * BoardInfo.COL; ++i)
        {
            GameObject prefab = Resources.Load("Prefabs/Cell") as GameObject;
            GameObject.Instantiate(prefab, _goTable.transform);
            prefab.layer = _goTable.layer;
            prefab.SetActive(false);
        }
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        // Timer
        bTimerOn = true;
        bTimerStop = false;
        fTimer = 0f;
        if (null != _corTimer)
            StopCoroutine(_corTimer);
        _corTimer = StartCoroutine("Timer");

        // Init variables
        _cellList.Clear();
        _bestRoute.Clear();
        _route.Clear();
        _gameBoard = null;
        _tempBoard= null;
        
        _firstCell = null;
        _hintSrcCell = null;
        _hintDestCell = null;

        // Board Setting
        BoardManager.Instance.ResetBoard();
        _gameBoard = new Cell[BoardInfo.ROW, BoardInfo.COL];

        TILE_SET curSet = BoardManager.Instance._info._tileSet;
        List<int> indexPool = BoardManager.Instance._itemPool;
        for (int i = 0; i < BoardInfo.ROW; ++i)
        {
            for (int j = 0; j < BoardInfo.COL; ++j)
            {
                Transform clone = _goTable.transform.GetChild(i * BoardInfo.COL + j);
                if (!clone)
                    continue;

                Cell cell = clone.gameObject.GetComponent<Cell>();
                if (!cell)
                    continue;

                Coord pair = new Coord(i, j);
                cell.SetCoord(pair);
                _gameBoard[i, j] = cell;

                if (0 != BoardManager.Instance._arrBoard[i, j])
                {
                    cell.SetIcon(curSet, indexPool[0]);
                    indexPool.RemoveAt(0);
                    _cellList.Add(cell);
                    cell.SetActive(true);
                }
                else
                {
                    cell.SetActive(false);
                    Util.SetTransform(cell.transform, cell._coord);
                }
            }
        }

        Util.BoardShuffle(ref _cellList, ref _gameBoard);
        StartCoroutine(CheckAllBoard());
    }

    private void DrawLine()
    {
        for (int i = 0; i < _bestRoute.Count - 1; ++i)
        {
            GameObject clone = GameObject.Instantiate(_goLine, _goTable.transform);

            Coord start = _bestRoute[i];
            Coord end = _bestRoute[i + 1];

            Util.SetTransform(clone.transform, start);

            if (start.row < end.row)
                clone.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
            else if (start.col > end.col)
                clone.transform.localEulerAngles = new Vector3(0f, 0f, 270f);
            else if (start.col < end.col)
                clone.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
        }
    }

    private void ShowShufflePopup()
    {
        Debug.Log("Board shuffled");
    }
    
    private void ShowGameEndPopup()
    {
        GameObject clone = GameObject.Instantiate(_goPopupVictory, _goBoard.transform);
        clone.SetActive(true);

        bTimerOn = false;
        bTimerStop = true;
    }

    public void ResumeGame()
    {
        bTimerOn = true;
    }

    #region OnClickEvent
    public void OnClickCell(Cell me)
    {
        if (!_boardCheck)
            return;

        if (null == _firstCell)
        {
            // regist first cell
            _firstCell = me;
            me.ToggleSelect(true);
        }
        else
        {
            // clicked same cell
            if (me == _firstCell)
                return;

            // check the sprite index
            if (me._spriteIdx != _firstCell._spriteIdx)
            {
                // different
                _firstCell.ToggleSelect(false);
                _firstCell = me;
                _firstCell.ToggleSelect(true);
            }
            else
            {
                // same ¡æ check route
                StartCoroutine(CheckItem(_firstCell, me));
                _firstCell = null;
            }
        }
    }
    public void OnClickHint()
    {
        StartCoroutine(FindHint());
    }
    public void OnClickShuffle()
    {
        Util.BoardShuffle(ref _cellList, ref _gameBoard);
        StartCoroutine(CheckAllBoard());
    }
    public void OnClickTest()
    {
        ShowGameEndPopup();
    }
    public void OnClickConfig()
    {
        GameObject clone = GameObject.Instantiate(_goPopupConfig, _goBoard.transform);
        clone.SetActive(true);

        bTimerOn = false;
    }
    #endregion

    #region Coroutine functions
    IEnumerator Timer()
    {
        while(true)
        {
            if (bTimerStop)
                yield break;

            if (!bTimerOn)
                yield return null;

            fTimer += Time.deltaTime;

            string minute = Util.GetMinute(fTimer).ToString("00");
            string second = Util.GetSecond(fTimer).ToString("00");

            if (_labelTimer)
                _labelTimer.text = minute + ":" + second;

            yield return null;
        }
    }
    IEnumerator CheckAllBoard()
    {
        _boardCheck = false;

        // Check all board if there is a pair to connect
        _tempBoard = (int[,])BoardManager.Instance._arrBoard.Clone();
        List<Cell> tempCellList = new List<Cell>();

        Cell srcCell = null;
        Cell destCell = null;

        tempCellList = _cellList.ToList();
        while (true)
        {
            srcCell = tempCellList[0];
            for (int i = 1; i < tempCellList.Count; ++i)
            {
                if (srcCell._spriteIdx != tempCellList[i]._spriteIdx)
                    continue;

                destCell = tempCellList[i];
                if (PathExploreForHint(srcCell._coord.row, srcCell._coord.col,
                                       destCell._coord.row, destCell._coord.col, MOVE_DIR.NONE, 0))
                {
                    _hintSrcCell = srcCell;
                    _hintDestCell = destCell;
                    _boardCheck = true;
                    tempCellList.Clear();
                    yield break;
                }
            }

            if (0 < tempCellList.Count)
                tempCellList.RemoveAt(0);

            if (0 >= tempCellList.Count)
            {
                ShowShufflePopup();
                Util.BoardShuffle(ref _cellList, ref _gameBoard);
                tempCellList = _cellList.ToList();
            }

            yield return null;
        }
    }
    IEnumerator CheckItem(Cell first, Cell last)
    {
        first.ToggleSelect(false);

        _route.Clear();
        _bestRoute.Clear();
        _tempBoard = (int[,])BoardManager.Instance._arrBoard.Clone();

        PathExplore(first._coord.row, first._coord.col, last._coord.row, last._coord.col, MOVE_DIR.NONE, 0);

        // Succeed!
        if (0 < _bestRoute.Count)
        {
            // Draw connect line
            DrawLine();

            // Move cells
            //MoveCells();

            // Add Score
            //AddScore();

            // Remove connected cells
            first.SetActive(false);
            last.SetActive(false);
            _cellList.Remove(first);
            _cellList.Remove(last);
            BoardManager.Instance._arrBoard[first._coord.row, first._coord.col] = 0;
            BoardManager.Instance._arrBoard[last._coord.row, last._coord.col] = 0;

            // Victory check
            if (0 == _cellList.Count)
            {
                ShowGameEndPopup();
                yield break;
            }

            // Board check
            IEnumerator checkBoard = CheckAllBoard();
            while (checkBoard.MoveNext())
                yield return null;
        }

        yield break;
    }
    IEnumerator FindHint()
    {
        if (null == _hintSrcCell || null == _hintDestCell)
        {
            IEnumerator checkBoard = CheckAllBoard();
            while (checkBoard.MoveNext())
                yield return null;
        }

        if (null != _hintSrcCell && null != _hintDestCell)
        {
            _hintSrcCell.ToggleSelect(true);
            _hintDestCell.ToggleSelect(true);
        }

        _hintSrcCell = null;
        _hintDestCell = null;
    }
    #endregion

    #region Pathfind functions
    void PathExplore(int row, int col, int dest_row, int dest_col, MOVE_DIR move, int rotate)
    {
        if (2 < rotate)
            return;

        if (0 > row || BoardInfo.ROW <= row || 0 > col || BoardInfo.COL <= col)
            return;

        if (row == dest_row && col == dest_col)
        {
            if (0 == _route.Count)
                return;

            int count = _bestRoute.Count;
            if (0 == count)
                count = int.MaxValue;

            if (_route.Count < count)
            {
                _bestRoute.Clear();
                foreach (var v in _route)
                    _bestRoute.Add(v);

                Coord myPair = new Coord(row, col);
                _bestRoute.Add(myPair);
            }
            return;
        }

        if (MOVE_DIR.NONE != move && 0 != _tempBoard[row, col])
            return;

        int alive = _tempBoard[row, col];
        _tempBoard[row, col] = 1;
        Coord pair = new Coord(row, col);
        _route.Add(pair);

        //Left
        if (move == MOVE_DIR.LEFT || move == MOVE_DIR.NONE)
            PathExplore(row - 1, col, dest_row, dest_col, MOVE_DIR.LEFT, rotate);
        else
            PathExplore(row - 1, col, dest_row, dest_col, MOVE_DIR.LEFT, rotate + 1);

        //Up
        if (move == MOVE_DIR.UP || move == MOVE_DIR.NONE)
            PathExplore(row, col - 1, dest_row, dest_col, MOVE_DIR.UP, rotate);
        else
            PathExplore(row, col - 1, dest_row, dest_col, MOVE_DIR.UP, rotate + 1);

        //Right
        if (move == MOVE_DIR.RIGHT || move == MOVE_DIR.NONE)
            PathExplore(row + 1, col, dest_row, dest_col, MOVE_DIR.RIGHT, rotate);
        else
            PathExplore(row + 1, col, dest_row, dest_col, MOVE_DIR.RIGHT, rotate + 1);

        //Down
        if (move == MOVE_DIR.DOWN || move == MOVE_DIR.NONE)
            PathExplore(row, col + 1, dest_row, dest_col, MOVE_DIR.DOWN, rotate);
        else
            PathExplore(row, col + 1, dest_row, dest_col, MOVE_DIR.DOWN, rotate + 1);

        _route.RemoveAt(_route.Count - 1);
        _tempBoard[row, col] = alive;

        return;
    }
    bool PathExploreForHint(int row, int col, int dest_row, int dest_col, MOVE_DIR move, int rotate)
    {
        if (2 < rotate)
            return false;

        if (0 > row || BoardInfo.ROW <= row || 0 > col || BoardInfo.COL <= col)
            return false;

        if (row == dest_row && col == dest_col)
            return true;

        if (MOVE_DIR.NONE != move && 0 != _tempBoard[row, col])
            return false;

        bool check = true;

        int alive = _tempBoard[row, col];
        _tempBoard[row, col] = 1;

        // Left
        if (MOVE_DIR.LEFT == move || MOVE_DIR.NONE == move)
            check = PathExploreForHint(row, col - 1, dest_row, dest_col, MOVE_DIR.LEFT, rotate);
        else
            check = PathExploreForHint(row, col - 1, dest_row, dest_col, MOVE_DIR.LEFT, rotate + 1);

        if (check) return true;

        // Up
        if (MOVE_DIR.UP == move || MOVE_DIR.NONE == move)
            check = PathExploreForHint(row - 1, col, dest_row, dest_col, MOVE_DIR.UP, rotate);
        else
            check = PathExploreForHint(row - 1, col, dest_row, dest_col, MOVE_DIR.UP, rotate + 1);

        if (check) return true;

        // Right
        if (MOVE_DIR.RIGHT == move || MOVE_DIR.NONE == move)
            check = PathExploreForHint(row, col + 1, dest_row, dest_col, MOVE_DIR.RIGHT, rotate);
        else
            check = PathExploreForHint(row, col + 1, dest_row, dest_col, MOVE_DIR.RIGHT, rotate + 1);

        if (check) return true;

        // Down
        if (MOVE_DIR.DOWN == move || MOVE_DIR.NONE == move)
            check = PathExploreForHint(row + 1, col, dest_row, dest_col, MOVE_DIR.DOWN, rotate);
        else
            check = PathExploreForHint(row + 1, col, dest_row, dest_col, MOVE_DIR.DOWN, rotate + 1);

        if (check)
            return true;

        _tempBoard[row, col] = alive;

        return false;
    }
    #endregion
}
