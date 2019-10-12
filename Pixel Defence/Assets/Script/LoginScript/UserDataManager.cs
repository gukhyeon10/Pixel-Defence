using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Linq;

public class UserDataManager : MonoBehaviour
{
    public string userName;
    public int userLevel;
    public float playTime;
    public int chapterLimit;
    public int chapterCurrent;

    public Dictionary<string, User> dicUserList;

    static private UserDataManager _instance = null;

    static public UserDataManager Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        dicUserList = new Dictionary<string, User>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.userName = "QA";
        this.userLevel = -1;
        this.playTime = 0f;
        this.chapterLimit = 3;
        this.chapterCurrent = 1;

        LoadUserList();
    }

    public void LoadUserList()
    {
        string strPath = Application.streamingAssetsPath + "/User/UserList.xml";

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(strPath);

        XmlNodeList UserList = xmlDoc.SelectNodes("UserList/User");

        foreach (XmlNode User in UserList)
        {
            User user;

            user.name = User.SelectSingleNode("ID").InnerText;
            user.pw = User.SelectSingleNode("PW").InnerText;
            user.chapterLimit = int.Parse(User.SelectSingleNode("Chapter").InnerText);
            dicUserList.Add(user.name, user);
        }
    }

    public void SaveUserList()
    {
        string strPath = string.Empty;
        strPath += Application.streamingAssetsPath + "/User/UserList.xml";

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));

        XmlNode root = xmlDoc.CreateNode(XmlNodeType.Element, "UserList", string.Empty);
        xmlDoc.AppendChild(root);

        List<string> Keys = dicUserList.Keys.ToList();

        for (int i = 0; i < Keys.Count; i++)
        {
            XmlNode user = xmlDoc.CreateNode(XmlNodeType.Element, "User", string.Empty);
            root.AppendChild(user);

            XmlElement userId = xmlDoc.CreateElement("ID");
            userId.InnerText = Keys[i];
            user.AppendChild(userId);

            XmlElement userPw = xmlDoc.CreateElement("PW");
            userPw.InnerText = dicUserList[Keys[i]].pw;
            user.AppendChild(userPw);

            XmlElement userChapter = xmlDoc.CreateElement("Chapter");
            userChapter.InnerText = dicUserList[Keys[i]].chapterLimit.ToString();
            user.AppendChild(userChapter);

        }

        xmlDoc.Save(strPath);
    }
    
}
