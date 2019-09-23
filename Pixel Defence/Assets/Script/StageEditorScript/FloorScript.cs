using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScript : MonoBehaviour
{
    int floorKind;
    public int trackNumber = 0;
    public int floorNumber;

    void Start()
    {
        if(this.name.Equals("StartFloor"))
        {
            floorKind = (int)FloorKind.START_FLOOR;
            //trackNumber = 0;
            floorNumber = -1;
        }
        else if(this.name.Equals("MiddleFloor"))
        {
            floorKind = (int)FloorKind.MIDDLE_FLOOR;
            //trackNumber = 0;
            //floorNumber = 0;
        }
        else if(this.name.Equals("EndFloor"))
        {
            floorKind = (int)FloorKind.END_FLOOR;
            //trackNumber = 0;
            floorNumber = -1;
        }
    }

    public int GetFloorKind
    {
        get
        {
            return floorKind;
        }
    }
    
}
