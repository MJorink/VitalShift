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

            defaultPage.CreateBool("Knocked", Color.yellow, KnockedEntry.Value, (a) => { KnockedEntry.Value = a; });
            defaultPage.CreateFloat("Knocked Duration", Color.yellow, KnockedDurationEntry.Value, 1f, 1f, 10f, (a) => { KnockedDurationEntry.Value = a;});
            defaultPage.CreateFloat("Death Duration", Color.yellow, DeadDurationEntry.Value, 1f, 1f, 10f, (a) => { DeadDurationEntry.Value = a;});
            defaultPage.CreateFunction("Save Settings", Color.cyan, () => { MelonPreferences.Save(); });                   
        }

        private void SetupMelonPreferences() {
            category = MelonPreferences.CreateCategory("VitalShift");
            KnockedEntry = category.CreateEntry("Knocked", false);
            KnockedDurationEntry = category.CreateEntry("Knocked Duration", 5f);
            DeadDurationEntry = category.CreateEntry("Death Duration", 5f);
            MelonPreferences.Save();
            category.SaveToFile();
        }

        public override void OnUpdate() {
            base.OnUpdate();
            Knocked();
            KnockedHandling();
            Unragdoll();
        }
    }
}