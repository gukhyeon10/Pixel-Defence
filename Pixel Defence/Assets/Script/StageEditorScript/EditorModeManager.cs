using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorModeManager : MonoBehaviour
{
    [SerializeField]
    FloorCursor floorCursor;
    [SerializeField]
    UnitCursor unitCursor;

    [SerializeField]
    UILabel label_CursorMode;
    [SerializeField]
    UILabel label_Testing;

    int CursorMode;

    

    // Start is called before the first frame update
    void Awake()
    {
        CursorMode = (int)CursorEditMode.FLOOR;
        floorCursor.isFloorCursor = true;
        floorCursor.gb_CursorObject.SetActive(true);

        unitCursor.isUnitCursor = false;
        unitCursor.gb_CursorObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //테스트 중인지 알림 label 활성화
        if(Input.GetKeyDown(KeyCode.F2))
        {
            label_Testing.gameObject.SetActive(!(label_Testing.gameObject.activeSelf));
        }

        //배치할 커서의 오브젝트 switch
        if(Input.GetKeyDown(KeyCode.F3))
        {
            switch(CursorMode)
            {
                case (int)CursorEditMode.FLOOR:
                    {
                        CursorMode = (int)CursorEditMode.ENEMY;

                        floorCursor.isFloorCursor = false;
                        floorCursor.gb_CursorObject.SetActive(false);

                        label_CursorMode.text = "Cursor : Enemy ";

                        break;
                    }
                case (int)CursorEditMode.ENEMY:
                    {
                        CursorMode = (int)CursorEditMode.UNIT;

                        unitCursor.isUnitCursor = true;
                        unitCursor.gb_CursorObject.SetActive(true);


                        label_CursorMode.text = "Cursor : Unit";
                        break;
                    }
                case (int)CursorEditMode.UNIT:
                    {
                        CursorMode = (int)CursorEditMode.FLOOR;

                        floorCursor.isFloorCursor = true;
                        floorCursor.gb_CursorObject.SetActive(true);

                        unitCursor.isUnitCursor = false;
                        unitCursor.gb_CursorObject.SetActive(false);


                        label_CursorMode.text = "Cursor : Floor";
                        break;
                    }
            }
        }
    }
}
