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

        private const float HighHealthThreshold = 0.7f;
        private const float MediumHealthThreshold = 0.3f;
        private const string DefaultAvatarBarcode = "SLZ.BONELAB.Content.Avatar.FordBW";

        private static MelonPreferences_Category category;
        private static MelonPreferences_Entry<bool> EnableModEntry;

        private static MelonPreferences_Entry<string> SavedAvatarHigh;
        private static MelonPreferences_Entry<string> SavedAvatarMedium;
        private static MelonPreferences_Entry<string> SavedAvatarLow;

        private static Barcode AvatarHigh;
        private static Barcode AvatarMedium;
        private static Barcode AvatarLow;
        private static Barcode currentAvatar;

        private static RigManager rig;

        public override void OnInitializeMelon()
        {
            SetupMelonPreferences();
            SetupBoneMenu();
            SetupHooks();
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
            Hooking.OnPlayerResurrected += OnPlayerResurrected;
        }

        private static void OnLevelLoaded(LevelInfo levelInfo)
        {
        	rig = Player.RigManager;
        	
            AvatarHigh = new Barcode(SavedAvatarHigh.Value);
            AvatarMedium = new Barcode(SavedAvatarMedium.Value);
            AvatarLow = new Barcode(SavedAvatarLow.Value);

            ResetToHighTier();
        }

		// Used for healing with SDK mods, like the Signalis Auto Injector
        private static void OnPlayerResurrected(Il2CppSLZ.Marrow.RigManager rigManager)
        {
            ResetToHighTier();
        }

        public override void OnUpdate()
        {
            if (!isModAllowed()) return;

            var health = rig.health;
            float highThreshold = health.max_Health * HighHealthThreshold;
            float mediumThreshold = health.max_Health * MediumHealthThreshold;
            float currentHealth = health.curr_Health;

            HealthTier targetTier = currentHealth > highThreshold ? HealthTier.High
                : currentHealth > mediumThreshold ? HealthTier.Medium
                : HealthTier.Low;

            SwapAvatar(targetTier);
        }

        private static void ResetToHighTier()
        {
            if (!isModAllowed()) return;

            SwapAvatar(HealthTier.High);
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

        private static bool isEquippedAvatar(Barcode avatar)
        {
            return avatar == currentAvatar;
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

            if (isEquippedAvatar(targetAvatar)) return;
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
                Title = $"{tierLabel} avatar set to:",
                Message = currentAvatar.ID,
                Type = NotificationType.Success,
                PopupLength = 0.75f,
                ShowTitleOnPopup = true
            };

            Notifier.Send(notif);
        }
    }
}
