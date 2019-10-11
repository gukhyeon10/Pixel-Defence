using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml;
using System.Linq;

public class LoginManager : MonoBehaviour
{
    Dictionary<string, User> dicUserList;
    
    [SerializeField]
    UIAnchor Anchor_Login;

    [SerializeField]
    UIAnchor Anchor_Join;

    [SerializeField]
    UILabel Label_LoginMessage;

    [SerializeField]
    UILabel Label_JoinMessage;

    // Start is called before the first frame update
    void Start()
    {
        dicUserList = new Dictionary<string, User>();
        LoadUserList();
    }

    void LoadUserList()
    {
        string strPath = Application.streamingAssetsPath + "/User/UserList.xml";

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(strPath);

        XmlNodeList UserList = xmlDoc.SelectNodes("UserList/User");

        foreach(XmlNode User in UserList)
        {
            User user;

            user.name = User.SelectSingleNode("ID").InnerText;
            user.pw = User.SelectSingleNode("PW").InnerText;
            user.chapterLimit = int.Parse(User.SelectSingleNode("Chapter").InnerText);
            dicUserList.Add(user.name, user);
        }

    }
    
    public void Login(UIInput id, UIInput pw)
    {
        if(dicUserList.ContainsKey(id.value))
        {
            if(dicUserList[id.value].pw.Equals(pw.value))
            {
                Label_LoginMessage.text = "로그인 성공!";
                UserDataManager.Instance.dicUserList = this.dicUserList;
                UserDataManager.Instance.userName = id.value;
                UserDataManager.Instance.chapterLimit = this.dicUserList[id.value].chapterLimit;
                SceneManager.LoadScene("Chapter Scene");
            }
            else
            {
                Label_LoginMessage.text = "비밀번호가 일치하지 않습니다.";
            }
        }
        else
        {
            Label_LoginMessage.text = "존재하지 않는 아이디입니다.";
        }

    }

    public void JoinUs(UILabel id, UIInput pw, UIInput pwCheck)  // 로컬디비 추가
    {
        if (dicUserList.ContainsKey(id.text))
        {
            Label_JoinMessage.text = "이미 있는 아이디 입니다.";
            return;
        }

        if (! pw.value.Equals(pwCheck.value))
        {
            Label_JoinMessage.text = "비밀번호가 일치한지 확인해주세요.";
            return;
        }

        User newUser;
        newUser.name = id.text;
        newUser.pw = pw.value;
        newUser.chapterLimit = 1;
        dicUserList.Add(id.text, newUser);

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

        PanelChange();

        Label_JoinMessage.text = "";
        id.text = "ID";
        pw.value = "";
        pwCheck.value = "";
    }

    public void PanelChange()
    {
        Anchor_Join.gameObject.SetActive(!Anchor_Join.gameObject.activeSelf);
        Anchor_Login.gameObject.SetActive(!Anchor_Login.gameObject.activeSelf);
    }


}
