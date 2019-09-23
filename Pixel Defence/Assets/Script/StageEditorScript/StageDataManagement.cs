using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using System.Linq;
using UnityEngine;

public class StageDataManagement : MonoBehaviour
{
    [SerializeField]
    GameObject FloorRoot;
    [SerializeField]
    FloorCursor floorCusor;

    [SerializeField]
    EnemyCursor enemyCursor;

    public void LoadTrack()
    {
        string filePath = "";
#if UNITY_EDITOR
        filePath = EditorUtility.OpenFilePanel("Open Track File Dialog"
                                            , Application.dataPath
                                            , "xml");
#endif
        if (filePath.Length != 0)  // 파일 선택
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            foreach (Transform child in FloorRoot.transform)  // 모든 오브젝트 삭제
            {
                Destroy(child.gameObject);
            }

            XmlNodeList nodeList = xmlDoc.SelectNodes("TrackInfo/Track");

            foreach (XmlNode node in nodeList)
            {
                GameObject floorObject = Resources.Load("Floor Resources/" + node.SelectSingleNode("FloorName").InnerText) as GameObject;

                Vector3 position = new Vector3(float.Parse(node.SelectSingleNode("X").InnerText),
                                               float.Parse(node.SelectSingleNode("Y").InnerText),
                                               float.Parse(node.SelectSingleNode("Z").InnerText));

                GameObject newFloor = Instantiate(floorObject, position, Quaternion.identity, FloorRoot.transform);

                newFloor.name = newFloor.name.Substring(0, newFloor.name.Length - 7);
                newFloor.transform.eulerAngles = new Vector3(90f, 0f, 0f);

                if (!(newFloor.name.Equals("MiddleFloor")))
                {
                    newFloor.transform.eulerAngles *= -1f;
                }

                FloorScript floorScript = newFloor.AddComponent<FloorScript>();
                floorScript.trackNumber = int.Parse(node.SelectSingleNode("TrackNumber").InnerText);
                floorScript.floorNumber = int.Parse(node.SelectSingleNode("FloorNumber").InnerText);
            }

            Debug.Log("Floor Xml File  Load Succes!");

        }

    }

    public void SaveTrack()
    {
        string filePath = "";
#if UNITY_EDITOR
        filePath = EditorUtility.SaveFilePanel("Save Track File Dialog"
                                   , Application.dataPath
                                   , "track"
                                   , "xml");
#endif
        if (filePath.Length != 0)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));

            XmlNode root = xmlDoc.CreateNode(XmlNodeType.Element, "TrackInfo", string.Empty);
            xmlDoc.AppendChild(root);


            for (int i = 0; i < FloorRoot.transform.childCount; i++)
            {
                XmlNode childNode = xmlDoc.CreateNode(XmlNodeType.Element, "Track", string.Empty);
                root.AppendChild(childNode);

                Transform floorChildObject = FloorRoot.transform.GetChild(i);

                XmlElement floorName = xmlDoc.CreateElement("FloorName");
                floorName.InnerText = floorChildObject.name;
                childNode.AppendChild(floorName);

                XmlElement trackNumber = xmlDoc.CreateElement("TrackNumber");
                trackNumber.InnerText = floorChildObject.GetComponent<FloorScript>().trackNumber.ToString();
                childNode.AppendChild(trackNumber);

                XmlElement floorNumber = xmlDoc.CreateElement("FloorNumber");
                floorNumber.InnerText = floorChildObject.GetComponent<FloorScript>().floorNumber.ToString();
                childNode.AppendChild(floorNumber);

                XmlElement x = xmlDoc.CreateElement("X");
                x.InnerText = floorChildObject.position.x.ToString();
                childNode.AppendChild(x);

                XmlElement y = xmlDoc.CreateElement("Y");
                y.InnerText = floorChildObject.position.y.ToString();
                childNode.AppendChild(y);

                XmlElement z = xmlDoc.CreateElement("Z");
                z.InnerText = floorChildObject.position.z.ToString();
                childNode.AppendChild(z);
            }

            xmlDoc.Save(filePath);
            Debug.Log("Track Xml File  Save Succes!");

        }
    
}
    
    public void LoadDeck()
    {
        string filePath = "";
#if UNITY_EDITOR
        filePath = EditorUtility.OpenFilePanel("Open Deck File Dialog"
                                            , Application.dataPath
                                            , "xml");
#endif
        if (filePath.Length != 0)  // 파일 선택
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            Dictionary<int, List<Enemy>> dicEnemyDeck = enemyCursor.dicEnemyDeck;

            foreach(KeyValuePair<int, List<Enemy>> enemyList in dicEnemyDeck)
            {
                enemyList.Value.Clear();
            }

            XmlNodeList nodeList = xmlDoc.SelectNodes("EnemyInfo/Enemy");

            foreach (XmlNode node in nodeList)
            {
                Enemy enemy = new Enemy(node.SelectSingleNode("EnemyName").InnerText,
                                        int.Parse(node.SelectSingleNode("TrackNumber").InnerText),
                                        float.Parse(node.SelectSingleNode("NextGap").InnerText));
                if (dicEnemyDeck.ContainsKey(enemy.trackNumber))
                {
                    dicEnemyDeck[enemy.trackNumber].Add(enemy);
                }
                else
                {
                    List<Enemy> enemyList = new List<Enemy>();
                    enemyList.Add(enemy);

                    dicEnemyDeck.Add(enemy.trackNumber, enemyList);
                }
            }

            enemyCursor.UpdateEnemyStack();
            Debug.Log("Enemy Xml File  Load Succes!");

        }

    }

    public void SaveDeck()
    {
        string filePath = "";
#if UNITY_EDITOR
        filePath = EditorUtility.SaveFilePanel("Save Deck File Dialog"
                                   , Application.dataPath
                                   , "deck"
                                   , "xml");
#endif
        if (filePath.Length != 0)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));

            XmlNode root = xmlDoc.CreateNode(XmlNodeType.Element, "EnemyInfo", string.Empty);
            xmlDoc.AppendChild(root);

            Dictionary<int, List<Enemy>> dicEnemyDeck = enemyCursor.dicEnemyDeck;

            for(int i=0; i<dicEnemyDeck.Count; i++)
            {
                List<Enemy> enemyDeck = dicEnemyDeck[dicEnemyDeck.Keys.ToList()[i]];
                for(int j = 0; j<enemyDeck.Count; j++)
                {
                    XmlNode childNode = xmlDoc.CreateNode(XmlNodeType.Element, "Enemy", string.Empty);
                    root.AppendChild(childNode);

                    XmlElement enemyName = xmlDoc.CreateElement("EnemyName");
                    enemyName.InnerText = enemyDeck[j].name;
                    childNode.AppendChild(enemyName);

                    XmlElement trackNumber = xmlDoc.CreateElement("TrackNumber");
                    trackNumber.InnerText = enemyDeck[j].trackNumber.ToString();
                    childNode.AppendChild(trackNumber);

                    XmlElement floorNumber = xmlDoc.CreateElement("NextGap");
                    floorNumber.InnerText = enemyDeck[j].nextGap.ToString();
                    childNode.AppendChild(floorNumber);
                }
            }
            
            xmlDoc.Save(filePath);
            Debug.Log("Enemy Xml File  Save Succes!");

        }
    }
}
