using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager GameManager;
    public CameraMovement CameraMovement;
    public ParticleBGMovement ParticleBGMovement;

    public string mode;
    public string tempMode;

    // ------------------------------------------------------

    // general
    public float playerSpeed;
    public float jumpForce;
    public float velocityCap;
    public int gravity;
    public int gravTemp;
    public bool level1Complete, level2Complete;

    public Vector3 tempTransform;

    // ground check
    public LayerMask groundMask;
    public Transform groundTransform;
    public float groundCheckWidth = 1f;
    public float groundCheckHeight = 0.1f;
    public bool isGrounded;

    // bump check
    public LayerMask bumpMask;
    public Transform bumpTransform;
    public bool isBumpable;

    // wall check
    public Transform wallTransform;
    public float wallCheckWidth = 0.2f;
    public float wallCheckHeight = 0.8f;
    private bool hitsWall;

    // rotate
    public Transform squareTransform;
    public Transform ballTransform;
    public Transform arrowTransform;
    public float rotationSpeed;

    private Rigidbody2D rb2d;
    private TrailRenderer tr2d;

    private BoxCollider2D squareCollider;
    private CircleCollider2D circleCollider;
    private PolygonCollider2D arrowCollider;

    private SpriteRenderer squareSpriteRen;
    private SpriteRenderer squareGlowSpriteRen;
    private SpriteRenderer ballSpriteRen;
    private SpriteRenderer ballGlowSpriteRen;
    private SpriteRenderer arrowSpriteRen;
    private SpriteRenderer arrowGlowSpriteRen;


    public GameObject Square;
    public GameObject Bounce;
    public GameObject Arrow;
    public GameObject SquareGlow;
    public GameObject BounceGlow;
    public GameObject ArrowGlow;

    public GameObject Explosion;
    public GameObject Skid;

    // ------------------------------------------------------

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        tr2d = GetComponent<TrailRenderer>();

        squareCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        arrowCollider = GetComponent<PolygonCollider2D>();

        //tempTransform = transform.position; // delete me

        squareSpriteRen = Square.GetComponent<SpriteRenderer>();
        squareGlowSpriteRen = SquareGlow.GetComponent<SpriteRenderer>();
        ballSpriteRen = Bounce.GetComponent<SpriteRenderer>();
        ballGlowSpriteRen = BounceGlow.GetComponent<SpriteRenderer>();
        arrowSpriteRen = Arrow.GetComponent<SpriteRenderer>();
        arrowGlowSpriteRen = ArrowGlow.GetComponent<SpriteRenderer>();
    }

// ------------------------------------------------------

    private void Start()
    {
        mode = "Inactive";
        transform.position = new Vector3(-17, 2, 0); //-- turn me back on

        tr2d.emitting = false;

        level1Complete = false;
        level2Complete = false;

        squareGlowSpriteRen.color = new Color(0.0f, 1.0f, 0.8f, 1.0f); //Green
    }

// ------------------------------------------------------

    void FixedUpdate()
    {
        rb2d.gravityScale = 12 * gravity;

        isBumpable = Physics2D.OverlapBox(bumpTransform.position, bumpTransform.localScale, 0, bumpMask);
        hitsWall = Physics2D.OverlapBox(wallTransform.position, new Vector2(wallCheckWidth, wallCheckHeight), 0, groundMask);

        if (isGrounded == true)
            Skid.GetComponent<ParticleSystem>().Play();

        if (mode == "Regular")
        {
            isGrounded = Physics2D.OverlapBox(groundTransform.position, new Vector2(groundCheckWidth, groundCheckHeight), 0, groundMask);
            squareCollider.enabled = true;
            circleCollider.enabled = false;
            arrowCollider.enabled = false;
        }

        if (mode == "Bounce")
        {
            //isGrounded = Physics2D.OverlapBox(groundTransform.position, new Vector2(groundCheckWidth, groundCheckHeight), 0, groundMask);
            isGrounded = false;

            squareCollider.enabled = false;
            circleCollider.enabled = true;
            arrowCollider.enabled = false;
        }

        if (mode == "UpsideDown")
        {
            isGrounded = Physics2D.OverlapBox(new Vector2(groundTransform.position.x, groundTransform.position.y + 1), new Vector2(groundCheckWidth, groundCheckHeight), 0, groundMask);
            squareCollider.enabled = true;
            circleCollider.enabled = false;
            arrowCollider.enabled = false;
        }

        if (mode == "Switch")
        {
            isGrounded = Physics2D.OverlapBox(bumpTransform.position, new Vector2(bumpTransform.localScale.x, bumpTransform.localScale.y + 0.2f), 0, groundMask);
            squareCollider.enabled = true;
            circleCollider.enabled = false;
            arrowCollider.enabled = false;
        }

        if (mode == "Arrow")
        {
            isGrounded = false;
            squareCollider.enabled = false;
            circleCollider.enabled = false;
            arrowCollider.enabled = true;
        }
    }

