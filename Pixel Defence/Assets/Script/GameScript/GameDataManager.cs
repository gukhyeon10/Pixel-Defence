using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class GameDataManager : MonoBehaviour
{
    const string dataPath = "ChapterData/";

    [SerializeField]
    GameObject MapRoot;

    public Dictionary<int, Dictionary<int, Floor>> dicTrack;
    public List<Enemy> listEnemy;
    public Dictionary<int, EnemyStats> dicEnemyStats;

    // Start is called before the first frame update
    void Awake()
    {
        dicTrack = new Dictionary<int, Dictionary<int, Floor>>();
        listEnemy = new List<Enemy>();
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

    public void LoadStage(int chapterNumber, int stageNumber)
    {

        string stage = "stage" + chapterNumber.ToString() + "_" + stageNumber.ToString();

        TextAsset textAsset = (TextAsset)Resources.Load("ChapterData/" + stage);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        Debug.Log(stage + " Load Success!");
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
        int maxFloorNumber = 0;
        foreach (XmlNode node in nodeList)
        {
            Floor floor;

            floor.x = float.Parse(node.SelectSingleNode("X").InnerText);
            floor.y = float.Parse(node.SelectSingleNode("Z").InnerText);
            
            floor.trackNumber = int.Parse(node.SelectSingleNode("TrackNumber").InnerText);
            floor.floorNumber = int.Parse(node.SelectSingleNode("FloorNumber").InnerText);

            if(floor.floorNumber > maxFloorNumber)
            {
                maxFloorNumber = floor.floorNumber;
            }

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

                        endFloor.trackNumber = floor.trackNumber;
                        endFloor.floorNumber = floor.floorNumber;

                        endFloor.x = floor.x;
                        endFloor.y = floor.y;

                        break;
                    }
            }
        }

        if(endFloor.trackNumber >= 0 && dicTrack.ContainsKey(endFloor.trackNumber))
        {
            dicTrack[endFloor.trackNumber].Add(maxFloorNumber+2, endFloor);
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
                Debug.Log("Enemy Stats Ditionary Key Lost -> Not Update");
            }
        }
    }
}
