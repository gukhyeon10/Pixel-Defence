using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainProcess : MonoBehaviour
{
    public static int totalEnemy;

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
    UILabel Label_Stage;
    [SerializeField]
    UILabel Label_Money;

    const int maxStageNumber = 10;
    int stageNumber = 1;

    public int money = 0;
    public float moneySpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if(UserDataManager.Instance == null)
        {
            Debug.Log("UserData 객체 NULL");

            gameDataManager.LoadChapter(1);
        }
        else
        {
            int chapterNumber = UserDataManager.Instance.chapterCurrent;
            if (chapterNumber <= 0)
            {
                Debug.Log("chapter 번호 값 갱신 실패");
                return;
            }

            gameDataManager.LoadChapter(chapterNumber);
        }
        Label_Stage.text = "STAGE "+stageNumber.ToString();
    }

    private void Update()
    {
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

        SkyBox.transform.Rotate(Vector3.up * Time.deltaTime);
        Label_Money.text = money.ToString();
    }

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

        Debug.Log("Stage END!");
        for (int i = 0; i < UnitRoot.transform.childCount; i++)
        {
            UnitRoot.transform.GetChild(i).GetComponent<UnitScript>().list_Enemy.Clear();
        }

        totalEnemy = 0;

        gameDataManager.InitStage();

        if(stageNumber > maxStageNumber)
        {
            Debug.Log("Chapter CLEAR!!");
            User userData = UserDataManager.Instance.dicUserList[UserDataManager.Instance.userName];
            if(userData.chapterLimit <= UserDataManager.Instance.chapterCurrent)
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

    IEnumerator MoneyUp()
    {
        while(true)
        {
            money++;
            yield return new WaitForSeconds(moneySpeed);
        }
    }

}
