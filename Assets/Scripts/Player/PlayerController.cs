using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start () {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move (Vector2 move) {
        // Move the player
        rb.velocity = move;
    }

    public void Hit (ref bool hit) {
        // Hit
        if (hit) {
            rb.AddTorque(1, ForceMode2D.Impulse);
            hit = false;
        }
    }

    public void Update () {
        // Update
    }
}
