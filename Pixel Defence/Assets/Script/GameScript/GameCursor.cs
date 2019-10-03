using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameCursor : MonoBehaviour
{
    [SerializeField]
    GameObject UnitRoot;
    
    GameObject CursorUnit = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CursorActive();

        CursorUpdate();

        CursorSet();
    }


    void CursorActive()
    {

        bool isUnit = false;
        string dataPath = "Character Resources/";
        
        switch(Input.inputString)
        {
            case "1":
                {
                    dataPath += "Alchemist";
                    isUnit = true;
                    break;
                }
            case "2":
                {
                    dataPath += "Farmer";
                    isUnit = true;
                    break;
                }
            case "3":
                {
                    dataPath += "Magician";
                    isUnit = true;
                    break;
                }
            case "4":
                {
                    dataPath += "Policeman";
                    isUnit = true;
                    break;
                }
            case "5":
                {
                    dataPath += "Ninja";
                    isUnit = true;
                    break;
                }
            case "6":
                {
                    dataPath += "Soldier";
                    isUnit = true;
                    break;
                }
            case "7":
                {
                    dataPath += "Wizard";
                    isUnit = true;
                    break;
                }
            case "8":
                {
                    dataPath += "Angel";
                    isUnit = true;
                    break;
                }
            case "9":
                {
                    dataPath += "Zeus";
                    isUnit = true;
                    break;
                }
            case "0":
                {
                    dataPath += "Hero";
                    isUnit = true;
                    break;
                }
        }

        if(isUnit)
        {
            if (CursorUnit != null)
            {
                Destroy(CursorUnit);
                CursorUnit = null;
            }

            GameObject prefab_Unit = Resources.Load(dataPath) as GameObject;

            CursorUnit = Instantiate(prefab_Unit, Vector3.zero, Quaternion.identity, this.transform);
        }
    }

    void CursorUpdate()
    {
        if(CursorUnit == null)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo))
        {
            if(hitInfo.transform.tag.Equals("Cube"))
            {
                CursorUnit.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
            }
        }
    }

    void CursorSet()
    {
        if(CursorUnit != null && Input.GetMouseButton(0))
        {
            GameObject newUnit = Instantiate(CursorUnit, CursorUnit.transform.position, Quaternion.identity, UnitRoot.transform);

            newUnit.AddComponent<GameUnit>();

            Destroy(CursorUnit);
            CursorUnit = null;
        }
    }
}
