using UnityEngine;

public enum ZombieType { Normal, Armor, Boss }
[CreateAssetMenu(fileName = "Zombie Type", menuName = "Game/Zombie Type")]
public class TypeZombie : ScriptableObject
{
    public string zombieName;
    public ZombieType zombieType;
    public int health;
    public int valueScore;
    public GameObject prefab;
}
