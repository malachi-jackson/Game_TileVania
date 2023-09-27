using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float ArrowSpeed = 20f;
    Rigidbody2D Rbody;
    PlayerMovementScripts player;
    float xSpeed;
    void Start()
    {
        Rbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovementScripts>();
        xSpeed = player.transform.localScale.x * ArrowSpeed;
    }

    void Update()
    {
        Rbody.velocity = new Vector2 (xSpeed, 0f);

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
     Destroy(gameObject);
    }
}
