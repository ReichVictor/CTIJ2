using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    /// <typeparam name="PlayerDeath"></typeparam>
    public class PlayerDeath : Simulation.Event<PlayerDeath>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;

            // Check if health is at or below zero.
            if (!player.health.IsAlive || player.health.currentHP <= 0)
            {
                Debug.Log("Player is dying...");

                // Set HP to zero to ensure it's consistent.
                player.health.currentHP = 0;

                // Disable camera following and player control.
                model.virtualCamera.m_Follow = null;
                model.virtualCamera.m_LookAt = null;
                player.controlEnabled = false;

                // Play audio feedback.
                if (player.audioSource && player.ouchAudio)
                    player.audioSource.PlayOneShot(player.ouchAudio);

                // Trigger death animation.
                player.animator.SetTrigger("hurt");
                player.animator.SetBool("dead", true);

                // Schedule player respawn or level restart.
                Simulation.Schedule<PlayerSpawn>(2);
            }
            else
            {
                Debug.LogWarning("PlayerDeath event triggered while player is alive!");
            }
        }

    }
}