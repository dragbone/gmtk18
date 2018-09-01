using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public GameObject Target;
    private float _speed = 32f;
    private float Damage;

    public void Construct(GameObject target, float damage)
    {
        Target = target;
        Damage = damage;
    }

    void Update()
    {
        if (Target == null)
        {
            Destroy(gameObject, 2f);
            enabled = false;
            return;
        }

        transform.position =
            Vector3.MoveTowards(transform.position, Target.transform.position, _speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, Target.transform.position) < 0.01f)
        {
            Target.GetComponent<Enemy>().Hit(Damage);
            Destroy(gameObject, 2f);
            enabled = false;
            return;
        }

        transform.rotation = Quaternion.LookRotation(Target.transform.position - transform.position);
    }
}