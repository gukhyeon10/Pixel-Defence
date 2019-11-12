using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainProcess : MonoBehaviour
{
    static private GameMainProcess _instance = null;

    static public GameMainProcess Instance
    {
        get
        {
            return _instance;
        }
    }
    public int totalEnemy;

    [SerializeField]
    GameDataManager gameDataManager;

    [SerializeField]
    GameObject button_Start;

    [SerializeField]
    GameObject SkyBox;

    [SerializeField]
    GameObject EnemyRoot;
    [SerializeField]
    GameObject UnitRoot;

    [SerializeField]
    GameObject ClearWindow;
    [SerializeField]
    GameObject GameOverWindow;

    [SerializeField]
    UILabel Label_Stage;
    [SerializeField]
    UILabel Label_Money;
    [SerializeField]
    UILabel Label_Life;

    [SerializeField]
    AudioSource Audio_BGM;
    [SerializeField]
    AudioClip[] Audio_Clip;

    const int maxChapterNumber = 4;
    const int maxStageNumber = 5;

    int stageNumber = 1;

    public int life = 50;
    public int money = 0;
    public float moneySpeed = 1f;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        if(UserDataManager.Instance == null)
        {
            Debug.Log("UserData 객체 NULL");

            CameraInit(1);
            gameDataManager.LoadChapter(1);
            Audio_BGM.clip = Audio_Clip[0];
            Audio_BGM.Play();
        }
        else
        {
            int chapterNumber = UserDataManager.Instance.chapterCurrent;
            if (chapterNumber <= 0)
            {
                Debug.Log("chapter 번호 값 갱신 실패");
                return;
            }

            CameraInit(chapterNumber);
            gameDataManager.LoadChapter(chapterNumber);

            this.money += chapterNumber * 30;

            Audio_BGM.clip = Audio_Clip[chapterNumber - 1];
            Audio_BGM.Play();

        }
        Label_Stage.text = "STAGE "+stageNumber.ToString();
    }


    // 카메라 위치 및 회전값 챕터 별 초기화
    void CameraInit(int chapterNumber)
    {
        Vector3 cameraPosition = Vector3.zero;
        Vector3 cameraEuler = Vector3.zero;
        switch (chapterNumber)
        {
            case 1:
                {
                    cameraPosition = new Vector3(3f, 20f, -20f);
                    cameraEuler = new Vector3(48f, 0f, 0f);
                    break;
                }
            case 2:
                {
                    cameraPosition = new Vector3(32f, 20f, 0f);
                    cameraEuler = new Vector3(42f, -50f, 0f);
                    break;
                }
            case 3:
                {
                    cameraPosition = new Vector3(17f, 17f, 20f);
                    cameraEuler = new Vector3(37f, -140f, 0f);
                    break;
                }
            case 4:
                {
                    cameraPosition = new Vector3(-1f, 24f, -27f);
                    cameraEuler = new Vector3(41f, 0f, 0f);
                    break;
                }
        }

        Camera.main.transform.position = cameraPosition;
        Camera.main.transform.eulerAngles = cameraEuler;

    }

    void Update()
    {
        //임시 스테이지 스킵 버튼
        if(Input.GetKeyDown(KeyCode.Insert))
        {
            for (int i = 0; i < UnitRoot.transform.childCount; i++)
            {
                UnitRoot.transform.GetChild(i).GetComponent<UnitScript>().list_Enemy.Clear();
            }

            StopAllCoroutines();
            gameDataManager.InitStage();
            button_Start.SetActive(true);

            totalEnemy = 0;
            Label_Stage.text = "STAGE " + stageNumber.ToString();
        }

        if(Input.GetKeyDown(KeyCode.F5))
        {
            if(Time.timeScale == 1f)
            {
                Time.timeScale = 2f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        //하늘 회전 및 UI갱신
        SkyBox.transform.Rotate(Vector3.up * Time.deltaTime);
        Label_Life.text = life.ToString();
        Label_Money.text = money.ToString();

        // 게임오버
        if(life <= 0)
        {
            GameOverWindow.gameObject.SetActive(true);
            Time.timeScale = 1f;
        }
    }

    //다음 스테이지 시작
    public void NextStageStart()
    {
        button_Start.SetActive(false);

        StopAllCoroutines();

        if (UserDataManager.Instance == null)
        {
            Debug.Log("UserData 객체 NULL");

            totalEnemy = gameDataManager.LoadStage(1, stageNumber++);
            Debug.Log("total enemey = " + totalEnemy.ToString());
        }
        else
        {
            totalEnemy = gameDataManager.LoadStage(UserDataManager.Instance.chapterCurrent, stageNumber++);
            Debug.Log("total enemey = " + totalEnemy.ToString());
        }

        foreach (KeyValuePair<int, Queue<GameObject>> enemyDeck in gameDataManager.dicEnemyDeck)
        {
            StartCoroutine(StagePlay(enemyDeck.Value));
        }
        StartCoroutine(MoneyUp());

        for(int i= 0; i<UnitRoot.transform.childCount; i++)
        {
            UnitRoot.transform.GetChild(i).GetComponent<UnitScript>().EnemyLoad();
        }
    }

    //스테이지 진행
    IEnumerator StagePlay(Queue<GameObject> enemyDeck)
    {
        yield return null;
        foreach (GameObject enemy in enemyDeck)
        {
            enemy.GetComponent<GameEnemy>().EnemyStart();
            yield return new WaitForSeconds(enemy.GetComponent<GameEnemy>().nextGap);
        }
        
        while(totalEnemy > 0)
        {
            yield return null;
        }

        if(life > 0)
        {
            StartCoroutine(StageClear());
        }
        else
        {
            StopAllCoroutines();
        }

    }

    // 스테이지 끝나고 적 사라지는 모션 끝나는거 기다리도록 코루틴 실행
    IEnumerator StageClear()
    {
        yield return new WaitForSeconds(1.5f);


        Debug.Log("Stage END!");
        for (int i = 0; i < UnitRoot.transform.childCount; i++)
        {
            UnitRoot.transform.GetChild(i).GetComponent<UnitScript>().list_Enemy.Clear();
        }

        totalEnemy = 0;

        gameDataManager.InitStage();

        if (stageNumber > maxStageNumber)
        {
            Time.timeScale = 1f;

            Debug.Log("Chapter CLEAR!!");
            User userData = UserDataManager.Instance.dicUserList[UserDataManager.Instance.userName];
            if (userData.chapterLimit < maxChapterNumber && userData.chapterLimit <= UserDataManager.Instance.chapterCurrent)
            {
                userData.chapterLimit++;
                UserDataManager.Instance.dicUserList[userData.name] = userData;

                UserDataManager.Instance.chapterLimit = userData.chapterLimit;

                UserDataManager.Instance.chapterCurrent += 1;

                UserDataManager.Instance.SaveUserList();
            }

            ClearWindow.gameObject.SetActive(true);
        }
        else
        {
            button_Start.SetActive(true);
            Label_Stage.text = "STAGE " + stageNumber.ToString();
        }

        StopAllCoroutines();
    }

    // 돈 쌓이도록
    IEnumerator MoneyUp()
    {
        while(true)
        {
            money++;
            yield return new WaitForSeconds(moneySpeed / 10f);
        }
    }

}
