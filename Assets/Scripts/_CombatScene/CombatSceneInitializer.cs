using System.Collections.Generic;
using UnityEngine;

public class CombatSceneInitializer : MonoBehaviour
{
    public CombatPlayer playerObject;            // 씬에서 드래그로 연결
    public Enemy enemyObject;                    // 씬에서 드래그로 연결
    public List<Enemy> additionalEnemies;        // 필요 시 추가 적들

    private void Awake()
    {
        GameStateManager.Instance.SetPhase(GamePhase.Combat);
        List<Enemy> enemyList = new List<Enemy> { enemyObject };
        if (additionalEnemies != null)
            enemyList.AddRange(additionalEnemies);

        // CombatContext 직접 세팅
        CombatContext.Instance.combatPlayer = playerObject;
        CombatContext.Instance.allEnemies = enemyList;
        CombatContext.Instance.selectedEnemy = enemyObject;
        CombatContext.Instance.ResetRewards();

        Debug.Log("CombatContext 초기화 완료");
    }

    private void Start()
    {
        C_HUDManager.Instance.UpdatePlayerHealth(playerObject.health, playerObject.maxHealth);
        C_HUDManager.Instance.UpdateEnemyHealth(enemyObject.health, enemyObject.maxHealth);
        TurnManager.Instance.StartCombat();
    }
}