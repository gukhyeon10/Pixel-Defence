﻿using System.Collections;
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
    GameObject enemyPoint;

    [SerializeField]
    EnemyDetailManager enemyDetailManager;

    GameObject CursorUnit = null; // 배치할 유닛 (커서에 할당)
    Transform CursorEnemy = null; // 능력치를 확인할 유닛 (적 캐릭터)

    Transform ClickEnemy = null;
    GameEnemy ClickGameEnemy = null;

    GameEnemy gameEnemy = null; // 커서 오버된 적 유닛 능력치 스크립트

    bool isCube = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyPoint = Instantiate(enemyPoint, Vector3.zero, Quaternion.identity, this.transform);
        enemyPoint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CursorCancel();

        CursorActive();

        CursorUpdate();

        ClickEnemyActive();

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

            //Debug.Log(newUnit.GetComponent<UnitScript>().price);

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

                // 기존 가리키고 있는 적과 다른 적 캐릭터를 가리켰다면 참조 객체 갱신
                if(CursorEnemy != hitInfo.transform)
                {
                    CursorEnemy = hitInfo.transform;
                    gameEnemy = CursorEnemy.GetComponent<GameEnemy>();
                    enemyDetailManager.EnemyDetailActive(true, gameEnemy.stats, CursorEnemy.name);
                }
                else
                {
                    enemyDetailManager.EnemyDetailActive(true, gameEnemy.stats, CursorEnemy.name);
                }

                // 적 유닛을 가리키는 화살표 활성화
                enemyPoint.SetActive(true);
                enemyPoint.transform.position = CursorEnemy.transform.position + (Vector3.up * 4f);


                // 적 유닛 클릭
                if(Input.GetMouseButtonDown(0))
                {
                    ClickEnemy = CursorEnemy;
                    ClickGameEnemy = ClickEnemy.GetComponent<GameEnemy>();
                    enemyDetailManager.EnemyDetailActive(true, ClickGameEnemy.stats, ClickEnemy.name);
                }

            }
            else // 적을 가리키고 있지 않다면 비활성화
            {
                if(ClickEnemy == null || Input.GetMouseButtonDown(0))
                {
                    ClickEnemy = null;
                    ClickGameEnemy = null;
                    enemyDetailManager.EnemyDetailActive(false, new EnemyStats(), string.Empty);
                    enemyPoint.SetActive(false);
                }

                if(ClickEnemy != null)
                {
                    enemyPoint.SetActive(true);
                    enemyDetailManager.EnemyDetailActive(true, ClickGameEnemy.stats, ClickEnemy.name);
                }

            }
        }
        else
        {
            // 클릭된 유닛이 없을 경우 초기화
            if(ClickEnemy == null)
            {
                //enemyDetailManager.EnemyDetailActive(false, new EnemyStats(), string.Empty);
                enemyPoint.SetActive(false);
            }
            return;
        }
    }

    // 클릭한 유닛 디테일 창 고정 활성화
    void ClickEnemyActive()
    {
        if(ClickGameEnemy != null)
        {
            // 라이프 오버된 적 유닛
            if(ClickGameEnemy.GetIsGo == false)
            {
                ClickEnemy = null;
                ClickGameEnemy = null;
            }
        }

        // 클릭된 유닛이 존재한다면
        if(ClickEnemy != null)
        {
            enemyPoint.transform.position = ClickEnemy.position + (Vector3.up * 4f);
            enemyDetailManager.EnemyDetailActive(true, ClickGameEnemy.stats, ClickGameEnemy.name);
        }
        else
        {
            //enemyDetailManager.EnemyDetailActive(false, new EnemyStats(), string.Empty);
            enemyPoint.SetActive(false);
        }
    }
}
