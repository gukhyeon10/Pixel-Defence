using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerScript : UnitScript
{
    // Start is called before the first frame update
    void Start()
    {
        UnitInit();
        EnemyLoad();

        if (!isCursor)
        {
            GameMainProcess.Instance.moneySpeed *= 0.9f;
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
