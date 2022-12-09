using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
    public float Speed = 0.8f;
    public float LightTimer;
    private bool canDoubleJump;
    [SerializeField] private LayerMask platformsLayerMask;
    private Rigidbody2D rigidbody2d;
    private GameController gameController;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2d;
    public GameObject TeleportLocation1;
    public GameObject TeleportLocation2;
    public GameObject TeleportLocation3;
    public GameObject CircleLight;
    public Animator MyAnimator;
    public AudioClip MainMusic;
    public AudioClip JumpSound;
    public AudioClip ItemSound;
    public AudioClip ShrinkSound;
    public AudioClip DeathSound;
    public AudioClip WinSound;
    public AudioClip OpenSound;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        gameController = GameObject.FindObjectOfType<GameController>().GetComponent<GameController>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        LightTimer = 0f;
        transform.position = gameController.spawnLocation.transform.position;
        AudioSource.PlayClipAtPoint(MainMusic, transform.position, 0.70f);
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
                AudioSource.PlayClipAtPoint(JumpSound, Camera.main.transform.position, 0.45f);
                MyAnimator.SetTrigger("Jump");
            }
            else
            {
                if (canDoubleJump)
                {
                    float jumpVelocity = 3f;
                    rigidbody2d.velocity = Vector2.up * jumpVelocity;
                    canDoubleJump = false;
                    AudioSource.PlayClipAtPoint(JumpSound, Camera.main.transform.position, 0.45f);
                    MyAnimator.SetTrigger("Jump");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            transform.localScale = new Vector2(.30f, .30f);
            AudioSource.PlayClipAtPoint(ShrinkSound, transform.position, 0.45f);
            MyAnimator.SetBool("Shrink", true);
        }

        float direction = Input.GetAxis("Horizontal");
        transform.Translate(direction * Vector2.right * Speed * Time.deltaTime);
        if (direction == 0)
        {
            MyAnimator.SetBool("Walk", false);
        }
        else
        {
            MyAnimator.SetBool("Walk", true);
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
            MyAnimator.SetBool("Shrink", false);
        }
        if(LightTimer >= 0)
        {
            LightTimer = LightTimer - Time.deltaTime; 
        }
        if (LightTimer <= 5f)
        {
            CircleLight.SetActive(false);
        }
        if (LightTimer >= 50f)
        {
            LightTimer -= 35;
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
            CircleLight.SetActive(true);
            LightTimer += 20;
            AudioSource.PlayClipAtPoint(ItemSound, transform.position, .45f);
        }

       if (collision.gameObject.tag == "level2")
        {
            Destroy(collision.gameObject);
            Speed += 0.5f;
            AudioSource.PlayClipAtPoint(ItemSound, transform.position, .45f);
        }

       if (collision.gameObject.tag == "level3")
        {
            Destroy(collision.gameObject);
            gameController.IncreaseLife();
            AudioSource.PlayClipAtPoint(ItemSound, transform.position, .45f);
        }

       if (collision.gameObject.tag == "Death")
        {
            Destroy(collision.gameObject);
            gameController.UpdateLives();
            AudioSource.PlayClipAtPoint(DeathSound, Camera.main.transform.position, .45f);
            AudioSource.PlayClipAtPoint(MainMusic, Camera.main.transform.position, 0f);
            transform.position = gameController.spawnLocation.transform.position;
        }

       if (collision.gameObject.tag == "Death1")
        {
            Destroy(collision.gameObject);
            gameController.UpdateLives();
            AudioSource.PlayClipAtPoint(DeathSound, Camera.main.transform.position, .45f);
            AudioSource.PlayClipAtPoint(MainMusic, Camera.main.transform.position, 0f);
            transform.position = TeleportLocation1.transform.position;
        }

       if (collision.gameObject.tag == "Death2")
        {
            Destroy(collision.gameObject);
            gameController.UpdateLives();
            AudioSource.PlayClipAtPoint(DeathSound, Camera.main.transform.position, .45f);
            AudioSource.PlayClipAtPoint(MainMusic, Camera.main.transform.position, 0f);
            transform.position = TeleportLocation2.transform.position;
        }

       if (collision.gameObject.tag == "Death3")
        {
            Destroy(collision.gameObject);
            gameController.UpdateLives();
            AudioSource.PlayClipAtPoint(DeathSound, Camera.main.transform.position, .45f);
            AudioSource.PlayClipAtPoint(MainMusic, Camera.main.transform.position, 0f);
            transform.position = TeleportLocation3.transform.position;
        }

       if (collision.gameObject.tag == "BigDeath1")
        {
            Destroy(collision.gameObject);
            gameController.DecreaseLife();
            AudioSource.PlayClipAtPoint(DeathSound, Camera.main.transform.position, .45f);
            AudioSource.PlayClipAtPoint(MainMusic, Camera.main.transform.position, 0f);
            transform.position = TeleportLocation1.transform.position;
        }

       if (collision.gameObject.tag == "BigDeath2")
        {
            Destroy(collision.gameObject);
            gameController.DecreaseLife();
            AudioSource.PlayClipAtPoint(DeathSound, Camera.main.transform.position, .45f);
            AudioSource.PlayClipAtPoint(MainMusic, Camera.main.transform.position, 0f);
            transform.position = TeleportLocation2.transform.position;

        }

       if (collision.gameObject.tag == "Ending")
        {
            gameController.WinGame();
            AudioSource.PlayClipAtPoint(WinSound, Camera.main.transform.position, .45f);
            AudioSource.PlayClipAtPoint(MainMusic, Camera.main.transform.position, 0f);
        }

       if (collision.gameObject.tag == "Gold")
        {
            transform.position = TeleportLocation1.transform.position;
            LightTimer = 0;
        }

       if (collision.gameObject.tag == "Door1")
        {
            SceneManager.LoadScene(0);
        }

       if (collision.gameObject.tag == "Door2")
        {
            transform.position = TeleportLocation2.transform.position;
        }

       if (collision.gameObject.tag == "Door3")
        {
            transform.position = TeleportLocation3.transform.position;
        }
       
       if (collision.gameObject.tag == "Secret")
        {
            spriteRenderer.color = Color.magenta;
            AudioSource.PlayClipAtPoint(OpenSound, Camera.main.transform.position, .45f);
            Destroy(collision.gameObject);
        }
    }
}
