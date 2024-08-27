using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class Projectile : MonoBehaviour
{
    public GameObject[] bulletPrefab;
    static GameObject BigbulletPrefab;
    public GameObject[] BigBullets;
    public float offset = 5f;
    public float[] bulletSpeed;
    static float BigbulletSpeed;
    public float reload = 0f;
    public float Bigload = 0f;
    static float reloadTime = 0.5f;
    public float jump;
    static float bigreloadTime;
    public GameObject player;
    public Transform playerTransform;
    public GameObject shot;
    public GameObject Bigshot;
    public Animator Animator;
    static float maxEnergy;
    public float Energy;
    static float Bigcost = 3;
    public float cost;
    public float Bigtime;
    public static Projectile instance;
    public GameObject[] ready;
    static int weapons;
    public GameObject laser;
    public GameObject shield;
    public GameObject[] SlasherEffecr;
    int count = 0;
    bool isSlash = false;
    float angle2;
    bool isReverse = false;
    float durationTime = 0.3f;
    public float angle;
    public float delayTime = 0.05f;
    float delay;
    public GameObject[] Arm;
    bool isDashing = false;
    float tmp;
    LinkedList<GameObject> list;
    public Vector2 pos;
    static float radius = 4.5f;
    public GameObject area;
    int big;
    public GameObject adio;
    public GameObject slasadio;
    // Start is called before the first frame update
    void Start()
    {
        Energy = maxEnergy;
        instance = this;
        list = new LinkedList<GameObject>();
        
        if(SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 27)
        {
            weapons = 0;
            maxEnergy = 30;
            Energy = maxEnergy;
            BigbulletPrefab = BigBullets[0];
            big = 0;
            BigbulletSpeed = 10;
            bigreloadTime = 2f;
            Bigcost = 3;
        }else if(SceneManager.GetActiveScene().buildIndex == PlayerPrefs.GetInt("level"))
        {
            weapons = PlayerPrefs.GetInt("weapons");
            maxEnergy = PlayerPrefs.GetFloat("maxEnergy");
            big = PlayerPrefs.GetInt("big");
            BigbulletPrefab = BigBullets[big];
            BigbulletSpeed = PlayerPrefs.GetFloat("BigbulletSpeed");
            bigreloadTime = PlayerPrefs.GetFloat("bigreloadTime");
            Bigcost = PlayerPrefs.GetFloat("Bigcost");
        }
        else
        {
            weapons = PlayerPrefs.GetInt("weapons");
            maxEnergy = PlayerPrefs.GetFloat("maxEnergy");
            big = PlayerPrefs.GetInt("big");
            BigbulletPrefab = BigBullets[big];
            BigbulletSpeed = PlayerPrefs.GetFloat("BigbulletSpeed");
            bigreloadTime = PlayerPrefs.GetFloat("bigreloadTime");
            Bigcost = PlayerPrefs.GetFloat("Bigcost");
           
        }
        
        tmp = PlayerController.instance.getrun();
    }

    // Update is called once per frame
    void Update()
    {
        tmp = PlayerController.instance.getrun();
        Vector3 mousePosition = pos;
        Vector3 direction = (mousePosition - transform.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        if (Bigload <= 0 && Energy > 0)
        {
            ready[weapons].SetActive(true);
        }
      
        if (weapons == 0 || weapons == 4 || weapons == 5)
        {
            for(int i = 0; i < Arm.Length; i++)
            {
                Arm[i].SetActive(false);
            }
            Arm[weapons].SetActive(true);
            if (Input.GetKey(KeyCode.J))
            {
                
                if (reload <= 0 && Energy > 0 && bulletPrefab != null)
                {
                    StartCoroutine(ShowEffect(0.5f));
                    float dir = angle + Random.Range(-7f, 7f);
                    GameObject bullet = Instantiate(bulletPrefab[weapons == 0 ? 0 : weapons - 3], this.transform.position + new Vector3(0.75f * Mathf.Cos(dir * Mathf.Deg2Rad), 0.75f * Mathf.Sin(dir * Mathf.Deg2Rad), 0), Quaternion.Euler(0, 0, dir));
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    rb.velocity = new Vector3(Mathf.Cos(dir * Mathf.Deg2Rad), Mathf.Sin(dir * Mathf.Deg2Rad), 0) * (bulletSpeed[weapons == 0 ? 0 : weapons - 3]);

                    Energy -= cost;
                    reload = (weapons == 0 ? 1 : 1.5f) * reloadTime;
                }
            }
            transform.localRotation = Quaternion.Euler(0, 0, player.transform.rotation.eulerAngles.y == 0 ? angle : 180 - angle);
        }
        else if (weapons == 1)
        {
            for(int i = 0; i < Arm.Length; i++)
            {
                Arm[i].SetActive(false);
            }
            Arm[weapons].SetActive(true);
            
            
            laser.transform.rotation = Quaternion.Euler(0, 0, angle);
            if (Input.GetKey(KeyCode.J) && isSlash == false && reload <= 0)
            {
                Instantiate(slasadio,this.transform.position, Quaternion.Euler(0, 0, 0));
                isSlash = true;
                if (angle <= 90 && angle >= -90)
                {
                    player.transform.rotation = Quaternion.Euler(0, 0, 0);

                }
                else
                {
                    player.transform.rotation = Quaternion.Euler(0, 180, 0);

                }
                SlasherEffecr[weapons - 1].SetActive(true);
                reload = reloadTime/1.5f;
               
            }
            if (isSlash == true)
            {
                if (isReverse == false)
                {
                    angle2 -= 2000 * Time.deltaTime;
                    Arm[weapons].transform.localRotation = Quaternion.Euler(0, 0, angle2);
                    if (angle2 <= -180)
                    {
                        isReverse = true;
                        SlasherEffecr[weapons - 1].SetActive(false);
                        
                    }

                }
                if (isReverse == true)
                {
                    angle2 += 700 * Time.deltaTime;
                    Arm[weapons].transform.localRotation = Quaternion.Euler(0, 0, angle2);
                    if (angle2 >= -40)
                    {

                        angle2 = -40;
                        isReverse = false;
                        isSlash = false;

                    }
                }
            }
            

        }
        else if (weapons == 2)
        {
            for (int i = 0; i < Arm.Length; i++)
            {
                Arm[i].SetActive(false);
            }
            Arm[weapons].SetActive(true);


            laser.transform.rotation = Quaternion.Euler(0, 0, angle);
           
           if (Input.GetKey(KeyCode.J) && isSlash == false && reload <= 0)
            {
                isSlash = true;
                if (angle <= 90 && angle >= -90)
                {
                    player.transform.rotation = Quaternion.Euler(0, 0, 0);

                }
                else
                {
                    player.transform.rotation = Quaternion.Euler(0, 180, 0);

                }
                SlasherEffecr[weapons - 1].SetActive(true);
                reload = reloadTime * 1.2f;
                Energy -= cost;
            }
            if (isSlash == true)
            {
                if (isReverse == false)
                {
                    angle2 -= 1000 * Time.deltaTime;
                    Arm[weapons].transform.localRotation = Quaternion.Euler(0, 0, angle2);
                    if (angle2 <= -90)
                    {
                        Arm[weapons].transform.localRotation = Quaternion.Euler(0, 0, -90);
                    }
                    if (angle2 <= -180)
                    {
                        isReverse = true;
                        SlasherEffecr[weapons-1].SetActive(false);
                        angle2 = -90;
                    }

                }
                if (isReverse == true)
                {
                    
                    angle2 += 300 * Time.deltaTime;
                    Arm[weapons].transform.localRotation = Quaternion.Euler(0, 0, angle2);
                    if (angle2 >= -40)
                    {

                        angle2 = -40;
                        isReverse = false;
                        isSlash = false;

                    }
                }
            }


        }
        else if (weapons == 3)
        {
            for (int i = 0; i < Arm.Length; i++)
            {
                Arm[i].SetActive(false);
            }
            Arm[weapons].SetActive(true);

            
                laser.transform.rotation = Quaternion.Euler(0, 0, angle);
            this.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);
            if (Input.GetKey(KeyCode.J) && isSlash == false && reload <= 0)
            {


                Instantiate(slasadio, this.transform.position, Quaternion.Euler(0, 0, 0)); 
                isSlash = true;
                if (angle <= 90 && angle >= -90)
                {
                    player.transform.rotation = Quaternion.Euler(0, 0, 0);

                }
                else
                {
                    player.transform.rotation = Quaternion.Euler(0, 180, 0);

                }
                if(count == 0)
                {
                    SlasherEffecr[weapons - 1].SetActive(true);
                }
                else
                {
                    SlasherEffecr[weapons].SetActive(true);
                }
                reload = reloadTime;
                Energy -= cost;
            }
            if (isSlash == true)
            {
                if(count == 0)
                {
                    if (isReverse == false)
                    {

                        Arm[weapons].transform.localPosition += new Vector3(7 * Time.deltaTime, 0, 0);

                        if (Arm[weapons].transform.localPosition.x >= 0.7f)
                        {
                            isReverse = true;
                            SlasherEffecr[weapons - 1].SetActive(false);

                        }

                    }
                    if (isReverse == true)
                    {

                        Arm[weapons].transform.localPosition -= new Vector3(10 * Time.deltaTime, 0, 0);

                        if (Arm[weapons].transform.localPosition.x <= 0)
                        {

                            Arm[weapons].transform.localPosition = new Vector3(0, 0, 0);
                            isReverse = false;
                            isSlash = false;
                            count = 1;

                        }
                    }
                    
                }
                else
                {
                    angle2 += 1500 * Time.deltaTime;
                    Arm[weapons].transform.localRotation = Quaternion.Euler(0, 0, angle2);
                    if (angle2 >= 360)
                    {

                        angle2 = 0;
                        SlasherEffecr[weapons].SetActive(false);
                        isSlash = false;
                        count = 0;

                    }
                }
                
            }


        }
        
        if (Input.GetKey(KeyCode.K))
        {
            
           
                if (Energy > cost)
                {
                    laser.SetActive(true);
                if (laser.GetComponent<Collider2D>() != null)
                {
                    Energy -= Time.deltaTime;
                }
                }
                else
                {
                    laser.SetActive(false) ;
                }
            
        }
        else if (Input.GetKeyUp(KeyCode.K))
        {
            
            laser.SetActive(false);
            if (Bigload <= 0 && Energy >= Bigcost)
            {
                Instantiate(adio, transform.position, Quaternion.identity);

                if (BigbulletPrefab.name == "Buff_0 Dash")
                {
                    isDashing = true;
                    
                }
                else
                {
                    if((weapons == 1 || weapons == 2 || weapons == 3) && isSlash == false)
                {
                        isSlash = true;
                        if (count == 0)
                        {
                            SlasherEffecr[weapons - 1].SetActive(true);
                        }
                        else
                        {
                            SlasherEffecr[weapons].SetActive(true);
                        }
                    }
                else
                    {
                        StartCoroutine(ShowBigEffect());
                    }
                    Vector3 spawnPosition = playerTransform.position + new Vector3(0.041f, 0.3f, 0);
                    GameObject Big = Instantiate(BigbulletPrefab, spawnPosition, Quaternion.Euler(0, 0, angle));
                    Rigidbody2D rb = Big.GetComponent<Rigidbody2D>();
                    rb.velocity = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad) + (BigbulletPrefab.name == "BIGBullet_0 Grenade" ? 0.45f : 0), 0) * BigbulletSpeed;
                    Energy -= Bigcost;
                    
                    Bigload = bigreloadTime;
                    ready[weapons].SetActive(false);
                }
                

            }
        }
        
        else
        {
            Energy += 1.5f*Time.deltaTime;
            if(Energy >= maxEnergy)
            {
                Energy = maxEnergy;
            }
            else if(Energy <= 0)
            {
                Energy = 0;
            }
        }
        if(isDashing == true)
        {
            delay -= Time.deltaTime;
            durationTime -= Time.deltaTime;
           

            if (delay <= 0)
            {
                StartCoroutine(ShowBigEffect());
                delay = delayTime;

            }
            if (durationTime <= 0)
            {
                if ((weapons == 1 || weapons == 2 || weapons == 3) && isSlash == false)
                {
                    isSlash = true;
                    if (count == 0)
                    {
                        SlasherEffecr[weapons - 1].SetActive(true);
                    }
                    else if (count == 1)
                    {
                        SlasherEffecr[weapons - 3].SetActive(true);
                    }
                    else
                    {
                        SlasherEffecr[weapons].SetActive(true);
                    }
                }
                shield.SetActive(false);
                player.GetComponent<Rigidbody2D>().gravityScale = 1.5f;
                isDashing = false;
                player.name = "Main";
                Energy -= Bigcost;
                durationTime = 0.3f;
                PlayerController.instance.run(tmp);
                PlayerController.instance.jump = 325;
                Bigload = bigreloadTime;
                ready[weapons].SetActive(false);
            }
            
            else
            {
                tmp = PlayerController.instance.getrun();
                PlayerController.instance.run(0);
                PlayerController.instance.jump = 0;
                player.transform.position += 10 * Time.deltaTime* new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
                shield.SetActive(true);

                player.name = "ggg";
                player.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
        PlayerPrefs.SetInt("weapons", weapons);
        PlayerPrefs.SetFloat("maxEnergy", maxEnergy);
        PlayerPrefs.SetInt("big", big);
        PlayerPrefs.SetFloat("BigbulletSpeed", BigbulletSpeed);
        PlayerPrefs.SetFloat("bigreloadTime", bigreloadTime);
        PlayerPrefs.SetFloat("Bigcost", Bigcost);
        reload -= Time.deltaTime;
        Bigload -= Time.deltaTime;
        float min = Mathf.Infinity;
        GameObject r = null;
        List<GameObject> itemsToRemove = new List<GameObject>();

        foreach (var item in list)
        {
            if (item == null || Vector2.Distance(item.transform.position, player.transform.position) > radius)
            {
                itemsToRemove.Add(item);
            }
        }

        foreach (var item in itemsToRemove)
        {
            list.Remove(item);
        }

        if (r == null)
        {
            foreach (var item in list)
            {
                
                if (Vector2.Distance(item.transform.position, player.transform.position) < min && item != null)
                {
                    min = Vector2.Distance(item.transform.position, player.transform.position);
                    r = item;
                }
            }
        }
        if(list.Count > 0)
        {
            Bursor.instance.off(false);
            foreach (var item in list)
            {
                if (Vector2.Distance(item.transform.position, player.transform.position) < min && item != null)
                {
                    min = Vector2.Distance(item.transform.position, player.transform.position);
                    r = item;
                }
            }
            pos = r.transform.position;
            
        }
        else
        {
            Bursor.instance.off(true);
            pos = area.transform.position;
        }
        
    }
    IEnumerator ShowEffect(float dis)
    {
        Vector3 mousePosition = pos;
        Vector3 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Tạo một instance của prefab hiệu ứng tại vị trí và quay của đối tượng này
        GameObject effect = Instantiate(shot, this.transform.position + new Vector3(dis * Mathf.Cos(angle * Mathf.Deg2Rad), dis * Mathf.Sin(angle * Mathf.Deg2Rad), 0), Quaternion.Euler(0,0,angle));

        
        // Chờ trong effectDuration giây
        yield return new WaitForSeconds(0.07f);

        // Hủy hiệu ứng
        Destroy(effect);

    }
    IEnumerator ShowBigEffect()
    {
        Vector3 mousePosition = pos;
        Vector3 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Tạo một instance của prefab hiệu ứng tại vị trí và quay của đối tượng này
        GameObject effect = Instantiate(Bigshot, transform.position + new Vector3(0.03f, 0, 0), Quaternion.Euler(0, 0, angle));

        Animator.SetBool("isOnAir", true);
        // Chờ trong effectDuration giây
        yield return new WaitForSeconds(Bigtime);
        Animator.SetBool("isOnAir", false);
        // Hủy hiệu ứng
        Destroy(effect);

    }
    public float fill()
    {
        float i = Energy / maxEnergy;
        if(i >= 1)
        {
            i = 1;
        }
        if(i < 0)
        {
            i = 0;
        }
        return i;
    }
    public float cooldown()
    {
        float i = reload / reloadTime;
        if(i >= 1)
        {
            i = 1;
        }
        if (i < 0)
        {
            i = 0;
        }
        return i;
    }
    public float Bigcooldown() {
        float i = Bigload / bigreloadTime;
        if (i >= 1)
        {
            i = 1;
        }
        if (i < 0)
        {
            i = 0;
        }
        return i;

    }
    public string str()
    {
        return BigbulletPrefab.name;
    }
    public void change(int i)
    {
        BigbulletPrefab = BigBullets[i];
        big = i;
        
        if(i == 3)
        {
            BigbulletSpeed = 3;
            bigreloadTime = 4f;
            Bigcost = 3;
        }
        else if(i == 2)
        {
            BigbulletSpeed = 0;
            bigreloadTime = 0.875f;
            Bigcost = 2.5f;
        }
        
        else if(i == 8)
        {
            BigbulletSpeed = 0;
            bigreloadTime = 3f;
            Bigcost = 3;
        }
        else if(i == 9)
        {
            BigbulletSpeed = 0;
            bigreloadTime = 3f;
            Bigcost = 3;
        }
        else
        {
            BigbulletSpeed = 12;
            bigreloadTime = 3f;
            Bigcost = 3;
        }
    }
    public Sprite get()
    {
        return BigbulletPrefab.GetComponent<SpriteRenderer>().sprite;
    }
   public void reduce()
    {
        reloadTime -= 0.1f;
        bigreloadTime *= 0.9f;
    }
    public void moreEnergy()
    {
        maxEnergy = 38;
    }
    public void numero(int i)
    {
        weapons = i;
        
    }
    public int got()
    {
        return weapons;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if((collision.GetComponent<EnemyMove>() != null || collision.GetComponent<Boss2>() != null || collision.GetComponent<Boss>() != null) && collision.name != "Saw(Clone)" && collision.name.Substring(0,Mathf.Min(6, collision.name.Length)) != "bullet")
        {
            list.AddFirst(collision.gameObject);
        }
    }
    
}
