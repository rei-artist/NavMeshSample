//
// Mecanimのアニメーションデータが、原点で移動しない場合の Rigidbody付きコントローラ
// サンプル
// 2014/03/13 N.Kobyasahi
//
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime;
using UnityEngine.InputSystem;

namespace UnityChan
{
// 必要なコンポーネントの列記
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Rigidbody))]

	public class UnityChanControlScriptWithRgidBody : MonoBehaviour
	{
		[SerializeField] private GameObject cameraTarget;
        [SerializeField] private List<AgentAttack> agentAttacks;

        [SerializeField] private float animSpeed = 1.5f;                // アニメーション再生速度設定

        // 以下キャラクターコントローラ用パラメタ
        // 前進速度
        [SerializeField] private float forwardSpeed = 7.0f;
        // 後退速度
        [SerializeField] private float backwardSpeed = 2.0f;
        // 旋回速度
        [SerializeField] private float rotateSpeed = 2.0f;
		private Rigidbody rb;
		private Animator anim;


		private bool attacking = false;
		[field: SerializeField] public int hp { get; private set; }
        [field: SerializeField] public int maxHp { get; private set; }

        public void Damage(int damage)
		{
			hp = Math.Max(hp-damage,0); 
			anim.SetTrigger(hp > 0 ? "GetHit" : "Dead");
        }

        void Start ()
		{
			anim = GetComponent<Animator> ();
			rb = GetComponent<Rigidbody> ();

            foreach (var attack in agentAttacks)
			{
				attack.Finish += OnEndAgentAttack;
			}
		}

		private void Attack(int index)
        {
            anim.SetFloat("Speed", 0);
            anim.SetFloat("SideSpeed", 0);
            attacking = true;
            agentAttacks[index].BeginAttack();
        }

        void OnEndAgentAttack()
		{
			attacking = false;
		}

		void Update()
        {
            if (attacking) return;

            if (Input.GetMouseButtonDown(0))
            {
                Attack(0);
                return;
            }
            if (Input.GetMouseButtonDown(1))
            {
                Attack(1);
				return;
            }

            float h = Input.GetAxis("Mouse X");
			float hKey = Input.GetAxis("Horizontal");
			float vCamera = Input.GetAxis("Mouse Y");
			cameraTarget.transform.localPosition += Vector3.up * vCamera * Time.deltaTime * 10.0f;


			float v = Input.GetAxis("Vertical");
			anim.SetFloat("Speed", v);
			anim.SetFloat("SideSpeed", hKey);
			anim.SetFloat("Direction", h);
			anim.speed = animSpeed;



            Vector3 velocity = new Vector3(0, 0, v); 
			velocity = transform.TransformDirection(velocity);
			if (v > 0.1) {
				velocity *= forwardSpeed; 
			}
			else if (v < -0.1) {
				velocity *= backwardSpeed;
			}
			Vector3 hVelocity = transform.TransformDirection(Vector3.right * hKey * 5f);

            transform.localPosition += (velocity + hVelocity) * Time.deltaTime;
            transform.Rotate(0, h * rotateSpeed * Time.deltaTime, 0);

		}
	}
}