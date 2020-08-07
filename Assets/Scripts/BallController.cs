using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float actualSpeed;
    public Vector2 velocity;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        actualSpeed = velocity.magnitude;
        transform.Translate(velocity * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.IncreaseScore();
            Vector2 wallNormal = collision.contacts[0].normal;
            velocity = wallNormal * velocity.magnitude;
        }
        else if(collision.gameObject.CompareTag("Kill"))
        {
            GameManager.Instance.Kill();
        }
        else
        {
            Vector2 wallNormal = collision.contacts[0].normal;
            if (wallNormal.x!=0)
            {
                velocity.x *= -1;
            }
            else
            {
                velocity.y *= -1;
            }
        }
    }
}
