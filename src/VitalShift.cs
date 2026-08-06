using MelonLoader;
using BoneLib;
using BoneLib.BoneMenu;
using BoneLib.Notifications;
using UnityEngine;
using Il2CppSLZ.Marrow.Warehouse;

[assembly: MelonInfo(typeof(VitalShift.Core), "VitalShift", "3.0.0", "jorink")]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace VitalShift
{
    public enum HealthTier
    {
        High,
        Medium,
        Low
    }

    public class Core : MelonMod
    {
        private const float HighHealthThreshold = 0.7f;
        private const float MediumHealthThreshold = 0.3f;
        private const string DefaultAvatarBarcode = "SLZ.BONELAB.Content.Avatar.FordBW";

        private MelonPreferences_Category category;
        private MelonPreferences_Entry<bool> EnableModEntry;

        private MelonPreferences_Entry<string> SavedAvatarHigh;
        private MelonPreferences_Entry<string> SavedAvatarMedium;
        private MelonPreferences_Entry<string> SavedAvatarLow;

        private Barcode AvatarHigh;
        private Barcode AvatarMedium;
        private Barcode AvatarLow;
        private HealthTier CurrentTier;

        public override void OnInitializeMelon()
        {
			base.OnInitializeMelon();
            SetupMelonPreferences();
            SetupBoneMenu();

            Hooking.OnLevelLoaded += OnLevelLoaded;
            Hooking.OnPlayerDamageReceived += OnPlayerDamageReceived;
            Hooking.OnPlayerResurrected += OnPlayerResurrected;
        }

        public override void OnDeinitializeMelon()
        {
        	base.OnDeinitializeMelon();
            Hooking.OnLevelLoaded -= OnLevelLoaded;
            Hooking.OnPlayerDamageReceived -= OnPlayerDamageReceived;
            Hooking.OnPlayerResurrected -= OnPlayerResurrected;
        }

        private void SetupBoneMenu()
        {
            Page defaultPage = Page.Root.CreatePage("Jorink", Color.red).CreatePage("VitalShift", Color.red);

            defaultPage.CreateBool("Enable Mod", Color.blue, EnableModEntry.Value, (a) => { EnableModEntry.Value = a; });

            defaultPage.CreateFunction("Set High HP Avatar", Color.green, () => { SetAvatar(HealthTier.High); });
            defaultPage.CreateFunction("Set Medium HP Avatar", Color.yellow, () => { SetAvatar(HealthTier.Medium); });
            defaultPage.CreateFunction("Set Low HP Avatar", Color.red, () => { SetAvatar(HealthTier.Low); });

            defaultPage.CreateFunction("Save Settings", Color.cyan, () => { MelonPreferences.Save(); });
        }

        private void SetupMelonPreferences()
        {
            category = MelonPreferences.CreateCategory("VitalShift");

            EnableModEntry = category.CreateEntry("Enable Mod", true);

            SavedAvatarHigh = category.CreateEntry("Avatar High", DefaultAvatarBarcode);
            SavedAvatarMedium = category.CreateEntry("Avatar Medium", DefaultAvatarBarcode);
            SavedAvatarLow = category.CreateEntry("Avatar Low", DefaultAvatarBarcode);

            MelonPreferences.Save();
            category.SaveToFile();
        }

        private void OnLevelLoaded(LevelInfo levelInfo)
        {
            AvatarHigh = new Barcode(SavedAvatarHigh.Value);
            AvatarMedium = new Barcode(SavedAvatarMedium.Value);
            AvatarLow = new Barcode(SavedAvatarLow.Value);

            if (!EnableModEntry.Value) { return; }

            SwapAvatar(HealthTier.High, force: true);
        }

        private void OnPlayerDamageReceived(Il2CppSLZ.Marrow.RigManager rigManager, float damage)
        {
            if (!EnableModEntry.Value || Player.RigManager == null) { return; }

            var health = Player.RigManager.health;
            float highThreshold = health.max_Health * HighHealthThreshold;
            float mediumThreshold = health.max_Health * MediumHealthThreshold;
            float currentHealth = health.curr_Health;

            HealthTier targetTier = currentHealth > highThreshold ? HealthTier.High
                : currentHealth > mediumThreshold ? HealthTier.Medium
                : HealthTier.Low;

            SwapAvatar(targetTier);
        }

        private void OnPlayerResurrected(Il2CppSLZ.Marrow.RigManager rigManager)
        {
            if (!EnableModEntry.Value || Player.RigManager == null) { return; }

            SwapAvatar(HealthTier.High);
        }

        private void SwapAvatar(HealthTier tier, bool force = false)
        {
            if (!force && tier == CurrentTier) { return; }

            Barcode avatar = tier switch
            {
                HealthTier.High => AvatarHigh,
                HealthTier.Medium => AvatarMedium,
                HealthTier.Low => AvatarLow,
                _ => AvatarHigh
            };

            Player.RigManager.SwapAvatarCrate(avatar);
            CurrentTier = tier;
        }

        private void SetAvatar(HealthTier tier)
        {
            if (Player.RigManager == null) { return; }

            Barcode avatar = Player.RigManager.AvatarCrate.Barcode;

            MelonPreferences_Entry<string> savedEntry;
            string tierLabel;

            switch (tier)
            {
                case HealthTier.High:
                    AvatarHigh = avatar;
                    savedEntry = SavedAvatarHigh;
                    tierLabel = "High";
                    break;
                case HealthTier.Medium:
                    AvatarMedium = avatar;
                    savedEntry = SavedAvatarMedium;
                    tierLabel = "Medium";
                    break;
                default:
                    AvatarLow = avatar;
                    savedEntry = SavedAvatarLow;
                    tierLabel = "Low";
                    break;
            }

            savedEntry.Value = avatar.ToString();

            var notif = new Notification
            {
                Title = $"{tierLabel} avatar set to:",
                Message = avatar.ID,
                Type = NotificationType.Success,
                PopupLength = 1.25f,
                ShowTitleOnPopup = true
            };

            Notifier.Send(notif);
        }
    }
}
