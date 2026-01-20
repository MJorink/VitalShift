using MelonLoader;
using BoneLib;
using UnityEngine;
using Il2CppSLZ.Marrow.Warehouse;

namespace VitalShift {
    public partial class Core : MelonMod {

        private void SetAvatar() {

            HighHealthThreshold = Player.RigManager.health.max_Health * 0.8f;
            MediumHealthThreshold = Player.RigManager.health.max_Health * 0.3f;

            if (Player.RigManager.health.curr_Health >= HighHealthThreshold) {
                MelonLogger.Msg("Setting Avatar to High HP Avatar: " + AvatarHigh);
                //Player.RigManager.SwapAvatarCrate(AvatarHigh);
            }

            else if (Player.RigManager.health.curr_Health >= MediumHealthThreshold) {
                MelonLogger.Msg("Setting Avatar to Medium HP Avatar: " + AvatarMedium);
                //Player.RigManager.SwapAvatarCrate(AvatarMedium);
            }

            else {
                MelonLogger.Msg("Setting Avatar to Low HP Avatar: " + AvatarLow);
                //Player.RigManager.SwapAvatarCrate(AvatarLow);
            }
        }

        private void SetAvatarHigh() {
            AvatarHigh = Player.RigManager.AvatarCrate.Barcode;
            MelonLogger.Msg("Set High HP Avatar to: " + AvatarHigh);
            //SavedAvatarHigh.Value = AvatarHigh;
        }
        
        private void SetAvatarMedium() {
            AvatarMedium = Player.RigManager.AvatarCrate.Barcode;
            MelonLogger.Msg("Set Medium HP Avatar to: " + AvatarMedium);
            //SavedAvatarMedium.Value = AvatarMedium;
        }
        
        private void SetAvatarLow() {
            AvatarLow = Player.RigManager.AvatarCrate.Barcode;
            MelonLogger.Msg("Set Low HP Avatar to: " + AvatarLow);
            //SavedAvatarLow.Value = AvatarLow;
        }
    }
}