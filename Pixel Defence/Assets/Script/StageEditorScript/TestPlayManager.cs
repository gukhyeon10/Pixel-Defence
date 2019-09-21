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

    public GameObject prefab_Enemy;

    Dictionary<int, GameObject> dicStartFloor;
    public Dictionary<int, Dictionary<int,GameObject>> dicMiddleFloor;
    public Dictionary<int, GameObject> dicEndFloor;
    
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
                StartCoroutine(TestPlay());
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

    void FloorDictionaryInit()
    {
        dicStartFloor.Clear();
        dicMiddleFloor.Clear();
        dicEndFloor.Clear();
    }

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

    IEnumerator TestPlay()
    {
        yield return null;
        foreach(var startFloorObject in dicStartFloor)
        {
            GameObject startFloor = startFloorObject.Value;
            GameObject TestEnemy = Instantiate(prefab_Enemy, new Vector3(startFloor.transform.position.x, 1f, startFloor.transform.position.z), Quaternion.identity, EnemyRoot.transform);
            TestEnemy.AddComponent<TestEnemyScript>();
            TestEnemy.GetComponent<TestEnemyScript>().trackNumber = startFloorObject.Key;

        }
        yield return new WaitForSeconds(10f);
        StartCoroutine(TestPlay());
    }
}
