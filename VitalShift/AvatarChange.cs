using MelonLoader;
using BoneLib;
using UnityEngine;
using Il2CppSLZ.Marrow.Warehouse;

namespace VitalShift {
    public partial class Core : MelonMod {

        private void SetAvatar() {
            if (Player.RigManager == null) return;
            if (AvatarHigh.ID == null) return;
            HighHealthThreshold = Player.RigManager.health.max_Health * 0.7f;
            MediumHealthThreshold = Player.RigManager.health.max_Health * 0.2f;

            Barcode TargetAvatar = null;
            if (Player.RigManager.health.curr_Health >= HighHealthThreshold) {
                TargetAvatar = AvatarHigh;
            }
            else if (Player.RigManager.health.curr_Health >= MediumHealthThreshold) {
                TargetAvatar = AvatarMedium;
            }
            else {
                TargetAvatar = AvatarLow;
            }

            if (CurrentAvatarSet != TargetAvatar) {
                MelonLogger.Msg("Setting Avatar to: " + TargetAvatar.ID);
                Player.RigManager.SwapAvatarCrate(TargetAvatar);
                CurrentAvatarSet = TargetAvatar;
            }
        }

        private void SetAvatarHigh() {
            if (Player.RigManager == null) return;
            AvatarHigh = Player.RigManager.AvatarCrate.Barcode;
            MelonLogger.Msg("Set High HP Avatar to: " + AvatarHigh.ID);
            SavedAvatarHigh.Value = AvatarHigh.ToString();
            MelonPreferences.Save();
        }
        
        private void SetAvatarMedium() {
            if (Player.RigManager == null) return;
            AvatarMedium = Player.RigManager.AvatarCrate.Barcode;
            MelonLogger.Msg("Set Medium HP Avatar to: " + AvatarMedium.ID);
            SavedAvatarMedium.Value = AvatarMedium.ToString();
            MelonPreferences.Save();
        }
        
        private void SetAvatarLow() {
            if (Player.RigManager == null) return;
            AvatarLow = Player.RigManager.AvatarCrate.Barcode;
            MelonLogger.Msg("Set Low HP Avatar to: " + AvatarLow.ID);
            SavedAvatarLow.Value = AvatarLow.ToString();
            MelonPreferences.Save();
        }
    }
}