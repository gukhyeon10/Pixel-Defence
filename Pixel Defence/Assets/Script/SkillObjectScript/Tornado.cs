using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    [SerializeField]
    GameObject Effect;
    Vector3 direct;
    float speed;

    public void Init(Transform target, float speed)
    {
        direct = (target.position - this.transform.position + new Vector3(0f, 2f, 0f)).normalized;
        this.transform.eulerAngles = direct;
        this.speed = speed;
        Destroy(this.gameObject, 2f);
    }
    

    void Update()
    {
        this.transform.Translate(direct * Time.deltaTime * speed * 5f);

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        Instantiate(Effect, other.transform.position + Vector3.up, Quaternion.identity);
    }

}
