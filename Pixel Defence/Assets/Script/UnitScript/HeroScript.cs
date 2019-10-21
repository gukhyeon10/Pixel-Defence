﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroScript : UnitScript
{
    bool isCoolTime = false;
    [SerializeField]
    GameObject Weapon;
    
    void Start()
    {
        UnitInit();
        EnemyLoad();
    }

    void Update()
    {
        FindTarget();
        if (target != null && target.position.y >= 0.9)
        {
            this.transform.LookAt(target);

            if (!isCoolTime)
            {
                //GameObject SkillObject = Instantiate(Weapon, this.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
                GameObject SkillObject = Instantiate(Weapon, target.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
                SkillObject.GetComponent<HeroWeaponScript>().Init();

                StartCoroutine(CoolTime());

            }

        }
    }

    IEnumerator CoolTime()
    {
        isCoolTime = true;
        yield return new WaitForSeconds(coolTime);
        isCoolTime = false;
    }
}
