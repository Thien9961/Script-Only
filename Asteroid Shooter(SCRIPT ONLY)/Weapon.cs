using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public float  cooldown = 1, damage = 1, spread=0;
    public static float sensivity = 20;
    public int maxAmmo;

    public int ammo;
    private float Spread
    {
        get { return spread; }
        set
        {
            spread = Mathf.Abs(value);
            
        }
    }
    public int multishot = 1, particlePool = 12;
    public Joystick joystick;
    public GameObject trigger;
    public Ship owner;
    public bool ready;


    // Start is called before the first frame update
    void Start()
    {
        ready=true;
        cooldown = Mathf.Clamp(cooldown, 0.1f, Mathf.Infinity);
        ammo = maxAmmo;
        if (trigger.GetComponent<Btn>().inputType != InputType.PRESS)
            trigger.GetComponent<Btn>().action = Fire;
        else
            trigger.GetComponent<Button>().onClick.AddListener(Fire);
        GetComponent<Image>().raycastTarget = false;
    }

    public int GetAmmo()
    {
        return ammo;
    }

    public List<Destrucible> HitScan()
    {
        List<Destrucible> d = new List<Destrucible>();
        for(int i=0;i<multishot;i++)
        {
            Vector2 hit = Camera.main.ScreenToWorldPoint(new Vector2(transform.position.x + Random.Range(-Spread, Spread), transform.position.y + Random.Range(-Spread, Spread)));
            foreach(Asteroid a in AsteroidField.GetActiveAsteroid())
                if(a.GetComponent<Collider2D>().bounds.Contains(hit))
                {
                    ParticleSystem p = Enviroment.pool.GetFromPool<Transform>(particlePool).GetComponent<ParticleSystem>();
                    p.transform.position = hit;
                    p.transform.localScale = a.transform.localScale;
                    p.GetComponent<Renderer>().sortingOrder = a.GetComponent<SpriteRenderer>().sortingOrder + 1;
                    p.Play();
                    if (owner!=null)
                        a.TakeDamage(owner.gameObject,damage);
                    if (!d.Contains(a))
                        d.Add(a);
                }         
        }
        return d;
    }

     private IEnumerator StopBlinking(float sec, Color c)
    {
        Image i = GetComponent<Image>();
        Color original = i.color;
        i.color = c;
        yield return new WaitForSeconds(sec);
        i.color = original;
        StopCoroutine(StopBlinking(sec, c));
    }
    public void BlinkCrosshair(Color c, float rate)
    {
        StartCoroutine(StopBlinking(rate, c));
    }

    IEnumerator coolingdown(float sec)
    {
        yield return new WaitForSeconds(sec);
        ready=true;
    }
    public void Fire()
    {
        if(ready && ammo>0)
        {
            ready = false;
            HitScan();
            ammo--;
            owner.HUD.SetText("Ammo Text", ammo.ToString());
            transform.DOPunchScale(transform.localScale*1.33f,0.1f,10,0);
            BlinkCrosshair(Color.red, 0.1f);
            StartCoroutine(coolingdown(cooldown));
        }
        
    }
    public void Aim()
    {
        Slider s = (Slider)UIManager.main.hashtable["Joystick Sensitivity"];
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        float x = Mathf.Clamp( transform.position.x+joystick.Horizontal * s.value,canvas.pixelRect.xMin,canvas.pixelRect.xMax);
        float y = Mathf.Clamp(transform.position.y+joystick.Vertical * s.value,canvas.pixelRect.yMin, canvas.pixelRect.yMax);
        Vector2 v = new Vector2(x, y);
        transform.position = v;
    }
    
    // Update is called once per frame
    void Update()
    {
        Aim();
    }
}
