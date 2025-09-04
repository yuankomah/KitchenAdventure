using UnityEngine;

public class SkeletonNecromancer : Enemy
{
    private float initialHealth = 10f;
    private void Start()
    {
        AssignWeapon();
        SetInitialHealth(initialHealth);
    }


    override public float GetInitialHealth()
    {
        return initialHealth;
    }
}
