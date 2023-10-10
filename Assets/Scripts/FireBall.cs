using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class FireBall : MonoBehaviour
    {
        public int damage = 10;
        public GameObject fireballPrefab;
        public Transform fireballSpawnPoint;
        public float fireballSpeed;
        private bool canShoot = true;
        public float detectionRadius = 10f;
        private Transform target;
        
        
        void Update()
        {
            StartCoroutine(ShootDelay());

            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    target = collider.transform;
                    break; 
                }
            }
        }

        void ShootFireball()
        {
            if (canShoot)
            {
                GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, fireballSpawnPoint.rotation);
                Rigidbody rb = fireball.GetComponent<Rigidbody>();
                rb.velocity = (target.position - fireballSpawnPoint.position).normalized * fireballSpeed;
            }
        }
        
        void LineShootFireBall()
        {
            if (canShoot)
            {
                GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, fireballSpawnPoint.rotation);
                Rigidbody rb = fireball.GetComponent<Rigidbody>();
                rb.velocity = transform.forward * fireballSpeed;
            }
        }

        private IEnumerator ShootDelay()
        {
            if (canShoot && target != null)
            {
                ShootFireball();
                canShoot = false;
                yield return new WaitForSeconds(1f);
                canShoot = true;
            }
            else if (canShoot && target == null)
            {
                LineShootFireBall();
                canShoot = false;
                yield return new WaitForSeconds(1f);
                canShoot = true;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }
