using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMarkedPoints : MonoBehaviour
{

    public Dictionary<Transform, int> markedPoints;
    [SerializeField]
    private Transform[] Points;
    // Start is called before the first frame update
    void Awake()
    {
        markedPoints = new Dictionary<Transform, int>();
        foreach(Transform p in Points)
        {
            markedPoints.Add(p, 0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
