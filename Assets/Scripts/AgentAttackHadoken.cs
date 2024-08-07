using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityChan;
using UnityEngine;

public class AgentAttackHadoken : AgentAttack
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private Terrain terrain;

    private Rigidbody rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        terrain = GameObject.FindGameObjectWithTag("Ground").GetComponent<Terrain>();
    }

    public override void BeginAttack()
    {
        StartCoroutine(Attack());
    }

    private bool OnCollisionBall(Collider collider) 
    {
        return TryDamage(collider.gameObject, 1);
    }

    private IEnumerator Attack()
    {
        Vector3 pos = transform.position + transform.forward * 20f;
        float height = terrain.SampleHeight(pos);
        pos.y = height + 1f;

        anim.SetFloat("Speed", 0);
        anim.SetBool("Hadoken", true);

        if (audioSource != null && audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }

        yield return new WaitForSeconds(0.5f);
        var ball = Instantiate(ballPrefab);
        ball.transform.position = transform.position;
        ball.transform.Translate(Vector3.up + transform.forward);
        HadokenBall hadokenBall = ball.GetComponent<HadokenBall>();
        hadokenBall.target = pos;
        hadokenBall.OnCollision = OnCollisionBall;
        if(isPlayer)
        {
            hadokenBall.OnDestroy = (go) =>
            {
                DoTarget(go.transform.position, 10f);
            };
        }

        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Hadoken", false);
        if(!isPlayer) yield return new WaitForSeconds(0.8f);

        Finish();
    }
}
