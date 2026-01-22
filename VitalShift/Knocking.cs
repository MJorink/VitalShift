using MelonLoader;
using BoneLib;
using UnityEngine;

namespace VitalShift {
    public partial class Core : MelonMod {

        private void Knocked() {
            if (!KnockedEntry.Value) return;
            if (Player.RigManager == null) return;
            
            if (IsKnocked) return;
            if (IsDead) return;

            if (Player.RigManager.health.curr_Health <= 0f) {
                Player.RigManager.health.Respawn();
                IsKnocked = true;
                
            }}    

        private void KnockedHandling() {
            if (IsKnocked) {
                RagdollKnocked();
            }
            if (IsDead) {
                RagdollDead();
            }
        }

        private void RagdollDead() {
            // Only once per death
            if (RagdollingDead) return;
            RagdollingDead = true;
            RagdollDeadStart = Time.time;

            Player.PhysicsRig.ShutdownRig();
            Player.PhysicsRig.RagdollRig(); 
        }

        private void RagdollKnocked() {
            // Repeated check for death during knocked
            if (Player.RigManager.health.curr_Health <= 0f) {
                Player.RigManager.health.Dying(5);
                IsDead = true;
            }

            // Only once per knocked
            if (RagdollingKnocked) return;
            RagdollingKnocked = true;
            RagdollKnockedStart = Time.time;

            Player.RigManager.health.curr_Health = MediumHealthThreshold / 2f;
            Player.PhysicsRig.RagdollRig(); 
            Player.PhysicsRig.DisableBallLoco();
            Player.PhysicsRig.PhysicalLegs();
            Player.PhysicsRig.legLf.ShutdownLimb();
            Player.PhysicsRig.legRt.ShutdownLimb();
        }        

        private void Unragdoll() {
            if (RagdollingDead) {
            if (Time.time - RagdollDeadStart > 5f) {

            var feet = Player.PhysicsRig.feet.transform;
            var knee = Player.PhysicsRig.knee.transform;
            var pelvis = Player.PhysicsRig.m_pelvis.transform;

            Player.PhysicsRig.TurnOnRig();
            Player.PhysicsRig.UnRagdollRig();

            var position = pelvis.position;
            var rotation = pelvis.rotation;

            knee.SetPositionAndRotation(position, rotation);
            feet.SetPositionAndRotation(position, rotation);
            
            RagdollingDead = false;
            RagdollingKnocked = false;
            IsDead = false;
            IsKnocked = false;
            Player.RigManager.health.Respawn();
            }}

            if (RagdollingKnocked) {
            if (Time.time - RagdollKnockedStart > KnockedDurationEntry.Value) {
            if (IsDead) return;

            var feet = Player.PhysicsRig.feet.transform;
            var knee = Player.PhysicsRig.knee.transform;
            var pelvis = Player.PhysicsRig.m_pelvis.transform;

            Player.PhysicsRig.TurnOnRig();
            Player.PhysicsRig.UnRagdollRig();

            var position = pelvis.position;
            var rotation = pelvis.rotation;

            knee.SetPositionAndRotation(position, rotation);
            feet.SetPositionAndRotation(position, rotation);
            
            RagdollingKnocked = false;
            IsKnocked = false;
            Player.RigManager.health.Respawn();
            }}
        }
    }
}