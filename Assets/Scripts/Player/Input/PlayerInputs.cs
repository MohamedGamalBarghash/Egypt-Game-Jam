using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    // Input Variables
    private Vector2[] move = new Vector2[2];
    public bool[] hit = new bool[2];

    // Players Handlers
    public PlayerController[] players = new PlayerController[2];

    public void OnMove(InputValue context)
    {
        move[0] = context.Get<Vector2>();
    }

    public void OnMoveTwo(InputValue context)
    {
        move[1] = context.Get<Vector2>();
    }

    public void OnHit (InputValue context)
    {
        hit[0] = context.isPressed;
    }

    public void OnHitTwo (InputValue context)
    {
        hit[1] = context.isPressed;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].Move(move[i]);
            players[i].Hit(ref hit[i]);
        }
    }
}
