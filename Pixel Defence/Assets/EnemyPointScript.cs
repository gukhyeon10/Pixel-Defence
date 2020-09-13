using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //상은 시형 ㅎㅇ
    }

    // Update is called once per frame
    void Update()
    {
        // y축 기준 자전
        this.transform.Rotate(Vector3.down);
    }
}
