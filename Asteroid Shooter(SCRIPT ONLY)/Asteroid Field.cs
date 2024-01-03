using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Utility;

public class AsteroidField : Enviroment
{
    public int asteroidLimit, minAsteroid,maxAsteroid;
    public float asteroidMaxTorque, asteroidSize;
    public Vector3 asteroidMaxVelocity;
    public static Transform asteroidContainer;
    public int[] objectPool;
    public AsteroidField[] encounter;

    // Start is called before the first frame update
    protected override void Start()
    {
        
        base.Start();
        Draw.Rec(spawnArea, Color.red);
        if(asteroidContainer== null)
            asteroidContainer = GetComponent<Transform>();
        foreach(AsteroidField field in encounter)
            Instantiate(field,transform);  
    }

    public static List<Asteroid> GetActiveAsteroid()
    {
        List<Asteroid> l = new List<Asteroid>();
        foreach (Transform t in asteroidContainer)
            if (t.gameObject.activeSelf && t.TryGetComponent(out Asteroid asteroid))
                l.Add(asteroid);
        return l;
    }

    private void OnDisable()
    {
        StopCoroutine(NaturalSpawn());
    }
    public override IEnumerator NaturalSpawn()
    {
        yield return new WaitForSeconds(Mathf.Clamp(GameManager.env_interval_coefficent,0.5f,Mathf.Infinity) * Random.Range(minInterval, maxInterval));
        var max = Random.Range(minAsteroid, maxAsteroid);
        var count = 0;
        for (int i = 0; i < objectPool.Length; i++)
            foreach (Asteroid ast in GetActiveAsteroid())
                if (ast.poolIndex == objectPool[i])
                    count++;
        if (count < asteroidLimit)
        {
            for (int i = 0; i < max; i++)
            {
                int rng = Random.Range(0, objectPool.Length);
                var asteroidClone = pool.GetFromPool<Transform>(objectPool[rng]);
                if (asteroidClone != null)
                {
                    Asteroid a = asteroidClone.GetComponent<Asteroid>();
                    a.poolIndex = objectPool[rng];
                    a.maxSize = asteroidSize;
                    layerOrder--;
                    a.GetComponent<SpriteRenderer>().sortingOrder = layerOrder;
                    Vector2 v1 = GameManager.RandomLocInRect(spawnArea);
                    Vector2 v2 = Camera.main.ScreenToWorldPoint(new Vector3(v1.x, v1.y, 0));
                    Vector3 v3 = new Vector3(Random.Range(-asteroidMaxVelocity.x, asteroidMaxVelocity.x), Random.Range(-asteroidMaxVelocity.y, asteroidMaxVelocity.y), Random.Range(-asteroidMaxVelocity.z, asteroidMaxVelocity.z));
                    a.IntialState(v2, v3, Random.Range(-asteroidMaxTorque, asteroidMaxTorque), Vector3.zero,a.maxHp,Color.white);
                }

            }
        }
        StartCoroutine(NaturalSpawn());
    }
}
