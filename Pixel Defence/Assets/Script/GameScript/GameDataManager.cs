using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml;
using System;

public class GameDataManager : MonoBehaviour
{
    const string dataPath = "ChapterData/";

    [SerializeField]
    GameObject MapRoot;
    [SerializeField]
    GameObject EnemyRoot;

    public Dictionary<int, Dictionary<int, Floor>> dicTrack;
    public Dictionary<int, List<Enemy>> dicEnemyInfo;
    public Dictionary<int, Queue<GameObject>> dicEnemyDeck;
    public Dictionary<int, EnemyStats> dicEnemyStats;
    

    // Start is called before the first frame update
    void Awake()
    {
        
        dicTrack = new Dictionary<int, Dictionary<int, Floor>>();
        dicEnemyInfo = new Dictionary<int, List<Enemy>>();
        dicEnemyDeck = new Dictionary<int, Queue<GameObject>>();
        dicEnemyStats = new Dictionary<int, EnemyStats>();

    }
    
    public void LoadChapter(int chapterNumber)
    {
        
        // 맵 오브젝트 로드
        string map = "map" + chapterNumber.ToString();

        TextAsset textAsset = (TextAsset)Resources.Load(dataPath + map);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        LoadMap(xmlDoc.SelectNodes("MapInfo/Object"));

        // 맵 트랙 로드
        string track = "track" + chapterNumber.ToString();
        textAsset = (TextAsset)Resources.Load(dataPath + track);

        xmlDoc.LoadXml(textAsset.text);
        LoadTrack(xmlDoc.SelectNodes("TrackInfo/Track"));

        // 적 스탯 로드
        string enemyStats = "enemyStats" + chapterNumber.ToString();
        textAsset = (TextAsset)Resources.Load(dataPath + enemyStats);

        xmlDoc.LoadXml(textAsset.text);
        LoadEnemyStats(xmlDoc.SelectNodes("EnemyStats/Stats"));

        Debug.Log("Chapter " + chapterNumber.ToString() + " Load Success!");
    }


    void LoadMap(XmlNodeList nodeList)
    {
        foreach (XmlNode node in nodeList)
        {
            GameObject MapObject = Resources.Load("Map Resources/" + node.SelectSingleNode("Name").InnerText) as GameObject;
            Vector3 position = new Vector3(float.Parse(node.SelectSingleNode("X").InnerText),
                                           float.Parse(node.SelectSingleNode("Y").InnerText),
                                           float.Parse(node.SelectSingleNode("Z").InnerText));

            Vector3 rotation = new Vector3(float.Parse(node.SelectSingleNode("RotX").InnerText),
                                           float.Parse(node.SelectSingleNode("RotY").InnerText),
                                           float.Parse(node.SelectSingleNode("RotZ").InnerText));

            Vector3 scale = new Vector3(float.Parse(node.SelectSingleNode("ScaleX").InnerText),
                                        float.Parse(node.SelectSingleNode("ScaleY").InnerText),
                                        float.Parse(node.SelectSingleNode("ScaleZ").InnerText));

            GameObject newObject = Instantiate(MapObject, position, Quaternion.identity);
            newObject.name = newObject.name.Substring(0, newObject.name.Length - 7);

            newObject.transform.eulerAngles = rotation;
            newObject.transform.localScale = scale;
            newObject.transform.parent =  MapRoot.transform;

            newObject.tag = node.SelectSingleNode("Tag").InnerText;

            if (newObject.tag.Equals("Cube"))
            {
                newObject.AddComponent<BoxCollider>();
            }

        }
    }


    void LoadTrack(XmlNodeList nodeList)
    {
        Floor endFloor = new Floor(-1, -1, false, true, 0, 0);

        List<Floor> endFloorList = new List<Floor>();
        
        foreach (XmlNode node in nodeList)
        {
            Floor floor;

            floor.x = float.Parse(node.SelectSingleNode("X").InnerText);
            floor.z = float.Parse(node.SelectSingleNode("Z").InnerText);
            
            floor.trackNumber = int.Parse(node.SelectSingleNode("TrackNumber").InnerText);
            floor.floorNumber = int.Parse(node.SelectSingleNode("FloorNumber").InnerText);
            

            switch (node.SelectSingleNode("FloorName").InnerText)
            {
                case "StartFloor":
                    {
                        floor.isEnd = false;
                        floor.isStart = true;

                        // 출발점 장판의 트랙 번호가 기존에 없으면 추가
                        if (!(dicTrack.ContainsKey(floor.trackNumber)))
                        {
                            Dictionary<int, Floor> newTrack= new Dictionary<int, Floor>();
                            newTrack.Add(0, floor);
                            dicTrack.Add(floor.trackNumber, newTrack);

                        }
                        else
                        {
                            if(!(dicTrack[floor.trackNumber].ContainsKey(0)))
                            {
                                dicTrack[floor.trackNumber].Add(0, floor);
                            }
                        }

                        break;
                    }
                case "MiddleFloor":
                    {
                        floor.isEnd = false;
                        floor.isStart = false;

                        if (dicTrack.ContainsKey(floor.trackNumber))
                        {
                            // 해당 트랙 번호 딕셔너리에 장판 번호가 기존에 없으면 추가
                            if (!(dicTrack[floor.trackNumber].ContainsKey(floor.floorNumber + 1)))
                            {
                                dicTrack[floor.trackNumber].Add(floor.floorNumber + 1, floor);
                            }
                        }
                        // 중간 장판의 트랙 번호가 기존에 없으면 추가
                        else
                        {
                            Dictionary<int, Floor> newTrack = new Dictionary<int, Floor>();
                            newTrack.Add(floor.floorNumber + 1, floor);
                            dicTrack.Add(floor.trackNumber, newTrack);
                        }
                        break;
                    }
                case "EndFloor":
                    {
                        floor.isEnd = true;
                        floor.isStart = false;

                        endFloor.trackNumber = floor.trackNumber;
                        endFloor.floorNumber = floor.floorNumber;

                        endFloor.x = floor.x;
                        endFloor.z = floor.z;

                        endFloorList.Add(floor);
                        break;
                    }
            }
        }
     

        for(int i=0; i<endFloorList.Count; i++)
        {
            int trackNumber = endFloorList[i].trackNumber;
            if (dicTrack.ContainsKey(trackNumber))
            {
                dicTrack[trackNumber].Add(dicTrack[trackNumber].Count, endFloorList[i]);
            }
        }

       
    }

