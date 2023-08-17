using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourID, IData
{
    // Start is called before the first frame update
    [HideInInspector]public float isInteract;
    public PhysicsMaterial2D nofriction;
    public float speed;
    public float jumpForce;
    public AudioSource audioSource;
    public PlayerSound playerSound;
    public GameObject deadEffect;

    private float horizontalInput;
    [HideInInspector] public Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    [HideInInspector]
    private PlayerInput input;
    private CapsuleCollider2D playerCollider;
    private Quaternion rotation;
    private Vector3 scale;
    private LayerMask layerGround;
    
    private void OnEnable()
    {
        input.Enable();
    }
    public void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        playerCollider = gameObject.GetComponent<CapsuleCollider2D>();
        layerGround = GameManager.instance.layerGround;
        input = KeyboardManager.input;
        rotation = transform.rotation;
        scale = transform.localScale;
        
    }
    private void Start()
    {
        input.normal.Jump.performed += Jump;
        if(DataManager.instance.hasData)
        LoadData(DataManager.instance.gameData);
    }
    private void OnDisable()
    {
        input.normal.Jump.performed -= Jump;
    }
    private void Jump(InputAction.CallbackContext e)
    {
        if (isGrounded() && Time.timeScale != 0)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    public void Update()
    {
        if (!isGrounded() && Time.timeScale != 0)
        {
            Falling();
        }
    }
    private void FixedUpdate()
    {
        Running();
    }
    private void Running()
    {
        horizontalInput = input.normal.Walk.ReadValue<float>();
        rb.velocity = new Vector2(horizontalInput * speed * Time.fixedDeltaTime, rb.velocity.y);
        if (horizontalInput > 0)
        {
            anim.SetBool("playerRun", true);
            sr.flipX = false;
        }
        else if (horizontalInput < 0)
        {

            anim.SetBool("playerRun", true);
            sr.flipX = true;
        }
        
        else StopRunning();
    }
    private void StopRunning()
    {
        anim.SetBool("playerRun", false);
    }
    private void Falling()
    {
        if (rb.velocity.y > 0.05f)
        {
            anim.SetBool("playerJump", true);
            anim.SetBool("playerFall", false);
        }
        else
        {
            anim.SetBool("playerJump", false);
            anim.SetBool("playerFall", true);
        }
    }
    private bool isGrounded()
    {
        //RaycastHit2D raycastHit = Physics2D.Raycast(gameObject.GetComponent<CapsuleCollider2D>().bounds.center, Vector2.down, gameObject.GetComponent<CapsuleCollider2D>().bounds.extents.y + 0.2f,layerGround);
        RaycastHit2D raycastHitGround = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size - new Vector3(0.15f, 0, 0), 0f, Vector2.down, 0.02f, layerGround);
        if (raycastHitGround.collider == null)
        {
            rb.sharedMaterial = nofriction;
            return false;
        }
        else
        {
            rb.sharedMaterial = null;
            anim.SetBool("playerJump", false);
            anim.SetBool("playerFall", false);
            return true;
        }
    }
    public void Dead()
    {
        if (!GameManager.instance.isGameOver)
        {
            audioSource.mute = true;
            Instantiate(deadEffect, gameObject.transform.position + new Vector3(0, 1f, 0), deadEffect.transform.rotation);
            GameManager.instance.GameOver();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("Elevator") || collision.transform.CompareTag("Elevator"))
        {
            transform.parent = collision.transform;
        }
        if (collision.gameObject.CompareTag("Wooden"))
        {
            playerSound.currentWalkingSound = playerSound.walkClip[(int)SoundType.Wooden];
            playerSound.currentJumpingSound = playerSound.jumpClip[(int)SoundType.Wooden];
        }
        if (collision.gameObject.CompareTag("Elevator"))
        {
            playerSound.currentJumpingSound = playerSound.jumpClip[(int)SoundType.Metal];
            playerSound.currentWalkingSound = playerSound.walkClip[(int)SoundType.Metal];
        }
        if (collision.gameObject.CompareTag("Metal"))
        {
            playerSound.currentJumpingSound = playerSound.jumpClip[(int)SoundType.Metal];
            playerSound.currentWalkingSound = playerSound.walkClip[(int)SoundType.Metal];
        }
        if (collision.gameObject.CompareTag("Grass"))
        {
            playerSound.currentWalkingSound = playerSound.walkClip[(int)SoundType.Grass];
            playerSound.currentJumpingSound = playerSound.jumpClip[(int)SoundType.Grass];
        }
        if (collision.gameObject.CompareTag("Portal"))
        {
            playerSound.currentJumpingSound = playerSound.jumpClip[(int)SoundType.Metal];
            playerSound.currentWalkingSound = playerSound.walkClip[(int)SoundType.Metal];
        }
        if (collision.gameObject.CompareTag("PressurePad"))
        {
            playerSound.currentJumpingSound = playerSound.jumpClip[(int)SoundType.Metal];
            playerSound.currentWalkingSound = playerSound.walkClip[(int)SoundType.Metal];
        }
    }
    private void OnTriggerEnter2D(Collider collision)
    {
        if (collision.transform.root.CompareTag("Elevator") || collision.transform.CompareTag("Elevator"))
        {
            transform.parent = collision.transform;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Wooden"))
        {
            playerSound.currentWalkingSound = playerSound.walkClip[(int)SoundType.Wooden];
            playerSound.currentJumpingSound = playerSound.jumpClip[(int)SoundType.Wooden];
        }
        if (collision.gameObject.CompareTag("Elevator"))
        {
            playerSound.currentJumpingSound = playerSound.jumpClip[(int)SoundType.Metal];
            playerSound.currentWalkingSound = playerSound.walkClip[(int)SoundType.Metal];
        }
        if (collision.gameObject.CompareTag("Metal"))
        {
            playerSound.currentJumpingSound = playerSound.jumpClip[(int)SoundType.Metal];
            playerSound.currentWalkingSound = playerSound.walkClip[(int)SoundType.Metal];
        }
        if (collision.gameObject.CompareTag("Grass"))
        {
            playerSound.currentWalkingSound = playerSound.walkClip[(int)SoundType.Grass];
            playerSound.currentJumpingSound = playerSound.jumpClip[(int)SoundType.Grass];
        }
        if (collision.gameObject.CompareTag("Portal"))
        {
            playerSound.currentJumpingSound = playerSound.jumpClip[(int)SoundType.Metal];
            playerSound.currentWalkingSound = playerSound.walkClip[(int)SoundType.Metal];
        }
        if (collision.gameObject.CompareTag("PressurePad"))
        {
            playerSound.currentJumpingSound = playerSound.jumpClip[(int)SoundType.Metal];
            playerSound.currentWalkingSound = playerSound.walkClip[(int)SoundType.Metal];
        }
        if (collision.transform.root.CompareTag("Elevator"))
        {
            if (!gameObject.activeInHierarchy == false)
            {
                transform.parent = collision.transform.root;
                if(!collision.transform.root.GetComponent<AudioSource>().isPlaying)
                collision.transform.root.GetComponent<AudioSource>().Play();
                transform.rotation = rotation;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("Elevator"))
        {
            if (!gameObject.activeInHierarchy == false)
            {
                collision.transform.root.GetComponent<AudioSource>().Stop();
                transform.parent = null;
                transform.localScale = scale;
                transform.rotation = rotation;
            }
        }
    }

    public void LoadData(GameData data)
    {
        transform.position = data.playerData.position;
        transform.localScale = data.playerData.scale;
        rb.velocity = data.playerData.velocity;
    }

    public void SaveData(GameData data)
    {
        data.playerData.position = transform.position;
        data.playerData.scale = transform.localScale;
        data.playerData.velocity = rb.velocity;
    }
}
