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

            defaultPage.CreateBool("Immortal", Color.green, ImmortalEntry.Value, (a) => { ImmortalEntry.Value = a; });            
            defaultPage.CreateFunction("Save Settings", Color.cyan, () => { MelonPreferences.Save(); });                   
        }

        private void SetupMelonPreferences() {
            category = MelonPreferences.CreateCategory("VitalShift");
            ImmortalEntry = category.CreateEntry("Immortal", false);
            MelonPreferences.Save();
            category.SaveToFile();
        }

        //public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
        //    base.OnSceneWasLoaded(buildIndex, sceneName);
        //
        //}

        public override void OnUpdate() {
            base.OnUpdate();
            Immortal();
            RespawnHeal();
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
        }
    }
}