using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearScript : MonoBehaviour
{
    [SerializeField]
    AudioSource audio;
   
    void OnEnable()
    {
        audio.Play();
    }

 
}
