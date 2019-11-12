using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    [SerializeField]
    GameObject Effect;


    [SerializeField]
    AudioSource audio;

    Vector3 direct;
    float speed;
    
    float attack = 5f;

    public void Init(Transform target, float speed, float attack)
    {
        direct = (target.position - this.transform.position + new Vector3(0f, 2f, 0f)).normalized;
        this.transform.eulerAngles = direct;
        this.speed = speed;
        this.attack = attack;
        audio.Play();
        Destroy(this.gameObject, 2f);
    }
    

    void Update()
    {
        this.transform.Translate(direct * Time.deltaTime * speed * 5f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Enemy"))
        {
            other.GetComponent<GameEnemy>().stats.hp -= attack;
        }
        Instantiate(Effect, other.transform.position + Vector3.up, Quaternion.identity);
    }

}
