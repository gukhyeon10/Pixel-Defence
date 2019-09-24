using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
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

    public Dictionary<int, EnemyStats> dicEnemyStats;

    [SerializeField]
    Transform EnemyDeck;

    int EnemyTotal;
    
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
        dicEnemyStats = new Dictionary<int, EnemyStats>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isTestPlay = false;

        for(int i= 0;i<(int)EnemyKind.ENEMY_KIND_COUNT; i++)
        {
            dicEnemyStats.Add(i, new EnemyStats(10f, 1f, 2f));
        }
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

                EnemyRootInit();
                FloorDictionaryInit();
            }
        }
    }

    //적 오브젝트 모두 삭제
    void EnemyRootInit()
    {
        List<int> keyList = dicEnemyDeck.Keys.ToList();
        for(int i= 0; i<keyList.Count; i++)
        {
            for(int j=0; j<dicEnemyDeck[keyList[i]].Count; j++)
            {
                Destroy(dicEnemyDeck[keyList[i]].Dequeue());
            }
        }
        dicEnemyDeck.Clear();

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

    //Enemy 스탯 업로드
    public void EnemyStatsUpload(UILabel label_Name, UIInput input_Hp, UIInput input_Def, UIInput input_Speed)
    {
        int no = (int)Enum.Parse(typeof(EnemyKind), label_Name.text);

        input_Hp.value = dicEnemyStats[no].hp.ToString();
        input_Def.value = dicEnemyStats[no].def.ToString();
        input_Speed.value = dicEnemyStats[no].speed.ToString();
    }

    public void EnemyStatsUpdate(UILabel label_Name, UIInput input_Hp, UIInput input_Def, UIInput input_Speed)
    {
        int no = (int)Enum.Parse(typeof(EnemyKind), label_Name.text);

        dicEnemyStats[no] = new EnemyStats(float.Parse(input_Hp.value),
                                           float.Parse(input_Def.value),
                                           float.Parse(input_Speed.value));

    }

    //Grid_EnemyStack의 자식 오브젝트들을 통해 리소스 로드
    void EnemyUpload()
    {
        EnemyTotal = 0;
        dicEnemyDeck.Clear();
        foreach(var startFloorObject in dicStartFloor)
        {
            foreach (Enemy enemy in enemyCursor.dicEnemyDeck[startFloorObject.Key])
            {
                int trackNumber = enemy.trackNumber; 
                
                GameObject prefab_Enemy = Resources.Load("Character Resources/" + enemy.name) as GameObject;

                GameObject startFloor = startFloorObject.Value;

                GameObject newEnemy = Instantiate(prefab_Enemy, Vector3.zero, Quaternion.identity, EnemyRoot.transform);

                TestEnemyScript testEnemyScript = newEnemy.AddComponent<TestEnemyScript>();
                testEnemyScript.trackNumber = trackNumber;
                testEnemyScript.nextGap = enemy.nextGap;
                testEnemyScript.SetStartPosition = new Vector3(startFloor.transform.position.x, 1f, startFloor.transform.position.z);
                testEnemyScript.stats = EnemyStatsReturn(enemy.name);

                //해당 트랙의 enemy덱이 존재한다면 push
                if(dicEnemyDeck.ContainsKey(trackNumber))
                {
                    dicEnemyDeck[trackNumber].Enqueue(newEnemy);
                    EnemyTotal++;
                }
                //해당 트랙의 enemy덱이 존재하지 않다면 스택 새로 만들고 push한 후 dictionary에 추가
                else
                {
                    Queue<GameObject> newEnemyDeck = new Queue<GameObject>();
                    newEnemyDeck.Enqueue(newEnemy);
                    
                    dicEnemyDeck.Add(trackNumber, newEnemyDeck);
                    EnemyTotal++;
                }
                    
            }
        }

      
    }

    EnemyStats EnemyStatsReturn(string enemyName)
    {
        EnemyStats stats = dicEnemyStats[(int)EnemyKind.DINOTREBLE]; ;
        switch(enemyName)
        {
            case "DinoTreble":
                {
                    stats = dicEnemyStats[(int)EnemyKind.DINOTREBLE];
                    break;
                }
            case "Mime":
                {
                    stats = dicEnemyStats[(int)EnemyKind.MIME];
                    break;
                }
            case "Samurai":
                {
                    stats = dicEnemyStats[(int)EnemyKind.SAMURAI];
                    break;
                }
            case "Zombie":
                {
                    stats = dicEnemyStats[(int)EnemyKind.ZOMBIE];
                    break;
                }
            case "ScientistRig":
                {
                    stats = dicEnemyStats[(int)EnemyKind.SCIENTISTRIG];
                    break;
                }
            case "Pirate":
                {
                    stats = dicEnemyStats[(int)EnemyKind.PIRATE];
                    break;
                }
            case "CharacterAnimations":
                {
                    stats = dicEnemyStats[(int)EnemyKind.CHARACTERANIM];
                    break;
                }
        }
        return stats;
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
            EnemyTotal--;
            enemy.GetComponent<TestEnemyScript>().EnemyStart();
            yield return new WaitForSeconds(enemy.GetComponent<TestEnemyScript>().nextGap);
        }

        if(EnemyTotal <= 0)
        {
            Debug.Log("모든 적 시뮬레이션 완료");
        }
        
    }
}
