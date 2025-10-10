using UnityEngine;
public abstract class Spell
{
    public SpellData Data { get; private set; } = new SpellData();  
    public abstract void Action();
    public Spell(SpellData data)
    {   
        Data.Name = data.name;
        Data.ID = data.ID;
        Data.Cooldown = data.Cooldown;
        if (Data.Sprite != null) Data.Sprite = data.Sprite;
        Data.Receipt = data.Receipt;
    }

}

public class Fireball : Spell
{
    public Fireball(SpellData data) : base(data) { }

    public override void Action()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
      
        mouseScreenPosition.z = Mathf.Abs(Constants.MainCamera.transform.position.z);

        Vector3 mouseWorldPosition = Constants.MainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0;

        Debug.Log($"Mouse world position: {mouseWorldPosition}");

        Vector3 directionToShoot = (mouseWorldPosition - Constants.PlayerTransform.position).normalized;

        var fireball = GameObject.Instantiate(
            Constants.FireballPrefab,
            Constants.PlayerTransform.position,
            Quaternion.identity
        );

        fireball.GetComponent<Rigidbody2D>().AddForce(directionToShoot * 2, ForceMode2D.Impulse); 
    }
}

public class Waterball : Spell
{
    public Waterball(SpellData data) : base(data) { }
    public override void Action()
    {
        Debug.Log("Waterball casted!");
    }
}
