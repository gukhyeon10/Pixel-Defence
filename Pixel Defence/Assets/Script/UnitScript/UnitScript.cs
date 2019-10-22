using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    public bool isCursor = true;

    protected Animator anim;

    protected float speed = 1f;
    protected float coolTime = 1f;
    protected float attack = 1f;
    public float range = 10f;

    public GameObject EnemyRoot;
    public List<Transform> list_Enemy;
    public Transform target = null;

    public int price;

    GameEnemy TargetEnemy = null;

    protected void UnitInit()
    {
        list_Enemy = new List<Transform>();
        anim = this.GetComponent<Animator>();
        anim.runtimeAnimatorController = Resources.Load("CharacterAnim/UnitAnim") as RuntimeAnimatorController;
        AddCollider();
    }

    public void EnemyRootLoad(GameObject EnemyRoot)
    {
        this.EnemyRoot = EnemyRoot;
    }

    public void EnemyLoad()
    {
        if (isCursor) return;

        list_Enemy.Clear();

        for (int i = 0; i < EnemyRoot.transform.childCount; i++)
        {
            list_Enemy.Add(EnemyRoot.transform.GetChild(i));
        }

    }

    protected void FindTarget()
    {
        if (target == null)
        {
            int enemyFloorNumber = -1;
            float frontEnemyDistance = 0;
            for (int i = 0; i < list_Enemy.Count; i++)
            {
                Transform Enemy = list_Enemy[i];

                float distance = Vector3.Distance(Enemy.position, this.transform.position);

                // 일단 사정거리 체크
                if (distance <= range)
                {
                    GameEnemy TargetEnemyTemp = Enemy.GetComponent<GameEnemy>();
                    
                    //HP가 남아있는지
                    if (TargetEnemyTemp.getHp > 0f)
                    {
                        // 진행 경로가 더 앞서 있는 적 우선순위
                        if (enemyFloorNumber < TargetEnemyTemp.floorNumber)
                        {
                            frontEnemyDistance = Vector3.Distance(TargetEnemyTemp.targetPosition, Enemy.transform.position);
                            enemyFloorNumber = TargetEnemyTemp.floorNumber;
                            target = Enemy;
                            TargetEnemy = TargetEnemyTemp;
                        } 
                        // 진행 경로가 동일할 때 그 다음 타깃 경로와 거리가 짧은 적 우선순위   즉, 제일 앞선 적 우선
                        else if(enemyFloorNumber == TargetEnemyTemp.floorNumber)
                        {
                            float frontEnemyDisTemp = Vector3.Distance(TargetEnemyTemp.targetPosition, Enemy.transform.position);
                            if(frontEnemyDisTemp < frontEnemyDistance)
                            {
                                frontEnemyDistance = frontEnemyDisTemp;
                                target = Enemy;
                                TargetEnemy = TargetEnemyTemp;
                            }
                        }
                    }
                    
                    
                }

            }

        }
        else
        {
            float distance = Vector3.Distance(target.position, this.transform.position);
            if (distance > range || TargetEnemy.getHp <= 0f)
            {
                target = null;
                TargetEnemy = null;
            }
        }
    }

    protected void AddCollider()
    {
        if (isCursor) return;

        BoxCollider boxCollider = this.gameObject.AddComponent<BoxCollider>();
        boxCollider.center = new Vector3(0f, 6f, 0f);
        boxCollider.size = new Vector3(10f, 12f, 10f);
    }
}
