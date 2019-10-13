using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelWeaponScript : MonoBehaviour
{
    [SerializeField]
    GameObject Effect;
    float direct;
    float speed;

    public float attack = 7.7f;
    public void Init(float direct, float speed)
    {

        this.direct = direct;
        this.speed = speed;
        Destroy(this.gameObject, 2f);


    }

    private void Update()
    {
        this.transform.eulerAngles = new Vector3(0f, direct, 0f);
        this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Enemy"))
        {
            other.GetComponent<GameEnemy>().stats.hp -= attack;
        }

        Instantiate(Effect.gameObject, other.transform.position + Vector3.up, Quaternion.identity);
    }
    private void OnDestroy()
    {
    }
}
