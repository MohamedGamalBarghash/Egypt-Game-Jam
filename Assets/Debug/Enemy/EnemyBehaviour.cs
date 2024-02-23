using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // Adjust the speed for the application.
    public float speed ;

    public bool stolen = false;

    private bool canMove = true;
    public float hitDelay = 0.3f;
    
    //animator
    private Animator animator;
    public Village stolenFrom;
    
    private Rigidbody2D rb;
    private Transform target = null;
    [SerializeField]
    public Transform[] points;
    public UpdateMarkedPoints updateMarkedPoints;

    public Transform home;
    int index;
    private int id;
    float x,y;

    // Start is called before the first frame update
    void Start()
    {
        
        x=0;
        y=0;
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator= gameObject.GetComponent<Animator>();
        //index of target point is from 0 to 3
        index = Random.Range(0, points.Length);
        id = gameObject.GetInstanceID();
        pickPoint();
    }
    // Update is called once per frame
    void Update()
    {

        if (canMove)
        {
            // Move our position a step closer to the target.
            var step = speed * Time.deltaTime; // calculate distance to move

            if (target != null)
            {
                // Check if the position of the enemy and point are approximately equal.
                if (Mathf.Abs(Vector2.Distance(transform.position, target.transform.position)) < 0.001f)
                {
                    // Stop the movement of the enemy.
                    rb.velocity = new Vector2(0, 0);
                    x=0;
                    y=0;
                }
                else
                {
                    var  oldPosition = transform.position;
                    transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
                    var newPosition =transform.position-oldPosition;
                    x=newPosition.x;
                    y=newPosition.y;

                    // rb.MovePosition(target.transform.position.normalized * step);
                }
                animator.SetFloat("VelocityX",x);
                animator.SetFloat("VelocityY",y);
            }
            else
            {
                index = Random.Range(0, points.Length);
                pickPoint();
            }

        }


        // Debug.Log("VelocityX: " + rb.velocity.x);
        //Debug.Log("VelocityY: " + rb.velocity.x);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "TargetPoint")
        {
            target = home;
            // stolen
            stolen = true;
            speed = 8;
            stolenFrom = collision.gameObject.GetComponent<Village>();
            stolenFrom.StealResources(1);
        }
        else if (collision.gameObject.tag == "Home")
        {
            pickPoint();
            stolen = false;
            speed = 11;
            collision.gameObject.GetComponent<BankManager>().DepositResources(1);
            stolenFrom = null;
            // deposit
        }
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

    public void GetSlapped(float hitForce, Vector2 hitDirection)
    {
        rb.velocity = Vector2.zero;
        canMove = false;
        rb.AddForce(hitDirection * -hitForce, ForceMode2D.Impulse);
        StopAllCoroutines();
        if (stolenFrom != null)
            stolenFrom.ReturnResources(1);
        StartCoroutine(ReMove());
    }

    private IEnumerator ReMove () {
        yield return new WaitForSeconds(hitDelay);
        target = home;
        canMove = true;
    }
}
