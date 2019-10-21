using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaWeaponScript : MonoBehaviour
{
    [SerializeField]
    GameObject Effect;
    //Vector3 direct;
    float direct;
    float speed;

    public float attack = 5f;

    public void Init(float direct, float speed)
    {
        //direct = (target.position - this.transform.position + new Vector3(0f, 2f, 0f)).normalized;
        //Debug.Log(direct);
        //this.transform.eulerAngles = direct;
        this.direct = direct;
        this.speed = speed;
        Destroy(this.gameObject, 1f);
    }

    private void Update()
    {
        //this.transform.Translate(direct * Time.deltaTime * speed);
        this.transform.eulerAngles = new Vector3(0f, direct, 0f);
        this.transform.Translate(Vector3.forward * Time.deltaTime * speed);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            other.GetComponent<GameEnemy>().stats.hp -= attack;
        }
        Destroy(this.gameObject);
        Instantiate(Effect, this.transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        //Instantiate(Effect, this.transform.position, Quaternion.identity);
    }
}
