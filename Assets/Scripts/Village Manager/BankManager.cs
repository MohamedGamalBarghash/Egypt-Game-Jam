using System.Collections;
using UnityEngine;

public class BankManager : MonoBehaviour
{
    public int resources = 5;

    public void DepositResources(int amount)
    {
        resources += amount;
    }

    public void StealResources(int amount)
    {
        if (resources > 0)
        {
            resources -= amount;
        }
    }
}
