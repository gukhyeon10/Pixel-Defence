using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemisScript : UnitScript
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
        if (target != null && target.position.y >= 0.9)
        {
            this.transform.LookAt(target);
            
            if(!isCoolTime)
            {
                    GameObject SkillObject = Instantiate(Weapon, this.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
                    SkillObject.GetComponent<orb05_red_Script>().Init(this.target, speed * 15f);

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
