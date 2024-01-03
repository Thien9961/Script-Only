using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

public class DustCloud : Enviroment
{
    public static Transform cloudContainer;
    public int[] objectPool;
    public float  cloudSize;
    public static Cloud cloud;

    protected override void Start()
    {
        base.Start();
        Draw.Rec(spawnArea, Color.blue);
        cloudContainer = GetComponent<Transform>();
    }

    public override IEnumerator NaturalSpawn()
    {
        yield return new WaitForSeconds(GameManager.env_interval_coefficent * Random.Range(minInterval,maxInterval));
            var rng = Random.Range(0, objectPool.Length);
            var cloudClone = pool.GetFromPool<Transform>(objectPool[rng]);
            cloud = cloudClone.GetComponent<Cloud>();
            cloud.poolIndex = objectPool[rng];     
            Vector2 v1 = GameManager.RandomLocInRect(spawnArea);
            Vector2 v2 = Camera.main.ScreenToWorldPoint(new Vector3(v1.x, v1.y, 0));
            cloud.maxSize = cloudSize;
            cloud.dustCloud=GetComponent<DustCloud>();
            cloud.GetComponent<SpriteRenderer>().sortingOrder = layerOrder;
            layerOrder--;
            cloud.IntialState(v2,Vector3.zero,0,Vector3.zero,cloud.maxHp,Color.white);
    }
}
