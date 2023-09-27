using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScripts : MonoBehaviour
{
    [SerializeField] float Runspeed= 7f;
    [SerializeField] float Jumpspeed= 7f;
    [SerializeField] float Climbspeed= 7f;
    [SerializeField] Vector2 DeathKick = new Vector2 (15f,5f);
    [SerializeField] GameObject Arrow;
    [SerializeField] Transform Bow;

    Vector2 PlayerInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleatStart;

    bool IsAlive = true;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleatStart = myRigidbody.gravityScale;
    }


    void Update()
    {
        if (!IsAlive) { return; }
        Run();
        Flipsprite();
        ClimbLadder();
        Die();  
    }
    void OnMove(InputValue value)
    {
        if (!IsAlive) { return; }
        PlayerInput = value.Get<Vector2>();
    }
    
    void OnFire(InputValue value)
    {
        if (!IsAlive) { return; }
        Instantiate(Arrow, Bow.position, transform.rotation);
    }
    void OnJump(InputValue value)
    {
        if (!IsAlive) { return; }
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {return;}
        if(value.isPressed)
        {
            // do stuff
            myRigidbody.velocity += new Vector2 (0f, Jumpspeed);
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
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
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
    void Die()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazzards")))
        {
            IsAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = DeathKick;
        }
         if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "RoofHazzards")))
        {
            IsAlive = false;
            myAnimator.SetTrigger("Dying");
        }
    }
}