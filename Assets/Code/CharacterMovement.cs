using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float Speed = 5f;
    public Rigidbody2D rb;
    public Vector2 JumpForce = new Vector2(0, 1250);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.AddForce(JumpForce);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.down * Speed * Time.deltaTime);
        }

        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime);
        }

        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * Speed * Time.deltaTime);
        }    
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.tag == "Obstacle")
        {
            print("poop");
        }
    }
}
