using System.Collections;
using System.Collections.Generic;

enum FloorKind
{
    START_FLOOR = 0,
    MIDDLE_FLOOR = 1,
    END_FLOOR = 2,
}

enum CursorEditMode
{
    FLOOR = 0,
    ENEMY = 1,
    UNIT = 2,
}

public struct Enemy
{
    public  string name;
    public int trackNumber;
    public int hp;
    public int def;
    public int speed;
    public float nextGap;

    public Enemy(string name, int trackNumber)
    {
        this.name = name;
        this.trackNumber = trackNumber;
        hp = 0;
        def = 0;
        speed = 0;
        nextGap = 3f;
    }
}