// ------------------------------------------------------

    void Update()
    {
        // regular square mode
        if (mode == "Regular")
        {
            gravity = 1;
            jumpForce = 25;
            playerSpeed = 10;
            velocityCap = -25;
            rotationSpeed = 500;

            if (Input.GetKey(KeyCode.Space) && isGrounded == true && transform.position.x >= -6)
                Jump();

            if (Input.GetKeyDown(KeyCode.Space) && isBumpable == true)
                Bump();
        }

        // bounce mode
        if (mode == "Bounce")
        {
            gravity = 1;
            jumpForce = 25;
            playerSpeed = 10;
            velocityCap = -20;
            rotationSpeed = 500;

            isBumpable = true;

            if (Input.GetKeyDown(KeyCode.Space) && (isGrounded == true || isBumpable == true))
                Bump();
        }

        // Upside-down mode
        if (mode == "UpsideDown")
        {
            gravity = -1;
            jumpForce = 25;
            playerSpeed = 10;
            velocityCap = -25;
            rotationSpeed = 500;

            if (Input.GetKey(KeyCode.Space) && isGrounded == true)
                Jump();

            if (Input.GetKeyDown(KeyCode.Space) && isBumpable == true)
                Bump();

        }

        // Switch mode
        if (mode == "Switch")
        {
            gravity = gravTemp;
            jumpForce = 25;
            playerSpeed = 10;
            velocityCap = -25;
            rotationSpeed = 500;

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
                gravTemp *= -1;

            if (Input.GetKeyDown(KeyCode.Space) && isBumpable == true)
            {
                gravTemp *= -1;
                Jump();
            }
        }

        // Arrow mode
        if (mode == "Arrow")
        {
            rb2d.velocity = new Vector2(0, 10*gravTemp);
            gravTemp = -1;

            gravity = 0;
            playerSpeed = 10;
            velocityCap = -25;
            rotationSpeed = 0;

            arrowTransform.rotation = Quaternion.Euler(0, 0, -90);

            if (Input.GetKey(KeyCode.Space))
            {
                gravTemp = 1;
                arrowTransform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        // end mode
        if (mode == "End")
        {
            gravity = 1;
            jumpForce = 25;
            playerSpeed = 10;
            velocityCap = -25;
            rotationSpeed = 500;
        }

        // ------------------------------------------------------

        if (mode == "Inactive")
        {
            rb2d.Sleep();

            squareSpriteRen.enabled = false;
            squareGlowSpriteRen.enabled = false;
            ballSpriteRen.enabled = false;
            ballGlowSpriteRen.enabled = false;
            arrowSpriteRen.enabled = false;
            arrowGlowSpriteRen.enabled = false;

            Skid.GetComponent<ParticleSystem>().Stop();
            tr2d.emitting = false;
        }

        else
        {
            rb2d.WakeUp();

            if (mode == "Regular" || mode == "UpsideDown" || mode == "Switch")
            {
                squareSpriteRen.enabled = true;
                squareGlowSpriteRen.enabled = true;

                ballSpriteRen.enabled = false;
                ballGlowSpriteRen.enabled = false;
                arrowSpriteRen.enabled = false;
                arrowGlowSpriteRen.enabled = false;
            }

            if (mode == "Bounce")
            {
                ballSpriteRen.enabled = true;
                ballGlowSpriteRen.enabled = true;

                squareSpriteRen.enabled = false;
                squareGlowSpriteRen.enabled = false;
                arrowSpriteRen.enabled = false;
                arrowGlowSpriteRen.enabled = false;

                ballTransform.Rotate(new Vector3(0, 0, -1) * rotationSpeed * gravity * Time.deltaTime);
            }

            if (mode == "Arrow")
            {
                arrowSpriteRen.enabled = true;
                arrowGlowSpriteRen.enabled = true;

                ballSpriteRen.enabled = false;
                ballGlowSpriteRen.enabled = false;
                squareSpriteRen.enabled = false;
                squareGlowSpriteRen.enabled = false;
            }

            // auto-right movement
            transform.position += new Vector3(1, 0, 0) * playerSpeed * Time.deltaTime;


            // falling speed cap
            if (rb2d.velocity.y < velocityCap)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, velocityCap);
            }

            // snap rotation to nearest 90 on ground
            if (isGrounded == true)
            {
                Vector3 currentRotation = squareTransform.rotation.eulerAngles;
                float snap = Mathf.Round(currentRotation.z / 90) * 90;
                squareTransform.rotation = Quaternion.Euler(0, 0, snap);
            }

            // rotate in air
            else
            {
                squareTransform.Rotate(new Vector3(0, 0, -1) * rotationSpeed * gravity * Time.deltaTime);
                Skid.GetComponent<ParticleSystem>().Stop();
            }

            // wall collision
            if (hitsWall == true)
                Death();
        }
    }

