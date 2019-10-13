using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orb05_green_Script : MonoBehaviour
{
    [SerializeField]
    GameObject Effect;
    Vector3 direct;
    float speed;

    public float attack = 2f;

    public void Init(Transform target, float speed)
    {

        direct = (target.position - this.transform.position + new Vector3(0f, 2f, 0f)).normalized;
        this.transform.eulerAngles = direct;
        this.speed = speed;
        Destroy(this.gameObject, 2f);

        
    }

    private void Update()
    {
        this.transform.Translate(direct * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Enemy"))
        {
            other.GetComponent<GameEnemy>().stats.hp -= attack;
        }
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        Instantiate(Effect, this.transform.position, Quaternion.identity);
    }
}
