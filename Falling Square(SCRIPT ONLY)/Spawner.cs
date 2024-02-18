using UnityEngine;
using Redcode.Pools;

public class Spawner : Singleton<Spawner>,IListener
{
    public float spawnInterval,offsetMin=1,offsetMax=3,redSquareChance=0.1f;
    public Pool<Square> squarePool;
    // Start is called before the first frame update
    private void Start()
    {
        GameManager.instance.listener.Add(instance);
        squarePool = Pool.Create(Resources.Load<Square>("Square"), 0, transform);
    }
    public void GameStart()
    {
        
        InvokeRepeating(nameof(SpawnSquare), 0, spawnInterval);
    }

    public void GameEnd()
    {
        CancelInvoke();
        Extension.WaitForSeconds(this, GameManager.instance.delay+ GameManager.instance.loader.loadTime, GameManager.instance.over.ShowEndScreen);
    }

    public void SpawnSquare()
    {
        var sqr = squarePool.Get();
        System.Random random = new System.Random();
        int[] arr ={ -1, 1 };
        var rng = random.Next(0, arr.Length);
        var x = arr[rng] *( offsetMin + Random.value * offsetMax);
        Debug.Log(x);
        Vector3 v = transform.position + new Vector3(x, 0, 0);
        sqr.transform.position = v;
        sqr.GameStart();

        if (Random.value <= Mathf.Clamp01(redSquareChance))
        {
            sqr.faction = Faction.CIRCLE;
            sqr.GetComponent<Collider2D>().isTrigger=true;
        }
        else
            sqr.faction = Faction.SQUARE;
    }
}
