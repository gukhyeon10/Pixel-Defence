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

public struct Floor
{
    int trackNumber;
    int floorNumber;
}