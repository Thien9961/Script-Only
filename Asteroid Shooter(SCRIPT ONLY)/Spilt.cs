
using UnityEngine;

public class Spilt : PowerUp
{
    // Start is called before the first frame update
    public int qty; 
    public FlyingObject[] fragment;
    public FlyingObject originalObj;
    public Vector3[] intialDistant;
    public Vector2[] intialPos;

    public override void EffectStart()
    {
        for(int i = 0; i < qty; i++)
        {
            int rng=Random.Range(0,fragment.Length);
            FlyingObject f = Enviroment.pool.GetFromPool<Transform>(fragment[rng].poolIndex).GetComponent<FlyingObject>();
            f.maxSize = originalObj.maxSize;
            f.IntialState(intialPos[i], Random.value*new Vector3(0.5f,0.5f,originalObj.speed), 100, intialDistant[i], f.maxHp, Color.white);
        }
        base.EffectStart();
    }

    public void Init(Vector2 position, FlyingObject originalObject, Vector3[] fragments_initial_distance, Vector2[] fragments_initial_pos)
    {
        originalObj = originalObject;
        intialDistant = fragments_initial_distance;
        intialPos = fragments_initial_pos;     
    }
}
