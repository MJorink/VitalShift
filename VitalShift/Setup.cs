using MelonLoader;
using BoneLib;
using BoneLib.BoneMenu;
using UnityEngine;
using Il2CppSLZ.Marrow.Warehouse;

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

            defaultPage.CreateBool("Enable Mod", Color.blue, EnableModEntry.Value, (a) => { EnableModEntry.Value = a; });
            defaultPage.CreateBool("Knocked on Death", Color.cyan, KnockedEntry.Value, (a) => { KnockedEntry.Value = a; });
            defaultPage.CreateFloat("Knocked Duration", Color.yellow, KnockedDurationEntry.Value, 1f, 1f, 10f, (a) => { KnockedDurationEntry.Value = a;});
            defaultPage.CreateFloat("Death Duration", Color.red, DeadDurationEntry.Value, 1f, 1f, 10f, (a) => { DeadDurationEntry.Value = a;});
            defaultPage.CreateFunction("Set High HP Avatar", Color.green, () => { SetAvatarHigh(); });
            defaultPage.CreateFunction("Set Medium HP Avatar", Color.yellow, () => { SetAvatarMedium(); });
            defaultPage.CreateFunction("Set Low HP Avatar", Color.red, () => { SetAvatarLow(); });
            defaultPage.CreateFunction("Save Settings", Color.cyan, () => { MelonPreferences.Save(); });                   
        }

        private void SetupMelonPreferences() {
            category = MelonPreferences.CreateCategory("VitalShift");
            EnableModEntry = category.CreateEntry("Enable Mod", true);
            KnockedEntry = category.CreateEntry("Knocked on Death", false);
            KnockedDurationEntry = category.CreateEntry("Knocked Duration", 5f);
            DeadDurationEntry = category.CreateEntry("Death Duration", 5f);

            SavedAvatarHigh = category.CreateEntry("Avatar High", "SLZ.BONELAB.Content.Avatar.FordBW");
            SavedAvatarMedium = category.CreateEntry("Avatar Medium", "SLZ.BONELAB.Content.Avatar.FordBW");
            SavedAvatarLow = category.CreateEntry("Avatar Low", "SLZ.BONELAB.Content.Avatar.FordBW");

            MelonPreferences.Save();
            category.SaveToFile();
        }
        
        public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            AvatarHigh = new Barcode(SavedAvatarHigh.Value);
            AvatarMedium = new Barcode(SavedAvatarMedium.Value);
            AvatarLow = new Barcode(SavedAvatarLow.Value);
        }

        public override void OnUpdate() {
            base.OnUpdate();
            if (!EnableModEntry.Value) return;
            Knocked();
            KnockedHandling();
            Unragdoll();
            SetAvatar();
        }
    }
}