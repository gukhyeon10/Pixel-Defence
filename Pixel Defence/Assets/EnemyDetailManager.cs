using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetailManager : MonoBehaviour
{
    [SerializeField]
    GameObject EnemyDetailDialog;

    [SerializeField]
    UILabel label_Name;
    [SerializeField]
    UILabel label_Def;
    [SerializeField]
    UILabel label_Hp;

    // 적 유닛 세부 능력치 활성화
    public void EnemyDetailActive(bool isActive, EnemyStats stats, string name)
    {
        EnemyDetailDialog.SetActive(isActive);

        if(isActive)
        {
            label_Name.text = name.Substring(0, name.Length - 7); // 하이어라키 창에 등록된 이름 뒤에 '(clone)' 문자열 제외
            label_Def.text = stats.def.ToString();
            label_Hp.text = stats.hp.ToString();
        }
    }
}
