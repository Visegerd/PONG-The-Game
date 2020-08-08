using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed;
    public Vector2 velocity;
    private Rigidbody rb;
    private bool isHitAlready = false;
    private bool verticalWallHit;
    private Vector2 oldPos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //actualSpeed = velocity.magnitude;
        transform.Translate(velocity * Time.deltaTime);
        Vector2 frameStep = (Vector2)transform.position - oldPos;
        Vector2 perp = Vector2.Perpendicular(velocity);
        perp = perp.normalized;
        Debug.DrawRay((Vector2)transform.position + perp * transform.localScale.x / 2.0f, velocity.normalized * frameStep.magnitude, Color.green, 1.0f);
        Debug.DrawRay((Vector2)transform.position - perp * transform.localScale.x / 2.0f, velocity.normalized * frameStep.magnitude, Color.green, 1.0f);
        RaycastHit2D hitUp = Physics2D.Raycast((Vector2)transform.position + perp * transform.localScale.x / 2.0f, velocity / 2.0f, frameStep.magnitude);
        if (hitUp.collider != null && !isHitAlready)
        {
            if (hitUp.collider.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.IncreaseScore();
                Vector2 dir = transform.position - hitUp.collider.gameObject.transform.position;
                dir = dir.normalized * speed;
                Debug.DrawRay(transform.position, dir, Color.cyan, 20.0f);
                velocity = dir * velocity.magnitude * speed;
                //velocity.x *= -1;
                isHitAlready = true;
                Debug.Log("gora");
            }
        }
        RaycastHit2D hitDown = Physics2D.Raycast((Vector2)transform.position - perp * transform.localScale.x / 2.0f, velocity / 2.0f, frameStep.magnitude);
        if (hitDown.collider != null && !isHitAlready)
        {
            if (hitDown.collider.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.IncreaseScore();
                Vector2 dir = transform.position - hitDown.collider.gameObject.transform.position;
                dir = dir.normalized * speed;
                Debug.DrawRay(transform.position, dir, Color.cyan, 20.0f);
                velocity = dir * velocity.magnitude * speed;
                //velocity.x *= -1;
                isHitAlready = true;
                Debug.Log("dol");
            }
        }
        oldPos = transform.position;
    }

    public void CheckWall()
    {
        if(velocity.y>0)
        {
            verticalWallHit = true;
        }
        else verticalWallHit = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isHitAlready)
        {
            isHitAlready = true;
            GameManager.Instance.IncreaseScore();
            Debug.Log("Trafil gracza");
            Vector2 dir = transform.position - collision.gameObject.transform.position;
            dir = dir.normalized;
            Debug.DrawRay(transform.position, dir, Color.cyan, 20.0f);
            velocity = dir * speed;
            CheckWall();
            //velocity.x *= -1;

            //Vector2 wallNormal = collision.contacts[0].normal;
            //velocity = wallNormal * velocity.magnitude;
            //if (velocity.x < 0) velocity.x *= -1;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            isHitAlready = true;
            Vector2 dir = transform.position - collision.gameObject.transform.position;
            dir = dir.normalized;
            Debug.DrawRay(transform.position, dir, Color.cyan, 20.0f);
            velocity = dir * velocity.magnitude;
            CheckWall();
        }
        else if (collision.gameObject.CompareTag("Kill"))
        {
            GameManager.Instance.Kill();
        }
        else if (collision.gameObject.CompareTag("RightWall"))
        {
            isHitAlready = false;
            velocity.x *= -1;
        }
        else
        {
            Vector2 wallNormal = collision.contacts[0].normal;
            if (wallNormal.x!=0)
            {
                if (wallNormal.x < 0 && transform.position.x < 8.5 && velocity.x > 0)
                {
                    velocity.x *= -1;
                }
            }
            else 
            {
                if (wallNormal.y < 0 && transform.position.y < 4.5 && velocity.y > 0 && verticalWallHit)
                {
                    velocity.y *= -1;
                    verticalWallHit = !verticalWallHit;
                }
                else if (wallNormal.y > 0 && transform.position.y > -4.5 && velocity.y < 0 && !verticalWallHit)
                {
                    velocity.y *= -1;
                    verticalWallHit = !verticalWallHit;
                }
            }
        }

    }
}
