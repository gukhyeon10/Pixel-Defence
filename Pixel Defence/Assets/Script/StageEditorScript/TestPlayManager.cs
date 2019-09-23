using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayManager : MonoBehaviour
{
    bool isTestPlay;
    [SerializeField]
    GameObject FloorRoot;
    [SerializeField]
    GameObject EnemyRoot;

    [SerializeField]
    EnemyCursor enemyCursor;
    

    public GameObject prefab_Enemy;

    Dictionary<int, GameObject> dicStartFloor;
    public Dictionary<int, Dictionary<int,GameObject>> dicMiddleFloor;
    public Dictionary<int, GameObject> dicEndFloor;

    Dictionary<int, Queue<GameObject>> dicEnemyDeck;

    [SerializeField]
    Transform EnemyDeck;
    
    static private TestPlayManager _instance = null;

    static public TestPlayManager Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this; 

        dicStartFloor = new Dictionary<int, GameObject>();
        dicMiddleFloor = new Dictionary<int, Dictionary<int,GameObject>>();
        dicEndFloor = new Dictionary<int, GameObject>();

        dicEnemyDeck = new Dictionary<int, Queue<GameObject>>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isTestPlay = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2))
        {
            isTestPlay = !isTestPlay;

            if(isTestPlay)
            {
                Debug.Log("임시 테스트 진행");
                FloorUpload();
                EnemyUpload();

                TestPlayStart();
            }
            else
            {
                Debug.Log("테스트 중단");
                StopAllCoroutines();

                FloorDictionaryInit();
                EnemyRootInit();
            }
        }
    }

    //적 오브젝트 모두 삭제
    void EnemyRootInit()
    {
        foreach(Transform enemy in EnemyRoot.transform)
        {
            Destroy(enemy.gameObject);
        }
    }

    //장판 딕셔너리 초기화
    void FloorDictionaryInit()
    {
        dicStartFloor.Clear();
        dicMiddleFloor.Clear();
        dicEndFloor.Clear();
    }

    //임시 실행하기 위해 장판 정보 로드
    void FloorUpload()
    {
        int FloorCount = FloorRoot.transform.childCount;

        for(int i = 0; i<FloorCount; i++)
        {
           GameObject floor =  FloorRoot.transform.GetChild(i).gameObject;

            FloorScript floorInfo = floor.GetComponent<FloorScript>();
            
            switch(floorInfo.GetFloorKind)
            {
                case (int)FloorKind.START_FLOOR:
                    {
                        // 출발점 장판의 트랙 번호가 기존에 없으면 추가
                        if(!(dicStartFloor.ContainsKey(floorInfo.trackNumber)))
                        {
                            dicStartFloor.Add(floorInfo.trackNumber, floor);
                        }
                        break;
                    }
                case (int)FloorKind.MIDDLE_FLOOR:
                    {
                        // 중간 장판의 트랙 번호가 기존에 있으면 
                        if(dicMiddleFloor.ContainsKey(floorInfo.trackNumber))
                        {
                            // 해당 트랙 번호 딕셔너리에 장판 번호가 기존에 없으면 추가
                            if (!(dicMiddleFloor[floorInfo.trackNumber].ContainsKey(floorInfo.floorNumber)))
                            {
                                dicMiddleFloor[floorInfo.trackNumber].Add(floorInfo.floorNumber, floor);
                            }
                        }
                        // 중간 장판의 트랙 번호가 기존에 없으면 추가
                        else
                        {
                            Dictionary<int, GameObject> newDicMiddleFloor = new Dictionary<int, GameObject>();
                            newDicMiddleFloor.Add(floorInfo.floorNumber, floor);
                            dicMiddleFloor.Add(floorInfo.trackNumber, newDicMiddleFloor);
                        }
                        break;
                    }
                case (int)FloorKind.END_FLOOR:
                    {
                        // 종착점 장판의 트랙 번호가 기존에 없으면 추가
                        if (!(dicEndFloor.ContainsKey(floorInfo.trackNumber)))
                        {
                            dicEndFloor.Add(floorInfo.trackNumber, floor);
                        }
                        break;
                    }
            }

        }
    }

    //Grid_EnemyStack의 자식 오브젝트들을 통해 리소스 로드
    void EnemyUpload()
    {
        dicEnemyDeck.Clear();
        foreach(var startFloorObject in dicStartFloor)
        {
            foreach (Enemy enemy in enemyCursor.dicEnemyDeck[startFloorObject.Key])
            {
                int trackNumber = enemy.trackNumber; 
                
                GameObject prefab_Enemy = Resources.Load("Character Resources/" + enemy.name) as GameObject;


                GameObject startFloor = startFloorObject.Value;

                GameObject newEnemy = Instantiate(prefab_Enemy, new Vector3(startFloor.transform.position.x, 1f, startFloor.transform.position.z), Quaternion.identity, EnemyRoot.transform);
                TestEnemyScript testEnemyScript = newEnemy.AddComponent<TestEnemyScript>();
                testEnemyScript.trackNumber = trackNumber;
                testEnemyScript.nextGap = enemy.nextGap;

                //해당 트랙의 enemy덱이 존재한다면 push
                if(dicEnemyDeck.ContainsKey(trackNumber))
                {
                    dicEnemyDeck[trackNumber].Enqueue(newEnemy);
                    newEnemy.SetActive(false);
                }
                //해당 트랙의 enemy덱이 존재하지 않다면 스택 새로 만들고 push한 후 dictionary에 추가
                else
                {
                    Queue<GameObject> newEnemyDeck = new Queue<GameObject>();
                    newEnemyDeck.Enqueue(newEnemy);
                    newEnemy.SetActive(false);

                    dicEnemyDeck.Add(trackNumber, newEnemyDeck);
                }
                    
            }
        }

      
    }

    //임시 실행
    void TestPlayStart()
    {
        foreach(KeyValuePair<int, Queue<GameObject>> enemyDeck in dicEnemyDeck)
        {
            StartCoroutine(TestPlay(enemyDeck.Value));
        }
    }

    //임시 실행 코루틴
    IEnumerator TestPlay(Queue<GameObject> enemyDeck)
    {
        yield return null;
       
        foreach(GameObject enemy in enemyDeck)
        {
            enemy.SetActive(true);
            yield return new WaitForSeconds(enemy.GetComponent<TestEnemyScript>().nextGap);
        }
        
    }
}
