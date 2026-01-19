using MelonLoader;
using BoneLib;
using UnityEngine;

[assembly: MelonInfo(typeof(VitalShift.Core), "VitalShift", "1.0.0", "jorink")]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace VitalShift {
    public partial class Core : MelonMod {

        public override void OnInitializeMelon() {
            SetupMelonPreferences();
            SetupBoneMenu();
        }

        private void SetupBoneMenu() {
            BoneLib.BoneMenu.Page defaultPage = BoneLib.BoneMenu.Page.Root.CreatePage("VitalShift", Color.yellow);

            defaultPage.CreateBool("Immortal", Color.yellow, ImmortalEntry.Value, (a) => { ImmortalEntry.Value = a; });
            defaultPage.CreateFloat("Ragdoll Duration", Color.yellow, RagdollDurationEntry.Value, 1f, 1f, 10f, (a) => { RagdollDurationEntry.Value = a;});
            defaultPage.CreateFunction("Save Settings", Color.cyan, () => { MelonPreferences.Save(); });                   
        }

        private void SetupMelonPreferences() {
            category = MelonPreferences.CreateCategory("VitalShift");
            ImmortalEntry = category.CreateEntry("Immortal", false);
            RagdollDurationEntry = category.CreateEntry("Ragdoll Duration", 5f)
            MelonPreferences.Save();
            category.SaveToFile();
        }

        public override void OnUpdate() {
            base.OnUpdate();
            Immortal();
            RespawnHeal();
            Unragdoll();
        }
    
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
            if (Time.time - RespawnTime < 1f) return;
            Player.RigManager.health.curr_Health = 1.1f;
            NeedsHeal = false;
            Ragdoll();
        }

        private static void Ragdoll() {
            ragdolling = true;
            ragdollstart = Time.time;
            Player.PhysicsRig.ShutdownRig();
            Player.PhysicsRig.RagdollRig(); 
        }

        private static void Unragdoll() {
            if (!ragdolling) return;
            if (Time.time - ragdollstart < RagdollDurationEntry.Value) {
            Player.PhysicsRig.TurnOnRig();
            Player.PhysicsRig.UnRagdollRig();
            ragdolling = false;
            }
        }
    }
}