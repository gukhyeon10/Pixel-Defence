using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCursor : MonoBehaviour
{
    [SerializeField]
    GameObject[] prefab_Floor;

    public GameObject gb_CursorObject;

    [SerializeField]
    GameObject FloorRoot;

    public bool isFloorCursor;

    bool isRayHit; // 마우스가 큐브를 가리키고 있는지

    public int floorCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        isRayHit = false;
        gb_CursorObject = Instantiate(prefab_Floor[(int)FloorKind.MIDDLE_FLOOR], Vector3.zero, Quaternion.identity);
        gb_CursorObject.transform.eulerAngles = new Vector3(90f, 0f, 0f);
        gb_CursorObject.name = gb_CursorObject.name.Substring(0, gb_CursorObject.name.Length - 7);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(isFloorCursor)
        {
            FloorUpdate();

            FloorSet();

            FloorChange();
        }
    }

    //장판 위치 업데이트
    void FloorUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            // 태그가 Cube이여야 하면 무조건 Y값 0높이인 큐브에만 장판 깔수 있도록
            if (hitInfo.transform.tag.Equals("Cube") && hitInfo.transform.position.y == 0f)
            {
                isRayHit = true;
                gb_CursorObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + 1f, hitInfo.point.z);

            }
            else
            {
                isRayHit = false;
            }
        }
        else
        {
            isRayHit = false;
        }
    }

    //장판 배치
    void FloorSet()
    {
        if(Input.GetMouseButtonDown(0) && isRayHit)
        {
            GameObject newFloor = Instantiate(gb_CursorObject, gb_CursorObject.transform.position, Quaternion.identity, FloorRoot.transform);
            newFloor.transform.eulerAngles = gb_CursorObject.transform.eulerAngles;
            newFloor.name = newFloor.name.Substring(0, newFloor.name.Length - 7);

            newFloor.AddComponent<FloorScript>();

            //배치할때마다 플로어 인덱스 증가
            if(newFloor.name.Equals("MiddleFloor"))
            {
                newFloor.GetComponent<FloorScript>().floorNumber = floorCount++;
            }
        }
    }

    void FloorChange()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Destroy(gb_CursorObject);
            gb_CursorObject = Instantiate(prefab_Floor[(int)FloorKind.START_FLOOR], Vector3.zero, Quaternion.identity);
            gb_CursorObject.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
            gb_CursorObject.name = gb_CursorObject.name.Substring(0, gb_CursorObject.name.Length - 7);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Destroy(gb_CursorObject);
            gb_CursorObject = Instantiate(prefab_Floor[(int)FloorKind.MIDDLE_FLOOR], Vector3.zero, Quaternion.identity);
            gb_CursorObject.transform.eulerAngles = new Vector3(90f, 0f, 0f);
            gb_CursorObject.name = gb_CursorObject.name.Substring(0, gb_CursorObject.name.Length - 7);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Destroy(gb_CursorObject);
            gb_CursorObject = Instantiate(prefab_Floor[(int)FloorKind.END_FLOOR], Vector3.zero, Quaternion.identity);
            gb_CursorObject.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
            gb_CursorObject.name = gb_CursorObject.name.Substring(0, gb_CursorObject.name.Length - 7);
        }
    }
    
}
