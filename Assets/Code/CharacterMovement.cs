using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float Speed = 5f;
    public Rigidbody2D rb;
    public Vector2 JumpForce = new Vector2(0, 1250);
    private GameController gc;
    private SpriteRenderer lg;
    public GameObject TpLocation1;
    public GameObject TpLocation2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gc = GameObject.FindObjectOfType<GameController>().GetComponent<GameController>();
        lg = GetComponent<SpriteRenderer>();
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
            transform.localScale = new Vector2(5f, 5f);
        }

        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime);
            lg.flipX = false;
        }

        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * Speed * Time.deltaTime);
            lg.flipX = true;
        }

        if(!Input.GetKey(KeyCode.LeftShift))
        {
            transform.localScale = new Vector2(10f, 10f);
        }  
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.tag == "level1")
        {
            Destroy(collision.gameObject);
            Destroy(gc.Darkness);
        }

       if (collision.gameObject.tag == "level2")
        {
            Destroy(collision.gameObject);
            Speed += 5f;
        }

       if (collision.gameObject.tag == "level3")
        {
            Destroy(collision.gameObject);
            gc.IncreaseLife();
        }

       if (collision.gameObject.tag == "Death")
        {
            Destroy(collision.gameObject);
            gc.UpdateLives();
        }

       if (collision.gameObject.tag == "BigDeath")
        {
            Destroy(collision.gameObject);
            gc.DecreaseLife();
        }

       if (collision.gameObject.tag == "Ending")
        {
            gc.WinGame();
        }

       if (collision.gameObject.tag == "Door1")
        {
            transform.position = TpLocation1.transform.position;
        }

       if (collision.gameObject.tag == "Door2")
        {
            transform.position = TpLocation2.transform.position;
        }
    }
}
