using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public string userName;
    public int userLevel;
    public float playTime;
    public int chapterLimit;
    public int chapterCurrent;
    public int money;

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
        DontDestroyOnLoad(this.gameObject);

        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        this.userName = "QA";
        this.userLevel = 1;
        this.playTime = 0f;
        this.chapterLimit = 1;
        this.chapterCurrent = 0;
        this.money = 100;
    }
    
}
