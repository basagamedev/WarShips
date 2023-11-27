using UnityEngine;

[CreateAssetMenu(fileName = "ShipStats", menuName = "ScriptableObjects/ShipStats", order = 1)]
public class ShipStatsScriptable : ScriptableObject
{
    #region SERIALIZE_FIELDS
    [Header("Ship Stats")]
    [Range(1f, 10f)]
    [SerializeField] private int shipLife = 0;
    [SerializeField] private float shipMaxSpeed = 0;
    [SerializeField] private float shipAccelaration = 0;
    [SerializeField] private float shipDecelaration = 0;
    [SerializeField] private float shipRotationSpeed = 0;
    [SerializeField] private float shipCannonRotationSpeed = 0;
    [SerializeField] private float shipCannonBallSpeed = 0;
    [SerializeField] private float shipDistanceToAttack = 0;
    [SerializeField] private float shipDistanceStopAttack = 0;
    [SerializeField] private float shipDelayToAttack = 0;
    [SerializeField] private float shipOscillationToAttack = 0;
    #endregion

    #region GET_ENCAPSULATED_OBJECTS
    public ShipMovementStats GetShipMovementStats()
    {
        return new ShipMovementStats(shipMaxSpeed, shipAccelaration, shipDecelaration, shipRotationSpeed);
    }

    public ShipDamageableStats GetShipDamageableStats()
    {
        return new ShipDamageableStats(shipLife);
    }

    public ShipCannonStats GetShipCannonStats()
    {
        return new ShipCannonStats(shipCannonRotationSpeed);
    }

    public ShipCannonBallStats GetShipCannonBallStats()
    {
        return new ShipCannonBallStats(shipCannonBallSpeed);
    }
    public ShipAttackStats GetShipAttackStats()
    {
        return new ShipAttackStats(shipDistanceToAttack, shipDistanceStopAttack, shipDelayToAttack, shipOscillationToAttack);
    }
    #endregion
}

#region ENCAPSULATED_CLASSES
public class ShipMovementStats
{
    public float MaxSpeed { get; private set; }
    public float Acceleration { get; private set; }
    public float Deceleration { get; private set; }
    public float RotationSpeed { get; private set; }

    public ShipMovementStats(float maxSpeed, float acceleration, float deceleration, float rotationSpeed)
    {
        MaxSpeed = maxSpeed;
        Acceleration = acceleration;
        Deceleration = deceleration;
        RotationSpeed = rotationSpeed;
    }
}

public class ShipDamageableStats
{
    public int ShipLife { get; private set; }

    public ShipDamageableStats(int shipLife)
    {
        ShipLife = shipLife;
    }
}

public class ShipCannonStats
{
    public float ShipCannonRotationSpeed { get; private set; }

    public ShipCannonStats(float shipCannonRotationSpeed)
    {
        ShipCannonRotationSpeed = shipCannonRotationSpeed;
    }
}

public class ShipCannonBallStats
{
    public float ShipCannonBallSpeed { get; private set; }

    public ShipCannonBallStats(float shipCannonBallSpeed)
    {
        ShipCannonBallSpeed = shipCannonBallSpeed;
    }
}

public class ShipAttackStats
{
    public float ShipDistanceToAttack { get; private set; }
    public float ShipDistanceStopAttack { get; private set; }
    public float ShipDelayToAttack { get; private set; }
    public float ShipOscillationToAttack { get; private set; }

    public ShipAttackStats(float shipDistanceToAttack, float shipDistanceStopAttack, float shipDelayToAttack, float shipOscillationToAttack)
    {
        ShipDistanceToAttack = shipDistanceToAttack;
        ShipDistanceStopAttack = shipDistanceStopAttack;
        ShipDelayToAttack = shipDelayToAttack;
        ShipOscillationToAttack = shipOscillationToAttack;
    }
}
#endregion
