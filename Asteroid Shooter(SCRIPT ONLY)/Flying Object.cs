using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObject : Destrucible
{
    public float speed = 0,maxSize;
    public int poolIndex;
    protected virtual void Move()
    {
        if (Mathf.Abs(transform.localScale.x) < maxSize)
            transform.localScale += (Vector3.one * speed+Vector3.one*GameManager.player.speed);
        else
            Death(null);
    }

    public override void Death(GameObject killer)
    {
        base.Death(killer);
        Enviroment.pool.TakeToPool(poolIndex,transform);
    }

    public virtual void IntialState(Vector2 position, Vector3 velocity, float torque, Vector3 distance,float hp, Color color)
    {
        transform.position = position;
        transform.localScale = distance;
        if(TryGetComponent(out Rigidbody2D rb))
        {
            rb = GetComponent<Rigidbody2D>();
            rb.AddTorque(torque);
            rb.velocity = new Vector2(velocity.x, velocity.y);
        }  
        speed = velocity.z;
        this.hp=hp;
        GetComponent<SpriteRenderer>().color = color;
    }

    public virtual void FixedUpdate()
    {
        Move();
    }
}
