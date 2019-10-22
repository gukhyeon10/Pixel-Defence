using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelScript : UnitScript
{
    bool isCoolTime = false;
    [SerializeField]
    GameObject Weapon;
    [SerializeField]
    AudioSource audio;
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
                audio.Play();

                StartCoroutine(AngelSkill());

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

    IEnumerator AngelSkill()
    {
        float standY = this.transform.eulerAngles.y;
        for(int i=1; i<=6; i++)
        {
            GameObject SkillObject = Instantiate(Weapon, this.transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
            SkillObject.GetComponent<AngelWeaponScript>().Init(standY + i * 60f, speed * 5f);
        }

        yield return null;
    }
}
