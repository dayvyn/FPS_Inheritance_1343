using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    int enemyLayerNum = 7;
    public bool withinRange { get; private set; }
    public bool withinAttackRange { get; private set; }
    float timer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > .5f)
        {
            withinRange = WithinRange();
            withinAttackRange = WithinAttackRange();
            timer = 0;
        }
    }


    //Aggro Range
    bool WithinRange()
    {
        Collider[] player = Physics.OverlapSphere(transform.position, 7, 1 << enemyLayerNum);
        
        if (player.Length > 0)
        {
            return true;
        }

        return false;
    }

    //Attack Range
    bool WithinAttackRange()
    {
        Collider[] player = Physics.OverlapSphere(transform.position, 2f, 1 << enemyLayerNum);
        if (player.Length > 0)
        {
            return true;
        }

        return false;
    }

}
