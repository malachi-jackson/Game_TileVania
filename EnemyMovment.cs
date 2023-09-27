using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovment : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 1f;
    Rigidbody2D Rbody;
    void Start()
    {
        Rbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Rbody.velocity = new Vector2 (MoveSpeed, 0f);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        MoveSpeed = -MoveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2 (-(Mathf.Sign(Rbody.velocity.x)), 1f);
    }


}
