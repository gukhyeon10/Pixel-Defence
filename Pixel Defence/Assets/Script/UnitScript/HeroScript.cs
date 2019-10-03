using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroScript : UnitScript
{
    bool isCoolTime = false;
    [SerializeField]
    GameObject Weapon;
    // Start is called before the first frame update
    void Start()
    {
        UnitInit();
        EnemyLoad();
    }

    // Update is called once per frame
    void Update()
    {
        FindTarget();
        if (target != null)
        {
            this.transform.LookAt(target);
        }
    }
}
