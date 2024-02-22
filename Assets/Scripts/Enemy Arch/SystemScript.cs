using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemScript : MonoBehaviour
{
    public int Money = 2; // Counter variable

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && Money > 0)
        {
            Money--;
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Money++;
        }
    }
}