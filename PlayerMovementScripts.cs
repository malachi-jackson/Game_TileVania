using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScripts : MonoBehaviour
{
    [SerializeField] float Runspeed= 7f;
    [SerializeField] float Jumpspeed= 7f;
    [SerializeField] float Climbspeed= 7f;

    Vector2 PlayerInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    float gravityScaleatStart;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleatStart = myRigidbody.gravityScale;
    }


    void Update()
    {
        Run();
        Flipsprite();
        ClimbLadder();
    }
    void OnMove(InputValue value)
    {
        PlayerInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {return;}
        if(value.isPressed)
        {
            // do stuff
            myRigidbody.velocity = new Vector2 (0f, Jumpspeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(PlayerInput.x * Runspeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
    
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("IsRunning",playerHasHorizontalSpeed);
    }

    void Flipsprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
        transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x), 1f);
    }
    }

    void ClimbLadder()
    {
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myRigidbody.gravityScale = gravityScaleatStart;
            myAnimator.SetBool("IsCliming", false);
            return;
        }
    
        Vector2 climbVelocity = new Vector2 (myRigidbody.velocity.x, PlayerInput.y * Climbspeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("IsCliming", playerHasVerticalSpeed);
    }
}