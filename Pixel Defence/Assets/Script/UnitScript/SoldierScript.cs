using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierScript : UnitScript
{
    bool isCoolTime = false;
    [SerializeField]
    GameObject Weapon;
    [SerializeField]
    float attack;
    // Start is called before the first frame update
    void Start()
    {
        UnitInit();
        EnemyLoad();

        coolTime *= 2f;
    }

    // Update is called once per frame
    void Update()
    {
        FindTarget();
        if (target != null && target.position.y >= 0.9)
        {
            this.transform.LookAt(target);

            if (!isCoolTime)
            {
                StartCoroutine(SoliderSkill());

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

    IEnumerator SoliderSkill()
    {
        if(this.target != null)
        {
            GameObject SkillObject = Instantiate(Weapon, this.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
            SkillObject.GetComponent<orb05_green_Script>().Init(this.target, speed * 15f, attack);

        }
        yield return new WaitForSeconds(0.2f);

        if (this.target != null)
        {
            GameObject SkillObject = Instantiate(Weapon, this.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
            SkillObject.GetComponent<orb05_green_Script>().Init(this.target, speed * 15f, attack);

        }
        yield return new WaitForSeconds(0.2f);

        if (this.target != null)
        {
            GameObject SkillObject = Instantiate(Weapon, this.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
            SkillObject.GetComponent<orb05_green_Script>().Init(this.target, speed * 15f, attack * 1.5f);

        }
    }


}
