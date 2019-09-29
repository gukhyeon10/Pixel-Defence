using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class GameDataManager : MonoBehaviour
{
    const string dataPath = "ChapterData/";

    public Dictionary<int, Dictionary<int, Floor>> dicTrack;

    public List<Enemy> listEnemy;
    public Dictionary<int, EnemyStats> dicEnemyStats;

    // Start is called before the first frame update
    void Start()
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


        // 맵 트랙 로드
        string track = "track" + chapterNumber.ToString();
        textAsset = (TextAsset)Resources.Load(dataPath + track);

        xmlDoc.LoadXml(textAsset.text);

        Debug.Log("Chapter "+ chapterNumber.ToString() + " Load Success!");


        // 적 스탯 로드
        string enemyStats = "enemyStats" + chapterNumber.ToString();
        textAsset = (TextAsset)Resources.Load(dataPath + enemyStats);

        xmlDoc.LoadXml(textAsset.text);

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




}
