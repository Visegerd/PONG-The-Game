using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed; //player speed multiplier
    private Rigidbody rb; //reference to rigidbody

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(rb.velocity.x, Input.GetAxis("Vertical") * speed); //apply raw velocity to player (addforce is less convienent)
        if(Input.GetAxis("Start")>0.0f) //if we press "Start" button, game will start
        {
            GameManager.Instance.LaunchBall();
        }
    }
}
