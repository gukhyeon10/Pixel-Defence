using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceWeaponScript : MonoBehaviour
{
    Vector3 direct;
    float speed;

    public float attack = 5f;

    public void Init(Transform target, float speed)
    {
        direct = (target.position - this.transform.position + new Vector3(0f, 1f, 0f)).normalized;
        transform.rotation = Quaternion.LookRotation(direct);

        this.speed = speed;
        //Instantiate(Effect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject, 1f);
    }


    private void Update()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            other.GetComponent<GameEnemy>().stats.hp -= attack;
        }
        //Debug.Log("enemy");
        Destroy(this.gameObject);
        //Instantiate(Effect.gameObject, other.transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        //Instantiate(Effect, this.transform.position, Quaternion.identity);

    }
}
