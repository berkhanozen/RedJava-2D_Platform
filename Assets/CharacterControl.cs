using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    Rigidbody2D phy;
    Vector3 vec;
    public Sprite[] waitAnim;
    public Sprite[] jumpAnim;
    public Sprite[] runAnim;
    SpriteRenderer spriteRenderer;
    float horizontal = 0;
    bool onejump = true;
    int twojump = 0;
    int waitanimsayac = 0;
    int runanimsayac = 0;
    float waitanimtime = 0;
    float runanimtime = 0;
    Vector3 camFirstPos;
    Vector3 camLastPos;
    new GameObject camera;
    int health = 100;
    public Text healthtext;

    void Start()
    {
        phy = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        camFirstPos = camera.transform.position - transform.position;
    }
    void Update()
    {
        healthtext.text = "Can: " + health;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(onejump == true)
            {
                phy.velocity = new Vector2(0, 0);
                phy.AddForce(new Vector2(0, 1250));
                twojump++;
                if(twojump > 1)
                {
                    onejump = false;
                }
            }
            
        }
    }
    void FixedUpdate()
    {
        CharMovement();
        Animation();
    }
    void LateUpdate()
    {
        CameraControl();
    }
    void CameraControl()
    {
        camLastPos = camFirstPos + transform.position;
        camera.transform.position = Vector3.Lerp(camera.transform.position, camLastPos, 0.01f);
    }

    void CharMovement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vec = new Vector3(horizontal*10, phy.velocity.y,0);
        phy.velocity = vec;
        
    }
    void Animation()
    {
        if(onejump)
        {
            if (horizontal == 0)
            {
                WaitAnim();
            }
            else if (horizontal > 0)
            {
                RunAnim();
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontal < 0)
            {
                RunAnim();
                transform.localScale = new Vector3(-1, 1, 1);

            }
        }
        if(twojump > 0)
        {
            JumpAnim();
            if (horizontal > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontal < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
    void RunAnim()
    {
        runanimtime += Time.deltaTime;
        if (runanimtime > 0.01f)
        {
            spriteRenderer.sprite = runAnim[runanimsayac++];
            {
                if (runanimsayac >= runAnim.Length)
                {
                    runanimsayac = 0;
                }
                runanimtime = 0;
            }
        }
    }
    void WaitAnim()
    {
        waitanimtime += Time.deltaTime;
        if (waitanimtime > 0.05f)
        {
            spriteRenderer.sprite = waitAnim[waitanimsayac++];
            if (waitanimsayac >= waitAnim.Length)
            {
                waitanimsayac = 0;
            }
            waitanimtime = 0;
        }
    }
    void JumpAnim()
    {
        if(phy.velocity.y > 0)
        {
            spriteRenderer.sprite = jumpAnim[0];
        }
        else
        {
            spriteRenderer.sprite = jumpAnim[1];
        }
            
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        onejump = true;
        twojump = 0;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            health -= 5;
        }
        if (col.gameObject.tag == "Enemy")
        {
            health -= 10;
        }
        if (col.gameObject.tag == "Saw")
        {
            health -= 20;
        }
    }
}
