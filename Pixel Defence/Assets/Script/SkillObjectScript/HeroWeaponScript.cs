using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroWeaponScript : MonoBehaviour
{
    Vector3 direct;
    float speed;

    public void Init()
    {
        //direct = (target.position - this.transform.position + new Vector3(0f, 2f, 0f)).normalized;
        //this.transform.eulerAngles = direct;
        //this.speed = speed;
        Destroy(this.gameObject, 2f);
    }

    private void Update()
    {
        //this.transform.Translate(direct * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        //Instantiate(Effect, this.transform.position, Quaternion.identity);
    }
}
