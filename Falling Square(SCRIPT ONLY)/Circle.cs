using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]


public class Circle : Singleton<Circle>,IDestructible
{
    public Color faction { get { return GetComponent<SpriteRenderer>().color; } set { GetComponent<SpriteRenderer>().color = value; } }
    public Rigidbody2D rb;
    

    private void OnEnable()
    {
        Birth();
    }

    private void FixedUpdate()
    {       
        if (rb.velocity == Vector2.zero)
            Rail.instance.SwtichDirection(true);
    }

    public void Birth()
    {
        float xMax = Rail.instance.GetComponent<SliderJoint2D>().limits.max,xMin = Rail.instance.GetComponent<SliderJoint2D>().limits.min;
        rb = GetComponent<Rigidbody2D>();
        faction = Faction.CIRCLE;
        Vector3 v; 
        if (Random.value <= 0.5f)
            v = new Vector3(xMin, 0, 0);
        else
            v = new Vector3(xMax, 0, 0);
        transform.position= v;
        Rail.instance.GetComponent<SliderJoint2D>().connectedBody = rb; 
        Rail.instance.GetComponent<SliderJoint2D>().useMotor = true;
    }

    public void Death()
    {
        Instantiate(Resources.Load<ParticleSystem>("Circle Death"),transform.position,transform.rotation);
        transform.position = Vector3.zero;
        Destroy(gameObject);
        GameManager.instance.gameContext.state = GameManager.instance.over;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Death();  
    }
}

