using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    private BoxCollider col;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponentInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(rb.velocity.x, Input.GetAxis("Vertical") * speed);
        if(Input.GetAxis("Start")>0.0f)
        {
            GameManager.Instance.LaunchBall();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.CompareTag("Ball"))
        //{
        //    StartCoroutine(TempTurnOffCollider());
        //}
    }

    //IEnumerator TempTurnOffCollider()
    //{
    //    col.enabled = false;
    //    yield return new WaitForEndOfFrame();
    //    col.enabled = true;
    //}
}
