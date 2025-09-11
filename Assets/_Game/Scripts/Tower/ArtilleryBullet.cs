using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArtilleryBullet : Bullet
{
    [Header("AOE Settings")]
    [SerializeField] private float aoeRadius = 3f;
    [SerializeField] private LayerMask enemyLayerMask = -1;

    [Header("Visual Effects")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float gizmoDisplayTime = 1f;

    // Internal variables
    float offsetX, offsetY, offsetZ;
    public float timeToEnemy = 1f;
    public float height = 1f;
    public float heightTargertOffset = -0.5f;

    private bool hasExploded = false;
    private Vector3 explosionPosition;
    private float explosionTime = -1f;

    void Start()
    {

    }

    Vector3 newTargetPos;

    public override void SetTarget(Transform target, Vector3 BeginPos)
    {
        base.SetTarget(target, BeginPos);
        offsetX = target.position.x - BeginPos.x;
        offsetY = target.position.y - BeginPos.y;
        offsetZ = target.position.z - BeginPos.z;
        newTargetPos = target.position;
        float timer = 0;
        DOTween.To(() => offsetY, x => offsetY = x, offsetY + height, timeToEnemy / 2)
            .OnUpdate(() =>
            {
                UpdatePos();
            })
            .SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                DOTween.To(() => offsetY, x => offsetY = x, -heightTargertOffset, timeToEnemy / 2)
                    .OnUpdate(() =>
                    {
                        UpdatePos();
                    })
                    .SetEase(Ease.InSine)
                    .OnComplete(() =>
                    {
                        // Trigger AOE explosion when bullet reaches target
                        ExplodeAOE();
                    });
            });
        void UpdatePos()
        {
            UpdateTarget();
            timer += Time.deltaTime;
            float currentX = offsetX * (1f - (timer / timeToEnemy));
            float currentZ = offsetZ * (1f - (timer / timeToEnemy));
            transform.position = new Vector3(
                newTargetPos.x - currentX,
                newTargetPos.y - offsetY,
                newTargetPos.z - currentZ);
        }
    }

    public void UpdateTarget()
    {
        if (target.gameObject.activeInHierarchy) newTargetPos = target.position;
        else
        {
            newTargetPos = target.position + Vector3.down * 0.75f;
        }
        if (!target.gameObject.activeInHierarchy && coroutine == null && IsToTarget())
        {
            coroutine = StartCoroutine(RemoveMissingBullet());
        }
    }

    private void ExplodeAOE()
    {
        if (hasExploded) return;

        hasExploded = true;
        explosionPosition = transform.position;
        explosionTime = Time.time;

        // Create explosion effect if assigned
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, explosionPosition, Quaternion.identity);
        }

        // Find all enemies within AOE radius
        Collider[] hitColliders = Physics.OverlapSphere(explosionPosition, aoeRadius, enemyLayerMask);

        foreach (Collider hitCollider in hitColliders)
        {
            // Try to find enemy component or damage interface
            BaseEnemy enemy = hitCollider.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                // Apply damage to enemy
                enemy.TakeDamage(damage);
            }
            else
            {
                // Fallback: try to find any component that has TakeDamage method
                var damageable = hitCollider.GetComponent<MonoBehaviour>();
                if (damageable != null)
                {
                    // Use reflection or send message to apply damage
                    damageable.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        // Destroy bullet after explosion
        Destroy(gameObject,0.05f);
    }

    public override void Update()
    {
        // Original bullet doesn't override Update, so we keep it empty
        // The movement is handled by DOTween in SetTarget
    }

    // Gizmo visualization for AOE radius
    void OnDrawGizmos()
    {
        // Draw AOE radius in Scene view
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, aoeRadius);

        Gizmos.color = new Color(1f, 0.5f, 0f, 0.8f);
        Gizmos.DrawWireSphere(transform.position, aoeRadius);
    }

    void OnDrawGizmosSelected()
    {
        // Draw more detailed gizmo when selected
        Gizmos.color = new Color(1f, 0.2f, 0f, 0.5f);
        Gizmos.DrawSphere(transform.position, aoeRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aoeRadius);

        // Draw explosion effect gizmo if bullet has exploded recently
        if (hasExploded && Time.time - explosionTime < gizmoDisplayTime)
        {
            float alpha = 1f - ((Time.time - explosionTime) / gizmoDisplayTime);
            Gizmos.color = new Color(1f, 1f, 0f, alpha * 0.6f);
            Gizmos.DrawSphere(explosionPosition, aoeRadius);

            Gizmos.color = new Color(1f, 0f, 0f, alpha);
            Gizmos.DrawWireSphere(explosionPosition, aoeRadius);
        }
    }
}