    void LoadEnemyStats(XmlNodeList nodeList)
    {

        foreach (XmlNode node in nodeList)
        {
            int enemyNo = int.Parse(node.SelectSingleNode("No").InnerText);
            EnemyStats enemyStats = new EnemyStats(float.Parse(node.SelectSingleNode("Hp").InnerText),
                                                   float.Parse(node.SelectSingleNode("Def").InnerText),
                                                   float.Parse(node.SelectSingleNode("Speed").InnerText));
            if (dicEnemyStats.ContainsKey(enemyNo))
            {
                dicEnemyStats[enemyNo] = enemyStats;
            }
            else
            {
                dicEnemyStats.Add(enemyNo, enemyStats);
            }
        }
    }

    public int LoadStage(int chapterNumber, int stageNumber)
    {

        string stage = "stage" + chapterNumber.ToString() + "_" + stageNumber.ToString();

        TextAsset textAsset = (TextAsset)Resources.Load("ChapterData/" + stage);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        InitStage();
        
        XmlNodeList nodeList = xmlDoc.SelectNodes("EnemyInfo/Enemy");

        foreach (XmlNode node in nodeList)
        {
            Enemy enemy = new Enemy(node.SelectSingleNode("EnemyName").InnerText,
                                    int.Parse(node.SelectSingleNode("TrackNumber").InnerText),
                                    float.Parse(node.SelectSingleNode("NextGap").InnerText));
            if (dicEnemyInfo.ContainsKey(enemy.trackNumber))
            {
                dicEnemyInfo[enemy.trackNumber].Add(enemy);
            }
            else
            {
                List<Enemy> enemyList = new List<Enemy>();
                enemyList.Add(enemy);

                dicEnemyInfo.Add(enemy.trackNumber, enemyList);
            }
        }

        Debug.Log(stage + " Load Success!");

        StageStart();

        return nodeList.Count;
    }

    public void InitStage()
    {

        foreach (KeyValuePair<int, List<Enemy>> enemyList in dicEnemyInfo)
        {
            enemyList.Value.Clear();
        }

        foreach (Transform enemy in EnemyRoot.transform)
        {
            Destroy(enemy.gameObject);
        }

        List<int> keyList = dicEnemyDeck.Keys.ToList();
        for (int i = 0; i < keyList.Count; i++)
        {
            for (int j = 0; j < dicEnemyDeck[keyList[i]].Count; j++)
            {
                Destroy(dicEnemyDeck[keyList[i]].Dequeue());
            }
        }
        dicEnemyDeck.Clear();



    }

    void StageStart()
    {
      
        List<int> keyList = dicEnemyInfo.Keys.ToList();

        for(int i=0; i<keyList.Count; i++)
        {
            foreach (Enemy enemy in dicEnemyInfo[keyList[i]])
            {
                int trackNumber = enemy.trackNumber;

                GameObject prefab_Enemy = Resources.Load("Character Resources/" + enemy.name) as GameObject;

                GameObject newEnemy = Instantiate(prefab_Enemy, Vector3.zero, Quaternion.identity, EnemyRoot.transform);

                BoxCollider boxCollider = newEnemy.AddComponent<BoxCollider>();
                boxCollider.center = Vector3.up * 6f;
                boxCollider.size = new Vector3(6f, 12f, 6f);
                boxCollider.isTrigger = true;

                GameEnemy gameEnemy = newEnemy.AddComponent<GameEnemy>();
                gameEnemy.trackNumber = trackNumber;
                gameEnemy.nextGap = enemy.nextGap;
                gameEnemy.dicTrack = dicTrack[trackNumber];

                int no = (int)Enum.Parse(typeof(EnemyKind), enemy.name.ToUpper());

                if(dicEnemyStats.ContainsKey(no))
                {
                    gameEnemy.stats = dicEnemyStats[no];
                }
                else // 딕셔너리에 스텟 데이터가 없으면 기본값
                {
                    gameEnemy.stats = new EnemyStats(10f, 1f, 2f);
                }

                //해당 트랙의 enemy덱이 존재한다면 push
                if (dicEnemyDeck.ContainsKey(trackNumber))
                {
                    dicEnemyDeck[trackNumber].Enqueue(newEnemy);
                }
                //해당 트랙의 enemy덱이 존재하지 않다면 스택 새로 만들고 push한 후 dictionary에 추가
                else
                {
                    Queue<GameObject> newEnemyDeck = new Queue<GameObject>();
                    newEnemyDeck.Enqueue(newEnemy);

                    dicEnemyDeck.Add(trackNumber, newEnemyDeck);
                }

            }
        }

        Debug.Log("Stage Start!!");
    }
}
