using UnityEngine;

[CreateAssetMenu(fileName = "SOActorModel", menuName = "KillerWave/SOActorModel", order = 0)]
public class SOActorModel : ScriptableObject
{
    public enum AttackType
    {
        Wave, Player, Flee, Bullet
    }
    public AttackType attackType;
    public string actorName;
    public string description;
    public int score;
    public int health;
    public int speed;
    public int hitPower;
    public GameObject actor;
    public GameObject actorsBullets;
}