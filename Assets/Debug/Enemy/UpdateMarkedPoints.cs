using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMarkedPoints : MonoBehaviour
{

    public Dictionary<GameObject, int> markedPoints;
    [SerializeField]
    private GameObject[] Points;
    // Start is called before the first frame update
    // void Awake()
    // {
    //     markedPoints = new Dictionary<GameObject, int>();
    //     foreach(GameObject p in Points)
    //     {
    //         markedPoints.Add(p, 0);
    //     }
        
    // }

    // Update is called once per frame
    void Update()
    {
        
    }
}
