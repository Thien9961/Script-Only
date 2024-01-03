using UnityEngine;

public class Destrucible : MonoBehaviour
{
    public float maxHp;
    [HideInInspector] public float hp;
    public readonly int vfxPool=13;//Death vfx pool index

    public virtual void TakeDamage(GameObject source, float amount) 
    {
        hp-=amount;
        if (!(hp > 0))
            Death(source);
    }

    protected virtual void Start()
    {
        hp = maxHp;
    }

    public virtual void Death(GameObject killer)
    {
        if (killer!=null)
        {
            ParticleSystem p = Enviroment.pool.GetFromPool<Transform>(vfxPool).GetComponent<ParticleSystem>();
            p.transform.localScale = transform.localScale;
            p.transform.position = transform.position;
            if(TryGetComponent(out SpriteRenderer renderer))
                p.GetComponent<Renderer>().sortingOrder = renderer.sortingOrder;
            p.Play();
        } 
    }

}
