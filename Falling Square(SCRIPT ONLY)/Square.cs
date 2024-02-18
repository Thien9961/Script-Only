using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Square : MonoBehaviour,IDestructible,IListener
{
    public Color faction { get { return GetComponent<SpriteRenderer>().color; } set { GetComponent<SpriteRenderer>().color = value; } }
    public float speed, targetOffset=2f;
    public Rigidbody2D rb;
    public Animator animator;
    // Start is called before the first frame update

    public void GameStart()
    {
        if (Circle.instance != null && rb.bodyType != RigidbodyType2D.Static)
            rb.velocity = ((Circle.instance.transform.position + new Vector3(Mathf.Clamp(-Random.Range(-1f, 1f) * targetOffset,-2,2), 0, 0) - transform.position).normalized * speed);
    }

    public void GameEnd()
    {
        animator.speed = 0;
        rb.velocity = Vector2.zero;
        if(gameObject.activeSelf)
            Extension.WaitForSeconds(this, GameManager.instance.delay*0.5f, Death);
    }
    public void OnEnable()
    {
        Birth();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out IDestructible destructible) && destructible.faction == Faction.CIRCLE)
        {
            SoundManager.instance.Play(SoundManager.instance.hostileTouch);
            rb.velocity = Vector2.zero;
            animator.speed = 0;
        }
        else
            Death(); 
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.TryGetComponent(out IDestructible destructible) && destructible.faction==Faction.CIRCLE)
        {
            SoundManager.instance.Play(SoundManager.instance.friendlyTouch);
            GameManager.instance.score++;
            GameManager.instance.playing.UpdateScore(true);
        }
        Death();  
    }

    public void Birth()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GetComponent<Collider2D>().enabled = true;
        GameManager.instance.listener.Add(this);
        animator.speed = 1;
        if (Random.value <= 0.5f)
            animator.Play("Rotate Right");
    }

    public void Death()
    {
        animator.speed = 1;
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.zero;
        animator.Play("Square Death");
    }

    public void Recycle()
    {
        transform.localScale = Vector3.one * 0.5f;
        transform.position = Spawner.instance.squarePool.Container.transform.position;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Collider2D>().isTrigger = false;
        Spawner.instance.squarePool.Take(this);
    }
}
