using MelonLoader;
using BoneLib;
using UnityEngine;
using Il2CppSLZ;

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

        public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
            base.OnSceneWasLoaded(buildIndex, sceneName);

        }

        public override void OnUpdate() {
            Immortal();
        }
    
        private void Immortal() {
            if (!ImmortalEntry.Value) return;
            if (Time.time < lastcheck + 1f) return;
            lastcheck = Time.time;

            MelonLogger.Msg("Health: " + Player.RigManager.health.curr_Health);

            if (Player.RigManager.health.curr_Health <= 10f) {
                Player.RigManager.health.curr_Health = 20f;
            }
        }  
    }
}