using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemies/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public Sprite sprite;
    public int maxHealth;
    public bool isBoss;
    public List<EnemyActionPattern> normalPatterns;
    public EnemyActionPattern specialPattern; // optional
}
