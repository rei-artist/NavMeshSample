using System;
using System.Collections;
using System.Collections.Generic;
using UnityChan;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AgentAttackJump : AgentAttack
{
    [SerializeField] private float jumpSpeed = 8f;
    [SerializeField] private float jumpHighRate = 0.5f;
    [SerializeField] private float endTime = 0.5f;

    private Rigidbody rb;
    private Animator anim;
    private bool attacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    public override void BeginAttack()
    {
        StartCoroutine(Jump());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!attacking) return;
        TryDamage(collision.gameObject, 1);
    }

    private IEnumerator Jump()
    {
        attacking = true;
        Vector3 dir = transform.forward;

        anim.SetFloat("Speed", 0);
        rb.velocity = (dir + Vector3.up * jumpHighRate) * jumpSpeed;
        anim.SetBool("Jump", true);

        yield return new WaitForSeconds(1.5f);

        anim.SetBool("Jump", false);
        anim.SetFloat("Speed", 0);
        attacking = false;

        yield return new WaitForSeconds(endTime);
        Finish();
    }
}
