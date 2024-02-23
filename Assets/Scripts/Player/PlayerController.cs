using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private AudioSource audioSource;
    private Animator anim;

    [Header("Movement")]
    #region Movement Variables
    private Rigidbody2D rb;
    public float speed = 2.0f;
    public bool canMove = true;
    private Vector2 lastMove = new Vector2(1, 0);
    #endregion

    [Header("Slap")]
    #region slap Variables
    [SerializeField] private GameObject slapGO;
    [SerializeField] private float slapTimer = 0.2f;
    [SerializeField] private bool canSlap = true;
    [SerializeField] private float slapForce = 100.0f;
    [SerializeField] private AudioClip slapClip;
    [SerializeField] private AudioClip wooshClip;
    #endregion

    [Header("Hook")]
    #region Hook Variables
    [SerializeField] private float hookRange = 5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float hookSpeed = 5f;
    [SerializeField] private LineRenderer hookLine;

    private bool isHooked = false;
    private Transform hookedEnemy;
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    public void Move(Vector2 move)
    {
        if (canMove)
        {
            // Move the player
            // rb.MovePosition(transform.position + move.ConvertTo<Vector3>() * speed);
            rb.AddForce(move * speed, ForceMode2D.Impulse);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed);
            if (move.magnitude == 0) {
                anim.SetBool("Move", false);
            } else {
                anim.SetBool("Move", true);
            }

            // Update direction for hitting and hooking
            lastMove = (move.x == 0 && move.y == 0) ? lastMove : new Vector2(move.x, move.y);
        }
    }

    public void Slap(ref bool slap)
    {
        if (slap && canSlap)
        {
            // Hit
            slapGO.transform.rotation = Quaternion.LookRotation(Vector3.forward, lastMove);
            slapGO.transform.position = transform.position + new Vector3(lastMove.x, lastMove.y, 0);
            List<Collider2D> enemies = Physics2D.OverlapCircleAll(slapGO.transform.position, 0.5f, enemyLayer).ToList();
            if (enemies != null && enemies.Count > 0)
                enemies.ForEach(x => {x.GetComponent<EnemyBehaviour>().GetSlapped(slapForce, (transform.position - x.transform.position).normalized); audioSource.PlayOneShot(slapClip);});
            else
                audioSource.PlayOneShot(wooshClip);
            StartCoroutine(SlapEffect());
            // Play the slap animation
            // Trigger the slap effect

            // Untrigger the button
        }
        slap = false;
    }

    private IEnumerator SlapEffect () {
        yield return new WaitForSeconds(slapTimer);
        slapGO.transform.localPosition = Vector2.zero;
        canSlap = true;
    }

    public void Hook(ref bool hook)
    {
        if (hook)
        {
            // Hook
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, hookRange, enemyLayer);
            // Debug.Log("Hooking " + hitEnemies.Length + " enemies.");

            float shortestDistance = Mathf.Infinity;
            Transform nearestEnemy = null;

            foreach (Collider2D enemy in hitEnemies)
            {
                // Check if the enemy is colliding with the player
                if (enemy.GetComponent<Collider2D>().IsTouching(GetComponent<Collider2D>()))
                {
                    continue; // Skip this enemy if it's colliding with the player
                }

                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy.transform;
                    // Debug.Log("Hooking " + nearestEnemy.name + " at distance " + shortestDistance + " from the player.");
                }
            }

            if (isHooked)
            {
                // Unhook the player
                isHooked = false;
                hookedEnemy = null;
                canMove = true;
                hookLine.enabled = false;
            }
            else
            {
                if (nearestEnemy != null)
                {
                    isHooked = true;
                    hookedEnemy = nearestEnemy;

                    canMove = false;
                }
            }

            // perform hook on target
            hook = false;
        }
    }


    // for editor debugging and drawing
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hookRange);
    }

    // update for physics calculations
    public void FixedUpdate()
    {
        // Update
        if (isHooked && hookedEnemy != null)
        {
            // Move the player towards the hooked enemy
            // transform.position = Vector2.MoveTowards(transform.position, hookedEnemy.position, hookSpeed * Time.deltaTime);
            // rb.AddForce(-Vector2.MoveTowards(transform.position, hookedEnemy.position, hookSpeed * Time.deltaTime));
            rb.AddForce((hookedEnemy.position - transform.position).normalized * hookSpeed, ForceMode2D.Impulse);
            hookLine.enabled = true;
            hookLine.SetPosition(0, transform.position);
            hookLine.SetPosition(1, hookedEnemy.position);
        }
    }

    private void Update () {
        anim.SetFloat("VelocityX", lastMove.x);
        anim.SetFloat("VelocityY", lastMove.y);
    }

    // collision detection
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHooked && collision.transform == hookedEnemy)
        {
            // Unhook the player
            isHooked = false;
            hookedEnemy = null;
            canMove = true;
            hookLine.enabled = false;
        }
    }
}
