using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaScript : UnitScript
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
                StartCoroutine(NinjaSkill());

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

    IEnumerator NinjaSkill()
    {
        float standY = this.transform.eulerAngles.y;

        if (this.target != null)
        {
            GameObject SkillObject = Instantiate(Weapon, this.transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
            SkillObject.GetComponent<NinjaWeaponScript>().Init(standY, speed * 30f, attack);
        }

        if (this.target != null)
        {
            GameObject SkillObject = Instantiate(Weapon, this.transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
            SkillObject.GetComponent<NinjaWeaponScript>().Init(standY - 15f, speed * 30f, attack * 1.5f);
        }

        if (this.target != null)
        {
            GameObject SkillObject = Instantiate(Weapon, this.transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
            SkillObject.GetComponent<NinjaWeaponScript>().Init(standY + 15f, speed * 30f, attack);
        }

        yield return null;
    }

}
