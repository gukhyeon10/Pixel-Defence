using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCursor : MonoBehaviour
{
    public GameObject gb_CursorObject;
    [SerializeField]
    GameObject UnitRoot;

    public bool isUnitCursor;

    bool isRayHit;


    // Start is called before the first frame update
    void Start()
    {
        isRayHit = false;
        gb_CursorObject = Instantiate(gb_CursorObject, Vector3.zero, Quaternion.identity);
        
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(isUnitCursor)
        {
            UnitUpdate();

            UnitSet();
        }
    }

    void UnitUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            // 태그가 Cube이여야만 유닛 배치할 수 있도록  (임시방편으로 큐브 옆면에 배치못하도록 높이가 1, 3, 5 ... 홀수 일때만)
            if (hitInfo.transform.tag.Equals("Cube") && (int)hitInfo.point.y % 2 == 1)
            {
                isRayHit = true;
                gb_CursorObject.transform.position = new Vector3(hitInfo.point.x, (int)hitInfo.point.y , hitInfo.point.z);

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

    void UnitSet()
    {
        if (Input.GetMouseButtonDown(0) && isRayHit)
        {
            GameObject newUnit = Instantiate(gb_CursorObject, gb_CursorObject.transform.position, Quaternion.identity, UnitRoot.transform);
            newUnit.transform.eulerAngles = new Vector3(0f, Random.Range(0f, 360f), 0f);
            newUnit.name = newUnit.name.Substring(0, newUnit.name.Length - 7);

            newUnit.AddComponent<UnitScript>();
            newUnit.tag = "Unit";
            
        }
    }
}
