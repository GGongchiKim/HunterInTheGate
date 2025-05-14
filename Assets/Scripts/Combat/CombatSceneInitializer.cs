using System.Collections.Generic;
using UnityEngine;

public class CombatSceneInitializer : MonoBehaviour
{
    public Player playerObject;            // 씬에서 드래그로 연결
    public Enemy enemyObject;              // 씬에서 드래그로 연결
    public List<Enemy> additionalEnemies;  // 필요 시 추가 적들

    private void Start()
    {
      
        HUDManager.Instance.UpdatePlayerHealth(playerObject.health, playerObject.maxHealth);
        HUDManager.Instance.UpdateEnemyHealth(enemyObject.health, enemyObject.maxHealth);
        TurnManager.Instance.StartCombat();
    }

    private void Awake()
    {
        List<Enemy> enemyList = new List<Enemy> { enemyObject };
        if (additionalEnemies != null)
            enemyList.AddRange(additionalEnemies);

        GameContext.Instance.Initialize(playerObject, enemyObject, enemyList);
        Debug.Log("GameContext 초기화 완료");
    }

}
