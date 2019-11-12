using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerScript : UnitScript
{
    // Start is called before the first frame update
    void Start()
    {
        UnitInit();

        this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("CharacterAnim/IntroAnim 1") as RuntimeAnimatorController;

        EnemyLoad();

        if (!isCursor)
        {
            GameMainProcess.Instance.moneySpeed *= 0.95f;
        }
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
