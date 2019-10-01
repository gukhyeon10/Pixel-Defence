using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainProcess : MonoBehaviour
{
    [SerializeField]
    GameDataManager gameDataManager;
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
    
}
