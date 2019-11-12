using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicemanScript : UnitScript
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
                GameObject SkillObject = Instantiate(Weapon, this.transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
                //GameObject SkillObject = Instantiate(Weapon, target.transform.position, Quaternion.identity);
                SkillObject.GetComponent<PoliceWeaponScript>().Init(this.target, 15f, attack);
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
