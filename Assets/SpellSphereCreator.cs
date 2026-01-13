using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellSphereCreator : MonoBehaviour
{
    public GameObject sectorPrefab;
    public Transform sphereRoot;

    public Sprite fireSprite;
    public Sprite waterSprite;
    public Sprite earthSprite;
    public Sprite windSprite;

    public GameObject CreateSpellSphere(List<ElementTuple> recipe)
    {
        foreach (Transform child in sphereRoot)
            Destroy(child.gameObject);

        List<ElementTuple> visualRecipe = BuildVisualRecipe(recipe);

        int totalParts = 0;
        foreach (var tuple in visualRecipe)
            totalParts += tuple.count;

        float angleStep = 360f / totalParts;
        float currentAngle = 0f;

        GameObject spellProjectileGO = new GameObject("spellProjectile");
        spellProjectileGO.tag = "Projectile";
        spellProjectileGO.AddComponent<Rigidbody2D>().gravityScale = 0;
        spellProjectileGO.AddComponent<CircleCollider2D>().isTrigger = true;
        //spellProjectileGO.GetComponent<CircleCollider2D>().excludeLayers = LayerMask.GetMask("Player");
        spellProjectileGO.GetComponent<Transform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
        spellProjectileGO.AddComponent<SpellProjectile>();
        foreach (var tuple in visualRecipe)
        {
            for (int i = 0; i < tuple.count; i++)
            {
                GameObject sector = Instantiate(sectorPrefab, spellProjectileGO.transform);
                SpriteRenderer sr = sector.GetComponent<SpriteRenderer>();
                sr.sprite = GetSpriteForElement(tuple.elementType);
                sector.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
                sector.transform.position = new Vector3(0, 0, 0);
                currentAngle += angleStep;
            }
        }
        return spellProjectileGO;
    }

    private List<ElementTuple> BuildVisualRecipe(List<ElementTuple> original)
    {
        List<ElementTuple> visual = new List<ElementTuple>();
        foreach (var t in original)
            visual.Add(new ElementTuple(t.elementType, t.count));

        int total = visual.Sum(t => t.count);
        if (total >= 4)
            return visual;

        int missing = 4 - total;

        ElementTuple dominant = visual
            .OrderByDescending(t => t.count)
            .First();

        dominant.count += missing;
        return visual;
    }

    Sprite GetSpriteForElement(ElementType element)
    {
        switch (element)
        {
            case ElementType.Fire: return fireSprite;
            case ElementType.Water: return waterSprite;
            case ElementType.Earth: return earthSprite;
            case ElementType.Wind: return windSprite;
            default:
                Debug.LogError("Нет спрайта для элемента: " + element);
                return null;
        }
    }
}
