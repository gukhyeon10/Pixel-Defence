using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameCursor : MonoBehaviour
{
    [SerializeField]
    GameObject UnitRoot;
    [SerializeField]
    GameObject EnemyRoot;

    [SerializeField]
    GameMainProcess gameMainProcess;
    [SerializeField]
    UILabel Label_Price;

    [SerializeField]
    EnemyDetailManager enemyDetailManager;

    GameObject CursorUnit = null;
    bool isCube = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CursorCancel();

        CursorActive();

        CursorUpdate();

        CursorRayHitEnemy();

        CursorSet();
    }

    // 커서 초기화
    void CursorCancel()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CursorUnit != null)
            {
                Destroy(CursorUnit);
                CursorUnit = null;
                Label_Price.text = string.Empty;
            }
        }
    }

    // 커서에 배치할 유닛(미리보기) 활성화
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

            CursorUnit = Instantiate(prefab_Unit, Vector3.one * -1000f, Quaternion.identity, this.transform);
            Label_Price.text = CursorUnit.GetComponent<UnitScript>().price.ToString();
        }
    }

    // 커서 최신화
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
                isCube = true;
                if(hitInfo.point.y >= hitInfo.transform.position.y + 0.99f)
                CursorUnit.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
            }
            else
            {
                isCube = false;
            }

        }

    }
   
    //커서 셋팅
    void CursorSet()
    {
        if(Input.GetMouseButton(0) && CursorUnit != null && isCube)
        {

            GameObject newUnit = Instantiate(CursorUnit, CursorUnit.transform.position, Quaternion.identity, UnitRoot.transform);
            
            newUnit.GetComponent<UnitScript>().isCursor = false;
            newUnit.GetComponent<UnitScript>().EnemyRootLoad(EnemyRoot);

            Debug.Log(newUnit.GetComponent<UnitScript>().price);

            if(gameMainProcess.money >= newUnit.GetComponent<UnitScript>().price)
            {
                gameMainProcess.money -= newUnit.GetComponent<UnitScript>().price;
            }
            else
            {
                Destroy(newUnit);
            }

            Destroy(CursorUnit);
            CursorUnit = null;
            Label_Price.text = string.Empty;

        }
    }

    //필드의 적 RayCast
    void CursorRayHitEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.transform.tag.Equals("Enemy"))
            {
                enemyDetailManager.EnemyDetailActive(true);
            }
            else
            {
                enemyDetailManager.EnemyDetailActive(false);
            }
        }
        else
        {
            return;
        }
    }
}
