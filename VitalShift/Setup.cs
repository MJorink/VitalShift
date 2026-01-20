using MelonLoader;
using BoneLib;
using BoneLib.BoneMenu;
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
            Page defaultPage = Page.Root.CreatePage("Jorink", Color.magenta).CreatePage("VitalShift", Color.red);

            defaultPage.CreateBool("Immortal", Color.yellow, ImmortalEntry.Value, (a) => { ImmortalEntry.Value = a; });
            defaultPage.CreateFloat("Ragdoll Duration", Color.yellow, RagdollDurationEntry.Value, 1f, 1f, 10f, (a) => { RagdollDurationEntry.Value = a;});
            defaultPage.CreateFunction("Save Settings", Color.cyan, () => { MelonPreferences.Save(); });                   
        }

        private void SetupMelonPreferences() {
            category = MelonPreferences.CreateCategory("VitalShift");
            ImmortalEntry = category.CreateEntry("Immortal", false);
            RagdollDurationEntry = category.CreateEntry("Ragdoll Duration", 5f);
            MelonPreferences.Save();
            category.SaveToFile();
        }

        public override void OnUpdate() {
            base.OnUpdate();
            Immortal();
            RespawnHeal();
            Unragdoll();
        }
    }
}