
public enum DIR
{
    NONE = 0,
    RANDOM,
    LEFT,
    RIGHT,
    UP,
    DOWN,
    LEFT_RIGHT_OUT,
    UP_DOWN_OUT,
    LEFT_RIGHT_IN,
    UP_DOWN_IN,
    END
}

public enum TILE_SET
{
    NONE = 0,
    RANDOM,
    ALCHEMY,
    CARTOGRAPHY,
    CROSSHAIR,
    FANTASY,
    FANTASY2,
    FOOD,
    LEAF,
    PUMPKIN,
    RANK,
    TREASURE,
    END
}

public enum MOVE_DIR
{
    NONE = 0,
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public struct Coord
{
    public int row;
    public int col;

    public Coord(int i, int j)
    {
        row = i;
        col = j;
    }
}