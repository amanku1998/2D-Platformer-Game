using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public void PlayEffect()
    {
        //particleSystem.Play();
        gameObject.SetActive(true);
    }
}
