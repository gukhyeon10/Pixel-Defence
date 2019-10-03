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
