using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemScript : MonoBehaviour
{
    public int Money = 2; // Counter variable

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Money > 0 )
        {

            Money--;
        }
        else if (other.CompareTag("Enemy"))
        {
            StartCoroutine(WaitAndIncreaseCounter());
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }
    private IEnumerator WaitAndIncreaseCounter()
    {
        yield return new WaitForSeconds(4f);
        Money++;
    }


   
}