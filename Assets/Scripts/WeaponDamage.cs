using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    private List<Collider> alreadyCollidedWith = new List<Collider>();
    [SerializeField] private Collider myCollider;
    private int attackDamage;
    private float knockback;

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) return;
        if (alreadyCollidedWith.Contains(other)) return;
        alreadyCollidedWith.Add(other);
        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealtDamage(attackDamage);
        }
        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            Vector3 force = (other.transform.position - myCollider.transform.position).normalized;
            forceReceiver.AddForce(force * knockback);
        }

    }

    public void SetAttack(int damage, float knockback)
    {
        attackDamage = damage;
        this.knockback = knockback;
    }
}
