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
                NeedsHeal = true;
                DeathTime = Time.time;
                Player.RigManager.health.Respawn();
            }
                else {
                    Player.RigManager.health.curr_Health = 1.1f;
                }
            }
        }
    
        private void RespawnHeal() {
            if (!NeedsHeal) return;
            Ragdoll();
            if (Time.time - DeathTime < 1f) return;
            Player.RigManager.health.curr_Health = 1.1f;
            NeedsHeal = false;
        }

        private void Ragdoll() {
            if (ragdolling) return;
            ragdolling = true;
            ragdollstart = Time.time;
            Player.PhysicsRig.ShutdownRig();
            Player.PhysicsRig.RagdollRig(); 
        }

        private void Unragdoll() {
            if (!ragdolling) return;
            if (Time.time - ragdollstart > RagdollDurationEntry.Value) {

            var feet = Player.PhysicsRig.feet.transform;
            var knee = Player.PhysicsRig.knee.transform;
            var pelvis = Player.PhysicsRig.m_pelvis.transform;

            Player.PhysicsRig.TurnOnRig();
            Player.PhysicsRig.UnRagdollRig();

            var position = pelvis.position;
            var rotation = pelvis.rotation;

            knee.SetPositionAndRotation(position, rotation);
            feet.SetPositionAndRotation(position, rotation);
            
            ragdolling = false;
            }
        }
    }
}