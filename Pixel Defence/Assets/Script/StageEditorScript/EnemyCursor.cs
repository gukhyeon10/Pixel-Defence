using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCursor : MonoBehaviour
{
    [SerializeField]
    UIGrid gb_EnemyStack;
    [SerializeField]
    UIScrollView scrollView_EnemyStack;
    [SerializeField]
    UILabel label_TrackNumber;

    [SerializeField]
    GameObject[] SelectionPrefab;

    [SerializeField]
    UIInput input_NextGap;

    public Dictionary<int, List<Enemy>> dicEnemyDeck;

    public  bool isEnemyCursor = false;

    public int trackNumber = 0;
    // Start is called before the first frame update

    void Awake()
    {
        dicEnemyDeck = new Dictionary<int, List<Enemy>>();
        for(int i= 0; i<10; i++)
        {
            dicEnemyDeck.Add(i, new List<Enemy>());
        }
    }

    void Update()
    {
        if(isEnemyCursor)
        {
            KeyInput();
        }
        
    }

    void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            gb_EnemyStack.Reposition();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            trackNumber = 0;
            ChangeEnemyStack();
            label_TrackNumber.text = "0 TRACK";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            trackNumber = 1;
            ChangeEnemyStack();
            label_TrackNumber.text = "1 TRACK";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            trackNumber = 2;
            ChangeEnemyStack();
            label_TrackNumber.text = "2 TRACK";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            trackNumber = 3;
            ChangeEnemyStack();
            label_TrackNumber.text = "3 TRACK";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            trackNumber = 4;
            ChangeEnemyStack();
            label_TrackNumber.text = "4 TRACK";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            trackNumber = 5;
            ChangeEnemyStack();
            label_TrackNumber.text = "5 TRACK";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            trackNumber = 6;
            ChangeEnemyStack();
            label_TrackNumber.text = "6 TRACK";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            trackNumber = 7;
            ChangeEnemyStack();
            label_TrackNumber.text = "7 TRACK";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            trackNumber = 8;
            ChangeEnemyStack();
            label_TrackNumber.text = "8 TRACK";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            trackNumber = 9;
            ChangeEnemyStack();
            label_TrackNumber.text = "9 TRACK";
        }
    }

    public void PushEnemy(GameObject prefab_Enemy)
    {
        if(isEnemyCursor)
        {
            //enemyStack에 있는 프리팹을 더블 클릭하면 해당 프리팹 삭제
            if(prefab_Enemy.transform.parent == gb_EnemyStack.transform)
            {
               for(int i = 0; i< gb_EnemyStack.transform.childCount; i++)
                {
                    if(gb_EnemyStack.GetChild(i) == prefab_Enemy.transform)
                    {
                        dicEnemyDeck[trackNumber].RemoveAt(i);
                        break;
                    }
                }

                Destroy(prefab_Enemy.gameObject);
                StartCoroutine(RepositionCorutine());
            }
            else  // enemySelection 이면 enemyStack에 추가
            {
                GameObject newEnemy = Instantiate(prefab_Enemy, Vector3.zero, Quaternion.identity, gb_EnemyStack.transform);
                newEnemy.transform.eulerAngles = Vector3.up * 180f; // 앞을 보게끔
                newEnemy.GetComponent<UIDragScrollView>().scrollView = scrollView_EnemyStack;
                newEnemy.name = newEnemy.name.Substring(0, newEnemy.name.Length - 7);
                
                
                if (dicEnemyDeck.ContainsKey(trackNumber))
                {
                    dicEnemyDeck[trackNumber].Add(new Enemy(newEnemy.name, trackNumber, float.Parse(input_NextGap.value)));
                }


                gb_EnemyStack.Reposition();
            }
        }
    }


    // 이상하게 삭제하고나서는 grid가 reposition이 안됨
    IEnumerator RepositionCorutine()
    {
        yield return null;
        gb_EnemyStack.Reposition();
    }

    void ChangeEnemyStack()
    {
        NGUITools.DestroyChildren(gb_EnemyStack.transform);
        foreach(Enemy deckEnemy in dicEnemyDeck[trackNumber])
        {
            foreach(GameObject prefab in SelectionPrefab)
            {
                if(prefab.name.Equals(deckEnemy.name))
                {
                    GameObject newEnemy = Instantiate(prefab, Vector3.zero, Quaternion.identity, gb_EnemyStack.transform);
                    newEnemy.transform.eulerAngles = Vector3.up * 180f; // 앞을 보게끔
                    newEnemy.GetComponent<UIDragScrollView>().scrollView = scrollView_EnemyStack;
                    newEnemy.name = newEnemy.name.Substring(0, newEnemy.name.Length - 7);
                    
                    break;
                }
            }
            
        }

        gb_EnemyStack.Reposition();
    }
}
