using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float Speed = 0.8f;
    public float lightTimer;
    private bool canDoubleJump;
    [SerializeField] private LayerMask platformsLayerMask;
    private Rigidbody2D rigidbody2d;
    private GameController gameController;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2d;
    public GameObject teleportLocation1;
    public GameObject teleportLocation2;
    public GameObject teleportLocation3;
    public GameObject circleLight;
    public Animator myAnimator;
    public AudioClip mainMusic;
    public AudioClip jumpSound;
    public AudioClip itemSound;
    public AudioClip shrinkSound;
    public AudioClip deathSound;
    public AudioClip winSound;
    public AudioClip openSound;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        gameController = GameObject.FindObjectOfType<GameController>().GetComponent<GameController>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        lightTimer = 20f;
        transform.position = gameController.spawnLocation.transform.position;
        AudioSource.PlayClipAtPoint(mainMusic, transform.position, 0.60f);
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
                AudioSource.PlayClipAtPoint(jumpSound, Camera.main.transform.position, 0.45f);
                myAnimator.SetTrigger("Jump");
            }
            else
            {
                if (canDoubleJump)
                {
                    float jumpVelocity = 3f;
                    rigidbody2d.velocity = Vector2.up * jumpVelocity;
                    canDoubleJump = false;
                    AudioSource.PlayClipAtPoint(jumpSound, Camera.main.transform.position, 0.45f);
                    myAnimator.SetTrigger("Jump");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            transform.localScale = new Vector2(.30f, .30f);
            AudioSource.PlayClipAtPoint(shrinkSound, transform.position, 0.45f);
            myAnimator.SetBool("Shrink", true);
        }

        float direction = Input.GetAxis("Horizontal");
        transform.Translate(direction * Vector2.right * Speed * Time.deltaTime);
        if (direction == 0)
        {
            myAnimator.SetBool("Walk", false);
        }
        else
        {
            myAnimator.SetBool("Walk", true);
        }

        if (direction < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if( direction > 0)
        {
            spriteRenderer.flipX = false;
        }

        if(!Input.GetKey(KeyCode.LeftShift))
        {
            transform.localScale = new Vector2(1f, 1f);
            myAnimator.SetBool("Shrink", false);
        }

        lightTimer = lightTimer - Time.deltaTime;
        if (lightTimer == 0f)
        {
            circleLight.SetActive(false);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        return raycastHit2d.collider != null;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.tag == "level1")
        {
            Destroy(collision.gameObject);
            circleLight.SetActive(true);
            lightTimer += 15;
            AudioSource.PlayClipAtPoint(itemSound, transform.position, .45f);
        }

       if (collision.gameObject.tag == "level2")
        {
            Destroy(collision.gameObject);
            Speed += 0.5f;
            AudioSource.PlayClipAtPoint(itemSound, transform.position, .45f);
        }

       if (collision.gameObject.tag == "level3")
        {
            Destroy(collision.gameObject);
            gameController.IncreaseLife();
            AudioSource.PlayClipAtPoint(itemSound, transform.position, .45f);
        }

       if (collision.gameObject.tag == "Death")
        {
            Destroy(collision.gameObject);
            gameController.UpdateLives();
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, .45f);
            AudioSource.PlayClipAtPoint(mainMusic, Camera.main.transform.position, 0f);
            transform.position = gameController.spawnLocation.transform.position;
        }

       if (collision.gameObject.tag == "Death1")
        {
            Destroy(collision.gameObject);
            gameController.UpdateLives();
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, .45f);
            AudioSource.PlayClipAtPoint(mainMusic, Camera.main.transform.position, 0f);
            transform.position = teleportLocation1.transform.position;
        }

       if (collision.gameObject.tag == "Death2")
        {
            Destroy(collision.gameObject);
            gameController.UpdateLives();
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, .45f);
            AudioSource.PlayClipAtPoint(mainMusic, Camera.main.transform.position, 0f);
            transform.position = teleportLocation2.transform.position;
        }

       if (collision.gameObject.tag == "Death3")
        {
            Destroy(collision.gameObject);
            gameController.UpdateLives();
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, .45f);
            AudioSource.PlayClipAtPoint(mainMusic, Camera.main.transform.position, 0f);
            transform.position = teleportLocation3.transform.position;
        }

       if (collision.gameObject.tag == "BigDeath1")
        {
            Destroy(collision.gameObject);
            gameController.DecreaseLife();
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, .45f);
            AudioSource.PlayClipAtPoint(mainMusic, Camera.main.transform.position, 0f);
            transform.position = gameController.spawnLocation.transform.position;
        }

       if (collision.gameObject.tag == "BigDeath2")
        {
            Destroy(collision.gameObject);
            gameController.DecreaseLife();
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, .45f);
            AudioSource.PlayClipAtPoint(mainMusic, Camera.main.transform.position, 0f);
            transform.position = teleportLocation1.transform.position;

        }

       if (collision.gameObject.tag == "Ending")
        {
            gameController.WinGame();
            AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position, .45f);
            AudioSource.PlayClipAtPoint(mainMusic, Camera.main.transform.position, 0f);
        }

       if (collision.gameObject.tag == "Gold")
        {
            transform.position = teleportLocation1.transform.position;
        }

       if (collision.gameObject.tag == "Door2")
        {
            transform.position = teleportLocation2.transform.position;
        }

       if (collision.gameObject.tag == "Door3")
        {
            transform.position = teleportLocation3.transform.position;
        }
       
       if (collision.gameObject.tag == "Secret")
        {
            spriteRenderer.color = Color.magenta;
            AudioSource.PlayClipAtPoint(openSound, Camera.main.transform.position, .45f);
            Destroy(collision.gameObject);
        }
    }
}
