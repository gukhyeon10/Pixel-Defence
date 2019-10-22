using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] chapter;
    [SerializeField]
    GameObject chapterRoot;

    [SerializeField]
    GameObject cloud;

    [SerializeField]
    GameObject EnterButton;
    [SerializeField]
    GameObject LockButton;

    [SerializeField]
    UILabel label_ChapterNumber;

    [SerializeField]
    UILabel label_UserName;
    [SerializeField]
    UILabel label_Level;

    float chapterGap;

    int chapterNumber = 1;

    bool isChange = false;

    public float changeSpeed = 55f;
    

    // Start is called before the first frame update
    void Start()
    {
        chapterGap = (float)Screen.width;

        Vector3 vec = Camera.main.WorldToScreenPoint(chapter[0].transform.position);

        for(int i=1; i<chapter.Length; i++)
        {
            vec.x += chapterGap;

            chapter[i].transform.position = Camera.main.ScreenToWorldPoint(vec);

            if(i==3)//chapter 4는 위치 조정 필
            {
                chapter[i].transform.position = new Vector3(chapter[i].transform.position.x, chapter[i].transform.position.y, 1f);
            }
        }

        chapterGap = chapter[1].transform.position.x - chapter[0].transform.position.x;

        if(UserDataManager.Instance !=null)
        {
            Vector3 targetPos = chapterRoot.transform.position;
            targetPos.x -= chapterGap * (UserDataManager.Instance.chapterCurrent - 1);
            chapterRoot.transform.position = targetPos;

            chapterNumber = UserDataManager.Instance.chapterCurrent;
            label_ChapterNumber.text = "Chapter " + chapterNumber.ToString();
        }

        UserInfoSet();
    }

    void UserInfoSet()
    {
        if(UserDataManager.Instance == null)
        {
            return;
        }
        else
        {
            label_UserName.text = UserDataManager.Instance.userName;
            label_Level.text = "Lv." + UserDataManager.Instance.chapterLimit.ToString();
        }
        
        
    }

    //구름이동
    void FixedUpdate()
    {
        cloud.transform.Translate(Vector3.right * 0.1f);
        if(cloud.transform.position.x >= 180f + chapterGap)
        {
            cloud.transform.position = new Vector3( - 180f - chapterGap, cloud.transform.position.y, cloud.transform.position.z);
        }
    }

    public void NextChapter()
    {
        if(isChange)
        {
            return;
        }

        if (chapterNumber < 4)
        {
            Vector3 targetPos = chapterRoot.transform.position;
            targetPos.x -= chapterGap;
            
            StartCoroutine(ChapterChange(targetPos));

            chapterNumber++;
        }
        else
        {
            return;
        }
    }

    public void PreviousChapter()
    {
        if(isChange)
        {
            return;
        }

        if(chapterNumber > 1)
        {
            Vector3 targetPos = chapterRoot.transform.position;
            targetPos.x += chapterGap;

            StartCoroutine(ChapterChange(targetPos));

            chapterNumber--;
        }
        else
        {
            return;
        }
    }

    IEnumerator ChapterChange(Vector3 targetPos)
    {
        isChange = true;
        EnterButton.gameObject.SetActive(false);
        LockButton.gameObject.SetActive(false);
        label_ChapterNumber.gameObject.SetActive(false);

        while(true)
        {
            yield return null;
            if(targetPos== chapterRoot.transform.position)
            {

                label_ChapterNumber.gameObject.SetActive(true);
                label_ChapterNumber.text = "Chapter " + chapterNumber.ToString();

                isChange = false;

                if(UserDataManager.Instance != null)
                {
                    if(UserDataManager.Instance.chapterLimit >= chapterNumber)
                    {
                        EnterButton.gameObject.SetActive(true);
                        
                    }
                    else
                    {
                        LockButton.gameObject.SetActive(true);
                    }
                }
                else
                {
                    EnterButton.gameObject.SetActive(true);
                }

                break;
            }
            else
            {
                chapterRoot.transform.position = Vector3.MoveTowards(chapterRoot.transform.position, targetPos, changeSpeed * Time.deltaTime);
            }
        }
    }

    public void ChapterEnter()
    {
        if(isChange)
        {
            return; 
        }

        Debug.Log("Chapter Start!");

        if(UserDataManager.Instance == null)
        {
            Debug.Log("유저 데이터 관리 객체 NULL");
        }
        else
        {
            Debug.Log("유저 데이터 관리 객체 ACTIVE");
            UserDataManager.Instance.chapterCurrent = chapterNumber;
        }

        SceneManager.LoadScene("Game Scene");

    }

    public void BackLoginScene()
    {
        SceneManager.LoadScene("Login Scene");
    }
}