// ------------------------------------------------------

    void OnTriggerEnter2D(Collider2D obstacle)
    {
        if (obstacle.gameObject.CompareTag("Obstacle"))
            Death();

        if (obstacle.gameObject.CompareTag("PortalReg"))
        {
            mode = "Regular";
            tr2d.emitting = false;
            squareGlowSpriteRen.color = new Color(0.0f, 1.0f, 0.8f, 1.0f); //Green
            Shift("Regular");
        }

        if (obstacle.gameObject.CompareTag("PortalBounce"))
        {
            mode = "Bounce";
            tr2d.emitting = false;

            Shift("Bounce");
        }

        if (obstacle.gameObject.CompareTag("PortalUD"))
        {
            mode = "UpsideDown";
            tr2d.emitting = false;
            squareGlowSpriteRen.color = new Color(1.0f, 0.0f, 0.5f, 1.0f); //Pink

            Shift("UpsideDown");
        }

        if (obstacle.gameObject.CompareTag("PortalSwitch"))
        {
            gravTemp = gravity;
            mode = "Switch";
            tr2d.emitting = false;
            squareGlowSpriteRen.color = new Color(0.2f, 0.0f, 1.0f, 1.0f); //Blue

            Shift("Switch");
        }

        if (obstacle.gameObject.CompareTag("PortalArrow"))
        {
            gravTemp = gravity;
            mode = "Arrow";
            tr2d.emitting = true;

            Shift("Arrow");
        }

        if (obstacle.gameObject.CompareTag("PortalMirror"))
            StartCoroutine(CameraMovement.RotateCamera());

        if (obstacle.gameObject.CompareTag("EndCheck1"))
        {
            mode = "End";
            if (GameManager.State == "Level1")
                level1Complete = true;
            if (GameManager.State == "Level2")
                level2Complete = true;
        }

        if (obstacle.gameObject.CompareTag("EndCheck2"))
        {
            mode = "Inactive";
            StartCoroutine(LevelComplete());
        }

        if (obstacle.gameObject.CompareTag("Boost") && mode != "Switch")
        {
            Boost();
            StartCoroutine(Trail());
        }

        if (obstacle.gameObject.CompareTag("Boost") && mode == "Switch")
        {
            gravTemp *= -1;
        }
    }

// ------------------------------------------------------

    private void Jump()
    {
        rb2d.velocity = new Vector2(0, 0);
        rb2d.AddForce(new Vector2(0f, 1f) * jumpForce * gravity, ForceMode2D.Impulse);
    }

    private void Bump()
    {
        rb2d.velocity = new Vector2(0, 0);
        rb2d.AddForce(new Vector2(0f, 1f) * jumpForce * gravity, ForceMode2D.Impulse);
        StartCoroutine(Trail());
    }

    private void Boost()
    {
        rb2d.velocity = new Vector2(0, 0);
        rb2d.AddForce(new Vector2(0f, 1f) * (jumpForce * 1.4f) * gravity, ForceMode2D.Impulse);
    }

    IEnumerator Trail()
    {
        tr2d.emitting = true;
        yield return new WaitForSecondsRealtime(0.4f);
        tr2d.emitting = false;
    }

    private void Death()
    {
        GameManager.Instance.PlaySound(GameManager.explosionSound);
        mode = "Inactive";
        GameManager.attemptCount += 1;
        StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        Explosion.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSecondsRealtime(0.8f);

        transform.position = new Vector3(-13, 0.4f, 0);  //--turn me back on
        //transform.position = tempTransform; // delete me
        hitsWall = false;
        Shift("Regular");

        GameObject.FindObjectOfType<HueShift>().BroadcastMessage("ShiftColor", new Color(0.4f, 0, 0.4f, 1));
        mode = "Regular";
        CameraMovement.Reset();
        ParticleBGMovement.Reset();
    }

    IEnumerator LevelComplete()
    {
        GameManager.Instance.PlaySound(GameManager.victorySound);

        yield return new WaitForSecondsRealtime(2f);

        transform.position = new Vector3(-13, 0.4f, 0); // --turn me back on
        //transform.position = tempTransform; // delete me
        GameObject.FindObjectOfType<HueShift>().BroadcastMessage("ShiftColor", new Color(0.4f, 0, 0.4f, 1));
        CameraMovement.Reset();
        ParticleBGMovement.Reset();

        GameManager.State = "LevelSelect";
    }

    public void Shift(string Color)
    {
        var hues = GameObject.FindGameObjectsWithTag("Hue");

        if (Color == "Regular")
        {
            foreach (var hue in hues)
                hue.GetComponent<HueShift>().ShiftColor(new Color(0.4f, 0, 0.4f, 1));
        }


        if (Color == "Bounce")
        {
            foreach (var hue in hues)
                hue.GetComponent<HueShift>().ShiftColor(new Color(0.4f, 0.2f, 0, 1));
        }

        if (Color == "UpsideDown")
        {
            foreach (var hue in hues)
                hue.GetComponent<HueShift>().ShiftColor(new Color(0, 0.4f, 0.4f, 1));
        }

        if (Color == "Switch")
        {
            foreach (var hue in hues)
                hue.GetComponent<HueShift>().ShiftColor(new Color(0.2f, 0.4f, 0, 1));
        }

        if (Color == "Arrow")
        {
            foreach (var hue in hues)
                hue.GetComponent<HueShift>().ShiftColor(new Color(0.3f, 0, 0.5f, 1));
        }
    }
}
