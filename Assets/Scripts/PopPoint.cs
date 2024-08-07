using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopPoint : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int num;
    [SerializeField] private float time;
    [SerializeField] private List<Transform> wayPoints;
    
    private Terrain terrain;

    private bool popped = false;

    // Start is called before the first frame update
    void Start()
    {
        terrain = GameObject.FindGameObjectWithTag("Ground").GetComponent<Terrain>();

    }

    // Update is called once per frame
    void Update()
    {
        if(time < Time.time && !popped)
        {
            for (int i = 0; i < num; i++)
            {
                float tx = transform.position.x + Random.Range(-5f, 5f);
                float tz = transform.position.z + Random.Range(-5f, 5f);
                Vector3 pos = new Vector3(tx, 0, tz);
                var go = Instantiate(prefab);
                go.transform.position = new Vector3(tx, terrain.SampleHeight(pos), tz);
                var agentMovement = go.GetComponent<AgentMovement>();
                agentMovement.wayPoints = wayPoints;
            }
            popped = true;
        }
    }
}
