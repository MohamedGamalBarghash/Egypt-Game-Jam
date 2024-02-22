using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // Adjust the speed for the application.
    public float speed = 1.0f;

      
    private Rigidbody2D rb;    
    private GameObject target = null;
    [SerializeField]
    private GameObject[] points;
    public UpdateMarkedPoints updateMarkedPoints;
    int index;
    public int id;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //index of target point is from 0 to 3
        index = Random.Range(0, points.Length);
        pickPoint();
    }
    // Update is called once per frame
    void Update()
    {

        // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move

        
        if(target != null)
        {
            // Check if the position of the enemy and point are approximately equal.
            if (Mathf.Abs(Vector2.Distance(transform.position, target.transform.position)) < 0.001f)
            {
                // Stop the movement of the enemy.
                rb.velocity = new Vector2(0, 0);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
            }
        }
        else
        {
            index = Random.Range(1, points.Length);
            pickPoint();
        }
        
        
        Debug.Log("VelocityX: " + rb.velocity.x);
        Debug.Log("VelocityY: " + rb.velocity.x);
    }

    private void pickPoint()
    {
        
        //if point is not marked then update the dict
        if (updateMarkedPoints.markedPoints[points[index]] == 0)
        {
            target = points[index];
            updateMarkedPoints.markedPoints[points[index]] = id;
        }
        else if (updateMarkedPoints.markedPoints[points[index]] == id)
        {
            target = points[index];
        }
    }
}
