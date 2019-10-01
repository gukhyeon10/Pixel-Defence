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

    float chapterGap;

    int chapterNumber = 1;

    bool isChange = false;

    public float changeSpeed = 55f;
    

    // Start is called before the first frame update
    void Start()
    {
        chapterGap = (float)Screen.width;

        Vector3 vec = Camera.main.WorldToScreenPoint(chapter[0].transform.position);
        Vector3 vec2 = vec;
        vec2.x = vec2.x + chapterGap;

        chapter[1].transform.position = Camera.main.ScreenToWorldPoint(vec2);

        Vector3 vec3 = vec2;
        vec3.x = vec3.x + chapterGap;
        chapter[2].transform.position = Camera.main.ScreenToWorldPoint(vec3);

        chapterGap = chapter[1].transform.position.x - chapter[0].transform.position.x;


    }

    private void LateUpdate()
    {
    }

    public void NextChapter()
    {
        if(isChange)
        {
            return;
        }

        if (chapterNumber < 3)
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
        while(true)
        {
            yield return null;
            if(targetPos== chapterRoot.transform.position)
            {
                isChange = false;
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
}
