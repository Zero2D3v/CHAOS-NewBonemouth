using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrowSpear : MonoBehaviour
{
        public GameObject player;
        public GameObject spearPrefab;
        public Transform spearSpawnPoint;
        public float spearSpeed = 10f;
        public float spearCooldownTime = 1f;
        public float meleeRange = 2f;
        public float meleeCooldownTime = 1f;
        public float detectionRange = 10f;

        [SerializeField]
        private int spearsRemaining = 3;

        private bool isPlayerInRange;
        private bool isPlayerInMeleeRange;
        private bool canThrowSpear;
        private bool canMelee;
        private float timeSinceLastSpearThrow = 0f;
        private float timeSinceLastMelee = 0f;
        private Rigidbody2D rb;

        void Start()
        {
            player = GameObject.Find("Player");
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            DoChecks();

            if (canThrowSpear)
            {
                ThrowSpear();
                ResetThrowCooldown();
            }

            if (canMelee)
            {
                MeleeAttack();
                ResetMeleeCooldown();
            }

            timeSinceLastSpearThrow += Time.deltaTime;
            timeSinceLastMelee += Time.deltaTime;
        }

        void DoChecks()
        {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if(distance <= detectionRange)
        {
            isPlayerInRange = true;
        }
        else if(distance <= meleeRange)
        {
            isPlayerInMeleeRange = true;
        }

        if (spearsRemaining > 0 && timeSinceLastSpearThrow >= spearCooldownTime && isPlayerInRange)
        {
            canThrowSpear = true;
        }
        else
        {
            canThrowSpear = false;
        }
        if (isPlayerInMeleeRange && timeSinceLastMelee >= meleeCooldownTime)
        {
            canMelee = true;
        }
        else 
        { 
            canMelee = false;
        }
        }

        void ThrowSpear()
        {
            Vector3 direction = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
           // spearSpawnPoint.rotation = rotation;

            Vector2 spearVelocity = direction.normalized * spearSpeed;

            GameObject spear = Instantiate(spearPrefab, spearSpawnPoint.position, Quaternion.identity);
            spear.transform.right = spearVelocity.normalized;

            rb = spear.GetComponent<Rigidbody2D>();
            rb.velocity = spearVelocity;

            spearsRemaining--;
        }

        void MeleeAttack()
        {
            // Perform melee attack
        }

        void ResetThrowCooldown()
        {
            timeSinceLastSpearThrow = 0f;
        }

        void ResetMeleeCooldown()
        {
            timeSinceLastMelee = 0f;
        }
    }
