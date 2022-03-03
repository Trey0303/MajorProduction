using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public GameObject target;
    public float speed;
    public float range;

    private Vector3 detectionRange;

    // Start is called before the first frame update
    void Start()
    {
        detectionRange = this.transform.position + detectionRange;//?
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
