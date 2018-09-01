using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public GameObject Target;
    private float _speed = 16f;

    public void Construct(GameObject target)
    {
        Target = target;
    }

    void Update()
    {
        if (Target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position =
            Vector3.MoveTowards(transform.position, Target.transform.position, _speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, Target.transform.position) < 0.01f)
        {
            Destroy(Target);
            Destroy(gameObject);
        }
    }
}