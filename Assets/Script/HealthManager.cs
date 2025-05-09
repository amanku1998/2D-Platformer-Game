using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static int health = 3;
    public Image[] hearts;

    private void Awake()
    {
        health = 3;
    }

    private void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].color = (i < health) ? Color.red : Color.black;
        }
    }

}
