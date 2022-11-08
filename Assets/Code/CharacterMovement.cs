using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float Speed = 0.5f;
    public float lightTimer;
    private bool canDoubleJump;
    [SerializeField] private LayerMask platformsLayerMask;
    public Rigidbody2D rigidbody2d;
    private GameController gameController;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2d;
    public GameObject teleportLocation1;
    public GameObject teleportLocation2;
    public GameObject circleLight;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        gameController = GameObject.FindObjectOfType<GameController>().GetComponent<GameController>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        lightTimer = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded())
        {
            canDoubleJump = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                float jumpVelocity = 3f;
                rigidbody2d.velocity = Vector2.up * jumpVelocity;
            }
            else 
            {
                if (canDoubleJump)
                {
                    float jumpVelocity = 2f;
                    rigidbody2d.velocity = Vector2.up * jumpVelocity;
                    canDoubleJump = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            transform.localScale = new Vector2(.25f, .25f);
        }

        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime);
            spriteRenderer.flipX = false;
        }

        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * Speed * Time.deltaTime);
            spriteRenderer.flipX = true;
        }

        if(!Input.GetKey(KeyCode.LeftShift))
        {
            transform.localScale = new Vector2(.90f, .90f);
        }

        lightTimer = lightTimer - Time.deltaTime;
        if (lightTimer <= 0f)
        {
            circleLight.SetActive(false);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 1f, platformsLayerMask);
        return raycastHit2d.collider != null;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.tag == "level1")
        {
            Destroy(collision.gameObject);
            circleLight.SetActive(true);
            lightTimer += 15;
        }

       if (collision.gameObject.tag == "level2")
        {
            Destroy(collision.gameObject);
            Speed += 0.5f;
        }

       if (collision.gameObject.tag == "level3")
        {
            Destroy(collision.gameObject);
            gameController.IncreaseLife();
        }

       if (collision.gameObject.tag == "Death1")
        {
            Destroy(collision.gameObject);
            gameController.UpdateLives();
            transform.position = gameController.spawnLocation.transform.position;
        }

       if (collision.gameObject.tag == "Death2")
        {
            Destroy(collision.gameObject);
            gameController.UpdateLives();
            transform.position = teleportLocation1.transform.position;
        }

       if (collision.gameObject.tag == "Death3")
        {
            Destroy(collision.gameObject);
            gameController.UpdateLives();
            transform.position = teleportLocation2.transform.position;
        }

       if (collision.gameObject.tag == "BigDeath1")
        {
            Destroy(collision.gameObject);
            gameController.DecreaseLife();
            transform.position = gameController.spawnLocation.transform.position;
        }

       if (collision.gameObject.tag == "BigDeath2")
        {
            Destroy(collision.gameObject);
            gameController.DecreaseLife();
            transform.position = teleportLocation1.transform.position;

        }

       if (collision.gameObject.tag == "Ending")
        {
            gameController.WinGame();
        }

       if (collision.gameObject.tag == "Door1")
        {
            transform.position = teleportLocation1.transform.position;
        }

       if (collision.gameObject.tag == "Door2")
        {
            transform.position = teleportLocation2.transform.position;
        }
    }
}
