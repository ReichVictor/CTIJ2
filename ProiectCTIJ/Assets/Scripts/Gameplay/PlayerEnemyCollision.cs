using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{
    public class PlayerEnemyCollision : Simulation.Event<PlayerEnemyCollision>
    {
        public EnemyController enemy;
        public PlayerController player;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var willHurtEnemy = player.Bounds.center.y >= enemy.Bounds.max.y;

            // Get the player's Health component
            var health = player.GetComponent<Health>();

            if (health == null)
            {
                Debug.LogError("Player does not have a Health component!");
                return;
            }

            if (willHurtEnemy)
            {
                var enemyHealth = enemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.Decrement();
                    if (!enemyHealth.IsAlive)
                    {
                        Schedule<EnemyDeath>().enemy = enemy;
                        player.Bounce(2);
                    }
                    else
                    {
                        player.Bounce(7);
                    }
                }
                else
                {
                    Schedule<EnemyDeath>().enemy = enemy;
                    player.Bounce(2);
                }
            }
            else
            {
                Debug.Log($"Player health before damage: {health.currentHP}");
                health.Decrement(); // Reduce player's health by 20
                Debug.Log($"Player health after damage: {health.currentHP}");

                if (health.currentHP <= 0)
                {
                    Debug.Log("Player died.");
                    Schedule<PlayerDeath>();
                }
            }
        }
    }
}
