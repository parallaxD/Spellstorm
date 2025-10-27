using UnityEngine;

public class SpellStash : MonoBehaviour
{
    Spell _spell;

    public void ChangeCurrentSpell(Spell newSpell)
    {
        _spell = newSpell;
    }

    public void CastSpell()
    {
        _spell?.Action();
    }

    public void ClearStash()
    {
        _spell = null;
    }
}
