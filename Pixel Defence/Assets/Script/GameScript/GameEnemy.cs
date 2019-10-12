using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnemy : TestEnemyScript
{
    public Dictionary<int, Floor> dicTrack;
    // Start is called before the first frame update
    void Start()
    {
        EnemyInit();
    }
    
    protected override void  EnemyInit()
    {
        this.transform.position = initPosition;
        isGo = false;


    }

    public override void EnemyStart()
    {
        this.transform.position = new Vector3(dicTrack[0].x, 1f, dicTrack[0].z);

        floorNumber = 0;
        floorCount = 0;

        targetPosition = this.transform.position;
        targetPosition.y = 1f;

        isGo = true;

        this.GetComponent<Animator>().speed = stats.speed / 2f;
        
        GameObject prefab_Portal = Resources.Load("Floor Resources/StartPortal") as GameObject;
        GameObject startPortal = Instantiate(prefab_Portal, this.transform.position, Quaternion.identity);
        startPortal.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //종착점을 향해 가지 않는다면 그 다음 장판으로 이동
        if (isGo)
        {
            RotateToTarget();
            MoveFloor();
        }
    }

    protected override void MoveFloor()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, stats.speed * Time.deltaTime);
        

        if (this.transform.position == targetPosition)
        {
            
            if(dicTrack[floorNumber].isEnd)
            {
                GameObject prefab_Portal = Resources.Load("Floor Resources/EndPortal") as GameObject;
                GameObject startPortal = Instantiate(prefab_Portal, this.transform.position, Quaternion.identity);
                startPortal.transform.eulerAngles = new Vector3(-90f, 0f, 0f);

                this.transform.position = initPosition;
                isGo = false;
                
                GameMainProcess.totalEnemy--;
                
            }

            if (dicTrack.ContainsKey(++floorNumber))  // ++를 앞에 해줌으로써 오름차순 floor 탐색 가능
            {
                
                targetPosition = new Vector3(dicTrack[floorNumber].x, 1f, dicTrack[floorNumber].z);
                //첫번째 floor는 바로 바라보기
                if (floorCount <= 0)
                {
                    floorCount++;
                    this.transform.LookAt(targetPosition);
                }

            }
            
        }
    }
}
