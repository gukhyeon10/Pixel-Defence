﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceWeaponScript : MonoBehaviour
{
    Vector3 direct;
    float speed;
    [SerializeField]
    AudioSource audio;
    float attack = 5f;

    public void Init(Transform target, float speed, float attack)
    {
        direct = (target.position - this.transform.position + new Vector3(0f, 1f, 0f)).normalized;
        transform.rotation = Quaternion.LookRotation(direct);
        this.speed = speed;
        this.attack = attack;
        Destroy(this.gameObject, 1.4f);
    }


    void Update()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            other.GetComponent<GameEnemy>().stats.hp -= attack;
        }
        audio.Play();
    }
    
}
