using MelonLoader;
using BoneLib;
using UnityEngine;

namespace VitalShift {
    public partial class Core : MelonMod {
        private void Immortal() {
            if (!ImmortalEntry.Value) return;
            if (Player.RigManager == null) return;
            
            if (Player.RigManager.health.curr_Health <= 1f) {
                if (Player.RigManager.health.curr_Health <= 0f) {
                Player.RigManager.health.Respawn();
                NeedsHeal = true;
                RespawnTime = Time.time;
            }
                else {
                    Player.RigManager.health.curr_Health = 1.1f;
                }
            }
        }
    
        private void RespawnHeal() {
            if (!NeedsHeal) return;
            Ragdoll();
            if (Time.time - RespawnTime < 1f) return;
            Player.RigManager.health.curr_Health = 1.1f;
            NeedsHeal = false;
        }

        private void Ragdoll() {
            ragdolling = true;
            ragdollstart = Time.time;
            Player.PhysicsRig.ShutdownRig();
            Player.PhysicsRig.RagdollRig(); 
        }

        private void Unragdoll() {
            if (!ragdolling) return;
            if (Time.time - ragdollstart < RagdollDurationEntry.Value) {
            Player.PhysicsRig.TurnOnRig();
            Player.PhysicsRig.UnRagdollRig();
            ragdolling = false;
            }
        }
    }
}