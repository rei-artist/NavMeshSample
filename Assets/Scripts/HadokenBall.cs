using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadokenBall : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] public Vector3 target; // { get; set; }
    [SerializeField] private GameObject explosion;

    public Func<Collider,bool> OnCollision;
    public Action<GameObject> OnDestroy = null;

    void OnTriggerEnter(Collider collider)
    {

        if(OnCollision(collider))
        {
            var go = Instantiate(explosion);
            go.transform.position = transform.position;
            if (OnDestroy != null) OnDestroy(gameObject);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, target) < 0.5f)
        {
            if (OnDestroy != null) OnDestroy(gameObject);
            Destroy(gameObject);
            return;
        }
        Vector3 pos = transform.position;
        pos += (target - pos).normalized * speed * Time.deltaTime;
        transform.position = pos;
    }

}
