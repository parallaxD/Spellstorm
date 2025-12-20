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
}