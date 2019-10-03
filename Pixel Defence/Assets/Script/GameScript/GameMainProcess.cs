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


    int stageNumber = 1;

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
        }

        SkyBox.transform.Rotate(Vector3.up * Time.deltaTime);
    }

    public void NextStageStart()
    {
        button_Start.SetActive(false);

        StopAllCoroutines();

        if (UserDataManager.Instance == null)
        {
            Debug.Log("UserData 객체 NULL");

            gameDataManager.LoadStage(1, stageNumber++);
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

        gameDataManager.InitStage();
        button_Start.SetActive(true);
    }

}
