using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnemy : TestEnemyScript
{
    // Start is called before the first frame update
    void Start()
    {
        EnemyInit();
    }
    

    // Update is called once per frame
    void Update()
    {
        //종착점을 향해 가지 않는다면 그 다음 장판으로 이동
        if (isGo && !isTowardEndFloor)
        {
            RotateToTarget();
            MoveFloor();
        }
    }
}
