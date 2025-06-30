using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemies/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;

    [Header("Visual")]
    public Sprite enemySprite;

    [Header("Stats")]
    public int maxHealth;
    public bool isBoss;

    [Header("Action Patterns")]
    public List<EnemyActionPattern> normalPatterns;
    public EnemyActionPattern specialPattern; // optional
}