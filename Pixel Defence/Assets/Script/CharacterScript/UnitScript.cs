using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        anim.runtimeAnimatorController = Resources.Load("CharacterAnim/UnitAnim") as RuntimeAnimatorController;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
