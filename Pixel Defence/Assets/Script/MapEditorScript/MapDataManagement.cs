using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;

public class MapDataManagement : MonoBehaviour {

    [SerializeField]
    GameObject MapRoot;

    public void OpenLoadFile()     // 불러올 맵 파일 
    {
        string filePath = "";
#if UNITY_EDITOR
        filePath = EditorUtility.OpenFilePanel("Open Map File Dialog"
                                            , Application.dataPath
                                            , "xml");
#endif
        if (filePath.Length != 0)  // 파일 선택
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            foreach (Transform child in MapRoot.transform)  // 모든 오브젝트 삭제
            {
                Destroy(child.gameObject);
            }

            XmlNodeList nodeList = xmlDoc.SelectNodes("MapInfo/Object");
            
            foreach (XmlNode node in nodeList)
            {
                GameObject MapObject = Resources.Load("Map Resources/" + node.SelectSingleNode("Name").InnerText) as GameObject;
                Vector3 position = new Vector3(float.Parse(node.SelectSingleNode("X").InnerText),
                                               float.Parse(node.SelectSingleNode("Y").InnerText),
                                               float.Parse(node.SelectSingleNode("Z").InnerText));
                Vector3 rotation = new Vector3(0, float.Parse(node.SelectSingleNode("R").InnerText), 0);

                Vector3 scale = new Vector3(float.Parse(node.SelectSingleNode("ScaleX").InnerText),
                                            float.Parse(node.SelectSingleNode("ScaleY").InnerText),
                                            float.Parse(node.SelectSingleNode("ScaleZ").InnerText));

                GameObject newObject = Instantiate(MapObject, position, Quaternion.identity);
                newObject.name = newObject.name.Substring(0, newObject.name.Length - 7);

                newObject.transform.eulerAngles = rotation;
                newObject.transform.parent = MapRoot.transform;

                newObject.tag = node.SelectSingleNode("Tag").InnerText;

                if(newObject.tag.Equals("Cube"))
                {
                    newObject.AddComponent<BoxCollider>();
                }
                
            }
            
            Debug.Log("Map Xml File  Load Succes!");

        }

    }

    public void OpenSaveFile()    //저장할 맵 파일
    {
        string filePath = "";
#if UNITY_EDITOR
        filePath = EditorUtility.SaveFilePanel("Save Map File Dialog"
                                   , Application.dataPath
                                   , ""
                                   , "xml");
#endif
        if (filePath.Length != 0)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));

            XmlNode root = xmlDoc.CreateNode(XmlNodeType.Element, "MapInfo", string.Empty);
            xmlDoc.AppendChild(root);


            for (int i = 0; i < MapRoot.transform.childCount; i++)
            {
                XmlNode childNode = xmlDoc.CreateNode(XmlNodeType.Element, "Object", string.Empty);
                root.AppendChild(childNode);

                Transform mapChildObject = MapRoot.transform.GetChild(i);

                XmlElement name = xmlDoc.CreateElement("Name");
                name.InnerText = mapChildObject.name;
                childNode.AppendChild(name);

                XmlElement x = xmlDoc.CreateElement("X");
                x.InnerText = mapChildObject.position.x.ToString();
                childNode.AppendChild(x);

                XmlElement y = xmlDoc.CreateElement("Y");
                y.InnerText = mapChildObject.position.y.ToString();
                childNode.AppendChild(y);

                XmlElement z = xmlDoc.CreateElement("Z");
                z.InnerText = mapChildObject.position.z.ToString();
                childNode.AppendChild(z);

                XmlElement rotation = xmlDoc.CreateElement("R");
                rotation.InnerText = mapChildObject.eulerAngles.y.ToString();
                childNode.AppendChild(rotation);

                XmlElement scaleX = xmlDoc.CreateElement("ScaleX");
                scaleX.InnerText = mapChildObject.localScale.x.ToString();
                childNode.AppendChild(scaleX);

                XmlElement scaleY = xmlDoc.CreateElement("ScaleY");
                scaleY.InnerText = mapChildObject.localScale.y.ToString();
                childNode.AppendChild(scaleY);

                XmlElement scaleZ = xmlDoc.CreateElement("ScaleZ");
                scaleZ.InnerText = mapChildObject.localScale.z.ToString();
                childNode.AppendChild(scaleZ);

                XmlElement tag = xmlDoc.CreateElement("Tag");
                tag.InnerText = mapChildObject.tag;
                childNode.AppendChild(tag);

            }
            
            xmlDoc.Save(filePath);
            Debug.Log("Map Xml File  Save Succes!");

        }
    }
}
