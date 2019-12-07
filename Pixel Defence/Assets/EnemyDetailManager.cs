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

    public void EnemyDetailActive(bool isActive)
    {
        EnemyDetailDialog.SetActive(isActive);

        if(isActive)
        {
            
        }
    }
}
