using MelonLoader;
using BoneLib;
using BoneLib.BoneMenu;
using BoneLib.Notifications;
using UnityEngine;
using Il2CppSLZ.Marrow;
using Il2CppSLZ.Marrow.Warehouse;

[assembly: MelonInfo(typeof(VitalShift.Core), "VitalShift", "3.0.3", "jorink")]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace VitalShift
{
    public class Core : MelonMod
    {
        private enum HealthTier
        {
            High,
            Medium,
            Low
        }

		private const string DefaultAvatarBarcode = "SLZ.BONELAB.Content.Avatar.FordBW";
        private const float highMultiplier = 0.7f;
        private const float mediumMultiplier = 0.3f;
        private static float highThreshold;
        private static float mediumThreshold;
        private static float currentHealth;

        private static RigManager rig;
        private static Health health;

        private static MelonPreferences_Category category;
        private static MelonPreferences_Entry<bool> EnableModEntry;
        private static MelonPreferences_Entry<string> SavedAvatarHigh;
        private static MelonPreferences_Entry<string> SavedAvatarMedium;
        private static MelonPreferences_Entry<string> SavedAvatarLow;

        private static Barcode AvatarHigh;
        private static Barcode AvatarMedium;
        private static Barcode AvatarLow;
        private static Barcode currentAvatar;

        public override void OnInitializeMelon()
        {
            SetupMelonPreferences();
            SetupBoneMenu();
            SetupHooks();
            SetupAvatars();
        }

        private static void SetupMelonPreferences()
        {
            category = MelonPreferences.CreateCategory("VitalShift");

            EnableModEntry = category.CreateEntry("Enable Mod", true);
            SavedAvatarHigh = category.CreateEntry("Avatar High", DefaultAvatarBarcode);
            SavedAvatarMedium = category.CreateEntry("Avatar Medium", DefaultAvatarBarcode);
            SavedAvatarLow = category.CreateEntry("Avatar Low", DefaultAvatarBarcode);

            MelonPreferences.Save();
            category.SaveToFile();
        }
        
        private static void SetupBoneMenu()
        {
            Page defaultPage = Page.Root.CreatePage("Jorink", Color.red).CreatePage("VitalShift", Color.red);

            defaultPage.CreateBool("Enable Mod", Color.blue, EnableModEntry.Value, (a) => { EnableModEntry.Value = a; });
            defaultPage.CreateFunction("Set High HP Avatar", Color.green, () => { SetAvatar(HealthTier.High); });
            defaultPage.CreateFunction("Set Medium HP Avatar", Color.yellow, () => { SetAvatar(HealthTier.Medium); });
            defaultPage.CreateFunction("Set Low HP Avatar", Color.red, () => { SetAvatar(HealthTier.Low); });

            defaultPage.CreateFunction("Save Settings", Color.cyan, () => { MelonPreferences.Save(); });
        }

        private static void SetupHooks()
        {
            Hooking.OnLevelLoaded += OnLevelLoaded;
        }

        private static void SetupAvatars()
        {
        	AvatarHigh = new Barcode(SavedAvatarHigh.Value);
        	AvatarMedium = new Barcode(SavedAvatarMedium.Value);
        	AvatarLow = new Barcode(SavedAvatarLow.Value);
        }

        private static void OnLevelLoaded(LevelInfo levelInfo)
        {
        	rig = Player.RigManager;
        	health = rig.health;
        }

        public override void OnUpdate()
        {
            if (!isModAllowed()) return;

            highThreshold = health.max_Health * highMultiplier;
            mediumThreshold = health.max_Health * mediumMultiplier;
            currentHealth = health.curr_Health;

            HealthTier targetTier = currentHealth > highThreshold ? HealthTier.High
                : currentHealth > mediumThreshold ? HealthTier.Medium
                : HealthTier.Low;

            SwapAvatar(targetTier);
        }

        private static bool isModAllowed()
        {
            if (!EnableModEntry.Value || !rig) return false;
            
            currentAvatar = rig.AvatarCrate.Barcode;
            return isManagedAvatar(currentAvatar);
        }

        private static bool isManagedAvatar(Barcode avatar)
        {
            return avatar == AvatarHigh || avatar == AvatarMedium || avatar == AvatarLow;
        }

        private static void SwapAvatar(HealthTier tier)
        {
            Barcode targetAvatar = tier switch
            {
                HealthTier.High => AvatarHigh,
                HealthTier.Medium => AvatarMedium,
                HealthTier.Low => AvatarLow,
                _ => AvatarHigh
            };

            if (targetAvatar == currentAvatar) return;
            rig.SwapAvatarCrate(targetAvatar);
        }

        private static void SetAvatar(HealthTier tier)
        {
            MelonPreferences_Entry<string> savedEntry;
            string tierLabel;

            switch (tier)
            {
                case HealthTier.High:
                    AvatarHigh = currentAvatar;
                    savedEntry = SavedAvatarHigh;
                    tierLabel = "High";
                    break;
                case HealthTier.Medium:
                    AvatarMedium = currentAvatar;
                    savedEntry = SavedAvatarMedium;
                    tierLabel = "Medium";
                    break;
                default:
                    AvatarLow = currentAvatar;
                    savedEntry = SavedAvatarLow;
                    tierLabel = "Low";
                    break;
            }

            savedEntry.Value = currentAvatar.ToString();

            var notif = new Notification
            {
                Title = $"{tierLabel} tier set to:",
                Message = currentAvatar.ID,
                Type = NotificationType.Success,
                PopupLength = 0.75f,
                ShowTitleOnPopup = true
            };
            Notifier.Send(notif);
        }
    }
}
