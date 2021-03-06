﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWeaponScript : MonoBehaviour
{
    [SerializeField]
    GameObject Effect;
    Vector3 direct;
    float speed;

    float attack = 5f;

    public void Init(Transform target, float speed, float attack)
    {
        direct = (target.position - this.transform.position + new Vector3(0f, 2f, 0f)).normalized;
        this.transform.eulerAngles = direct;
        this.speed = speed;
        this.attack = attack;
        Destroy(this.gameObject, 2f);
    }

    private void Update()
    {
        this.transform.Translate(direct * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
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
