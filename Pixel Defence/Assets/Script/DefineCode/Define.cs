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

enum EnemyKind
{
    DINOTREBLE= 0,
    MIME= 1,
    SAMURAI= 2,
    ZOMBIE = 3,
    SCIENTISTRIG= 4,
    PIRATE = 5,
    CHARACTERANIM= 6,
    ENEMY_KIND_COUNT= 7,
}

public struct User
{
    public string name;
    public string pw;
    public int chapterLimit;
}

public struct Enemy
{
    public string name;
    public int trackNumber;
    public float nextGap;

    public Enemy(string name, int trackNumber, float nextGap)
    {
        this.name = name;
        this.trackNumber = trackNumber;
        this.nextGap = nextGap;
    }
}

public struct EnemyStats
{
    public float hp;
    public float def;
    public float speed;

    public EnemyStats(float hp, float def, float speed)
    {        
        this.hp = hp;
        this.def = def;
        this.speed = speed;
    }
  
}

public struct Floor
{
    public int trackNumber;
    public int floorNumber;
    public bool isStart;
    public bool isEnd;
    public float x;
    public float z;

    public Floor(int trackNumber, int floorNumber, bool isStart, bool isEnd, float x, float z)
    {
        this.trackNumber = trackNumber;
        this.floorNumber = floorNumber;
        this.isStart = isStart;
        this.isEnd = isEnd;
        this.x = x;
        this.z = z;
    }
}
