using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject appear;
    static float speed;
    public float jump;
    bool onGround = true;
    public Animator animator;
    public Canvas canvas;
    public GameObject player;
    public GameObject dead;
    public static PlayerController instance;
    int count = 0;
    static GameObject Boom;
    public GameObject bom;
    int isBom = 0;
    static float localScale;
    bool isOn = false;
    public GameObject shield;
    public float shieldTime;
    public float shielddelay;
    static int isShield = 0;
    float tmp;
    public GameObject go;
    Vector2 pos;
    public GameObject hurt;
    // Start is called before the first frame update
    void Start()
    {
       
        instance = this;
        if (SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 4 || SceneManager.GetActiveScene().buildIndex == 27 || SceneManager.GetActiveScene().buildIndex == PlayerPrefs.GetInt("level"))
        {
            isBom = 0;
            localScale = 6;
            speed = 3;
            isShield = 0;
        }
        else if(SceneManager.GetActiveScene().buildIndex == PlayerPrefs.GetInt("level"))
        {
            isBom = PlayerPrefs.GetInt("isBom");
            localScale = PlayerPrefs.GetFloat("localScale");
            speed = PlayerPrefs.GetFloat("speed");
            isShield = PlayerPrefs.GetInt("isShield");
        }
        else
        {
            isBom = PlayerPrefs.GetInt("isBom");
            localScale = PlayerPrefs.GetFloat("localScale");
            speed = PlayerPrefs.GetFloat("speed");
            isShield = PlayerPrefs.GetInt("isShield");

        }
       
        tmp = speed;
        StartCoroutine(appe());
        this.transform.position = new Vector3(-7, -4.02f, 0);
        if(isBom == 0)
        {
            Boom = bom;
        }
        else
        {
            Boom = go;
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        pos = Checker.instance.position();
        if(isOn == true)
        {
            Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
            if (Camera.main.orthographicSize < 6.4f)
            {
                Camera.main.orthographicSize += 2.5f * Time.deltaTime;
                Camera.main.transform.localScale = Vector3.one * Camera.main.orthographicSize / 5;
            }
            
            else
            {
                Camera.main.orthographicSize = 6.4f;
                Camera.main.transform.localScale = Vector3.one * 6.4f / 5;
                
            }
           
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(transform.up * jump);
                StartCoroutine(boom(this.gameObject));
                
            }
            if(rb.velocity.x != 0)
            {
                rb.velocity = new Vector2(0,rb.velocity.y);
            }
            this.transform.position += new Vector3(5*Time.deltaTime, 0, 0);
            Camera.main.transform.position = player.transform.position + new Vector3(0, 2, -100);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            if (Camera.main.orthographicSize > 5)
            {
                Camera.main.orthographicSize -= 5 * Time.deltaTime;
                Camera.main.transform.localScale = Vector3.one * Camera.main.orthographicSize / 5;
            }

            else
            {
                Camera.main.orthographicSize = 5;
                Camera.main.transform.localScale = Vector3.one * 5 / 5;
            }

        }
        
        if (isOn == false)
        {

            Camera.main.transform.position = player.transform.position + new Vector3(0, 2, -100);
            this.transform.localScale = Vector3.one * localScale;
            onGround = Checker.instance.onground();
            if (transform.rotation.eulerAngles.y == 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                if (animator.GetBool("isMoving") == false) animator.SetBool("isMoving", true);
                transform.rotation = Quaternion.Euler(0, 180, 0);
                this.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                Camera.main.transform.position = player.transform.position + new Vector3(0, 2, -100);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (animator.GetBool("isMoving") == false) animator.SetBool("isMoving", true);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                this.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                Camera.main.transform.position = player.transform.position + new Vector3(0, 2, -100);
            }

            else
            {
                animator.SetBool("isMoving", false);
            }
            Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
            if (rb.velocity.x != 0)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            if (Input.GetKeyDown(KeyCode.Space) && onGround == true)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(transform.up * jump);
                count = 1;
                Camera.main.transform.position = player.transform.position + new Vector3(0, 2, -100);
            }
            if (onGround == true)
            {
                animator.SetBool("isOnAir", false);

            }
            else
            {
                animator.SetBool("isOnAir", true);
                if (Input.GetKeyDown(KeyCode.Space) && count == 1 && Projectile.instance.Energy >= 0f)
                {
                    rb.AddForce(transform.up * jump*3/4);
                    if(rb.velocity.y <= 0)
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    else
                    {
                        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*3/4);
                    }
                    count--;
                    Projectile.instance.Energy -= 1f;
                    StartCoroutine(boom(this.gameObject));
                    Camera.main.transform.position = player.transform.position + new Vector3(0, 2, -100);
                }
            }
            if (transform.position.y <= -50)
            {
                HP.instance.loseHP(1);
                this.transform.position = pos;
            }
        }
       
        if(HP.instance.life() <= 0)
        {
            PlayerPrefs.SetInt("level", 4);
            StartCoroutine(die());
        }
        shielddelay -= Time.deltaTime;
        if(shielddelay <= 0 && isShield == 1)
        {
            shield.SetActive(true);
        }
        PlayerPrefs.SetInt("isBom", isBom);
        PlayerPrefs.SetFloat("localScale", localScale);
        PlayerPrefs.SetFloat("speed", speed);
        PlayerPrefs.SetInt("isShield", isShield);
    }
    public void Hurt()
    {
        if (shield.activeSelf == true && isShield == 1)
        {
            shield.SetActive(false);
            StartCoroutine(boom(shield));
            shielddelay = shieldTime;
        }
        else
        {
            StartCoroutine(anim());
        }
        
          
    }
    IEnumerator anim()
    {
        animator.SetBool("Hurt", true);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.zero;
        Instantiate(hurt,this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.21f);
        animator.SetBool("Hurt", false) ;
    }
    public Rigidbody2D rb()
    {
        Rigidbody2D rb2 = this.GetComponent<Rigidbody2D>();
        return rb2 ;
    }
    public float direct()
    {
        return this.transform.rotation.eulerAngles.y;
    }
    IEnumerator die()
    {
        Destroy(canvas);
        GameObject rb = Instantiate(dead, transform);
        yield return new WaitForSeconds(0.2f);
        Destroy(rb);
        
        Destroy(gameObject);
    }
    IEnumerator boom(GameObject i)
    {
       
        
         GameObject rb = Instantiate(Boom, i.transform.position, transform.rotation);
         yield return new WaitForSeconds(0.17f);
         Destroy(rb);
        
    }
    IEnumerator appe()
    {
        GameObject rb = Instantiate(appear, new Vector3(-7, -4.02f, 0), appear.transform.rotation);
        yield return rb;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "On" && isOn == false)
        {
            isOn = true;
        }
        
    }
    public void size(float size)
    {
        localScale = size;
    }
    public void run(float run)
    {
        speed = run;
    }
    public float getrun() {
        return tmp;
    }
    public void change()
    {
        Boom = go;
        isBom = 1;
    }
    public void Shid()
    {
        isShield = 1;
    }
}
