using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectManager : MonoBehaviour
{
    private Dictionary<IDamagable, Coroutine> activeDOTs = new Dictionary<IDamagable, Coroutine>();

    public void ApplyDOT(int DOTValue, int totalTicks, float tickInterval, IDamagable objToDamage)
    {
        if (activeDOTs.ContainsKey(objToDamage))
        {
            StopCoroutine(activeDOTs[objToDamage]);
            activeDOTs[objToDamage] = StartCoroutine(DOT(DOTValue, totalTicks, tickInterval, objToDamage));
        }
        else
        {
            activeDOTs[objToDamage] = StartCoroutine(DOT(DOTValue, totalTicks, tickInterval, objToDamage));
            StartCoroutine(ShowDOTVisual(objToDamage.gameObject.transform, totalTicks * tickInterval));
        }
    }

    private IEnumerator ShowDOTVisual(Transform target, float duration)
    {
        Transform dotEffect = target.Find("DOTEffect");
        if (dotEffect == null) yield break;

        GameObject effect = dotEffect.gameObject;
        effect.SetActive(true);

        yield return new WaitForSeconds(duration);

        if (effect != null)
        {
            effect.SetActive(false);
        }
    }

    private IEnumerator DOT(int DOTValue, int totalTicks, float tickInterval, IDamagable objToDamage)
    {
        for (int i = 0; i < totalTicks; i++)
        {
            yield return new WaitForSeconds(tickInterval);
            if (objToDamage != null && objToDamage.IsAlive)
            {
                objToDamage.TakeDamage(DOTValue);
                Debug.Log($"{objToDamage} got {DOTValue} DOT damage ({i + 1}/{totalTicks})");
            }
            else
            {
                break;
            }
        }
        if (activeDOTs.ContainsKey(objToDamage))
        {
            activeDOTs.Remove(objToDamage);
        }
    }

    public void ApplySlowEffect(GameObject target, float slowFactor, float duration)
    {
        var objToSlow = target.GetComponent<IEffectable>();
        if (objToSlow != null)
        {
            objToSlow.ApplySlow(slowFactor, duration);
        }
    }

    public void ApplyAOESlow(Vector3 position, float radius, float slowFactor, float duration)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, radius);
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                ApplySlowEffect(collider.gameObject, slowFactor, duration);
            }
        }
    }

    public void ApplyKnockbackEffect(GameObject target, Vector2 direction, float force)
    {
        IDamagable damagable = target.GetComponent<IDamagable>();
        if (damagable == null || !damagable.IsAlive)
            return;

        var rigidbody = target.GetComponent<Rigidbody2D>();
        if (rigidbody != null)
        {
            Vector2 knockbackForce = direction.normalized * force;
            rigidbody.AddForce(knockbackForce, ForceMode2D.Impulse);

            float maxKnockbackSpeed = 10f;
            if (rigidbody.linearVelocity.magnitude > maxKnockbackSpeed)
            {
                rigidbody.linearVelocity = rigidbody.linearVelocity.normalized * maxKnockbackSpeed;
            }

            Debug.Log($"Applied knockback to {target.name} with force {force}");
        }
        else
        {
            Debug.LogWarning($"No Rigidbody2D found on {target.name} for knockback effect");
        }
    }

    public void ApplyAOEKnockback(Vector3 position, float radius, Vector2 direction, float force)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, radius);
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                ApplyKnockbackEffect(collider.gameObject, direction, force);
            }
        }
    }
}

