using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml;
using System.Linq;

public class LoginManager : MonoBehaviour
{
    
    [SerializeField]
    UIAnchor Anchor_Login;

    [SerializeField]
    UIAnchor Anchor_Join;

    [SerializeField]
    UILabel Label_LoginMessage;

    [SerializeField]
    UILabel Label_JoinMessage;

    UserDataManager userDataManager;

    // Start is called before the first frame update
    void Start()
    {
        userDataManager = UserDataManager.Instance;
    }
    
    // 아이디 비밀번호 대조 
    public void Login(UIInput id, UIInput pw)
    {
        if(userDataManager.dicUserList.ContainsKey(id.value))
        {
            if(userDataManager.dicUserList[id.value].pw.Equals(pw.value))
            {
                Label_LoginMessage.text = "로그인 성공!";
                userDataManager.userName = id.value;
                userDataManager.chapterLimit = userDataManager.dicUserList[id.value].chapterLimit;
                userDataManager.chapterCurrent = userDataManager.chapterLimit;
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

    // 회원가입
    public void JoinUs(UILabel id, UIInput pw, UIInput pwCheck)  // 로컬디비 추가
    {
        if (userDataManager.dicUserList.ContainsKey(id.text))
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
        userDataManager.dicUserList.Add(id.text, newUser);

        PanelChange();

        userDataManager.SaveUserList();

        Label_JoinMessage.text = "";
        id.text = "ID";
        pw.value = "";
        pwCheck.value = "";
    }

    // 창 전환
    public void PanelChange()
    {
        Anchor_Join.gameObject.SetActive(!Anchor_Join.gameObject.activeSelf);
        Anchor_Login.gameObject.SetActive(!Anchor_Login.gameObject.activeSelf);
    }

    
    //종료
    public void Exit()
    {
        userDataManager.SaveUserList();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}
