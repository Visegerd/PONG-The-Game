using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed; //actual speed
    public Vector2 velocity; //movement vector

    private Rigidbody rb; //object rigidbody
    private bool isHitAlready = false; //bool to prevent multiple hits from happening
    private bool verticalWallHit; //bool to prevent ball bugging inside wall
    private Vector2 oldPos; //last frame ball position

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocity * Time.deltaTime); //move ball
        Vector2 frameStep = (Vector2)transform.position - oldPos;
        Vector2 perp = Vector2.Perpendicular(velocity);
        perp = perp.normalized;
        #region Check raycasts at the perpendicular edges of ball if we hit player
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
                isHitAlready = true;
                Debug.Log("dol");
            }
        }
        #endregion
        oldPos = transform.position; //save current pos for next frame
    }

    public void CheckWall() //Check which vertical wall will be hit next
    {
        if (velocity.y > 0)
        {
            verticalWallHit = true;
        }
        else verticalWallHit = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) //if hit player, set direction outwards from player and give 1 point
        {
            isHitAlready = true;
            GameManager.Instance.IncreaseScore();
            Debug.Log("Trafil gracza");
            Vector2 dir = transform.position - new Vector3(collision.gameObject.transform.position.x - 1.5f, collision.gameObject.transform.position.y, 0.0f);
            dir = dir.normalized;
            Debug.DrawRay(transform.position, dir, Color.cyan, 20.0f);
            velocity = dir * speed;
            CheckWall();
        }
        else if (collision.gameObject.CompareTag("Kill")) //if hit left wall
        {
            GameManager.Instance.Kill();
        }
        else if (collision.gameObject.CompareTag("RightWall")) //if hit right wall
        {
            isHitAlready = false;
            velocity.x *= -1;
        }
        else //if hit top or bottom wall
        {
            Vector2 wallNormal = collision.contacts[0].normal;
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
