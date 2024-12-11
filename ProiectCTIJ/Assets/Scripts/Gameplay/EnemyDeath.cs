using Platformer.Core;
using Platformer.Mechanics;

namespace Platformer.Gameplay
{
    public class EnemyDeath : Simulation.Event<EnemyDeath>
    {
        public EnemyController enemy;

        public override void Execute()
        {
            enemy._collider.enabled = false;
            enemy.control.enabled = false;
            if (enemy._audio && enemy.ouch)
                enemy._audio.PlayOneShot(enemy.ouch);

            // Track enemy kill in the GameController
            GameController.Instance.KillEnemy();
        }
    }
}