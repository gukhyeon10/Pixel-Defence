using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyScript : MonoBehaviour
{
    TestPlayManager testPlayManager;

    public int trackNumber;   // 트랙 번호
    public int floorCount;    // 이동한 장판 카운트
    public float nextGap = 2f;  // 다음 유닛 스폰 시간

    protected int floorNumber;          // 이동할 장판 번호
    protected bool isGo = false;      // 트랙 달리기 시작
    private bool isTowardEndFloor;  // 종착점 장판을 가고 있는지

    private Dictionary<int, GameObject> dicTrack;  // 트랙 딕셔너리
    private Transform endFloor;     // 종착점 오브젝트

    protected Vector3 targetPosition;   // 다음 이동 포지션
    protected Vector3 initPosition = new Vector3(-1000f, -1000f, -1000f);   // 비활성화 위치
    private Vector3 startPosition;   // 처음 시작 부분

    public EnemyStats stats;

    // Start is called before the first frame update
    void Start()
    {
        EnemyInit();
    }

    protected virtual void EnemyInit()
    {
        testPlayManager = TestPlayManager.Instance;
        dicTrack = new Dictionary<int, GameObject>();

        if (testPlayManager.dicMiddleFloor.ContainsKey(trackNumber))
        {
            dicTrack = testPlayManager.dicMiddleFloor[trackNumber];
        }

        if (testPlayManager.dicEndFloor.ContainsKey(trackNumber))
        {
            endFloor = testPlayManager.dicEndFloor[trackNumber].transform;
        }
        else
        {
            endFloor = null;
        }

        this.transform.position = initPosition;
    }

    // 풀링에서 시작floor로 시작
    public virtual void EnemyStart()
    {
        this.transform.position = startPosition;   // 시작 floor에 위치

        isTowardEndFloor = false;  // 종착점을 찾아 가는지 체크

        floorNumber = -1;  // 0번 floor부터 찾기 위함
        floorCount = 0;

        targetPosition = this.transform.position;
        targetPosition.y = 1f;

        isGo = true;

        this.GetComponent<Animator>().speed = stats.speed/2f;  // 애니메이션 속도 비례
    }

    // Update is called once per frame
    void Update()
    {
        //종착점을 향해 가지 않는다면 그 다음 장판으로 이동
        if(isGo && !isTowardEndFloor)
        {
            RotateToTarget();
            MoveFloor();
        }
    }

    //다음 FLOOR로 이동
    protected virtual void MoveFloor()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, stats.speed * Time.deltaTime);

        
        if (this.transform.position == targetPosition)
        {

            if (dicTrack.ContainsKey(++floorNumber))  // ++를 앞에 해줌으로써 오름차순 floor 탐색 가능
            {

                floorCount++;
                Transform target = dicTrack[floorNumber].transform;
                targetPosition = target.position;
                targetPosition.y = 1f;

                //첫번째 floor는 바로 바라보기
                if (floorCount <= 1)
                {
                    this.transform.LookAt(target);
                }

            }
            else
            {
                if (dicTrack.Count - 1 <= floorCount && endFloor != null)
                {
                    Transform target = endFloor;
                    targetPosition = target.position;
                    targetPosition.y = 1f;

                    isTowardEndFloor = true;
                    StartCoroutine(TowardEndFloor());
                }
            }
        }
    }


    //타겟을 향해 부드럽게 회전
    protected void RotateToTarget()
    {
        Vector3 vec =  targetPosition - this.transform.position;
        float step = stats.speed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(this.transform.forward, vec, step, 0.0f);

        this.transform.rotation = Quaternion.LookRotation(newDir);
        
    }

    protected void DieCheck()
    {
        if(this.stats.hp <= 0f)
        {
            Debug.Log(this.name);
            this.isGo = false;

            this.GetComponent<BoxCollider>().enabled = false;

            if(GameMainProcess.Instance != null)
            {
                GameMainProcess.Instance.totalEnemy--;
            }

            StartCoroutine(DisableEffect());
        }
    
    }

    // 종착점을 향한 마지막 이동
    IEnumerator TowardEndFloor()
    {
        while(true)
        {
            RotateToTarget();
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, stats.speed * Time.deltaTime);
            yield return null;
            if(this.transform.position.x == endFloor.position.x && this.transform.position.z == endFloor.position.z)
            {

                this.transform.position = initPosition;
                break;
            }
        }
    }

    // 적 유닛 HP 0일시 작아지게끔
    IEnumerator DisableEffect()
    {
        while(this.transform.localScale.x > 0.01f)
        {
            this.transform.localScale *= 0.9f;
            yield return new WaitForSeconds(0.005f);
        }
        this.transform.position = initPosition;
    }

    public Vector3 SetStartPosition
    {
        set
        {
            this.startPosition = value;
        }
    }
}
