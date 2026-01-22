using MelonLoader;
using BoneLib;
using BoneLib.Notifications;
using UnityEngine;
using Il2CppSLZ.Marrow.Warehouse;

namespace VitalShift {
    public partial class Core : MelonMod {

        private void SetAvatar() {
            if (Player.RigManager == null) return;
            if (AvatarHigh.ID == null) return;
            HighHealthThreshold = Player.RigManager.health.max_Health * 0.7f;
            MediumHealthThreshold = Player.RigManager.health.max_Health * 0.3f;

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
                Player.RigManager.SwapAvatarCrate(TargetAvatar);
                CurrentAvatarSet = TargetAvatar;
            }
        }

        private void SetAvatarHigh() {
            if (Player.RigManager == null) return;
            AvatarHigh = Player.RigManager.AvatarCrate.Barcode;

                var notif = new Notification {
                Title = "High avatar set to:",
                Message = AvatarHigh.ID,
                Type = NotificationType.Success,
                PopupLength = 1.25f,
                ShowTitleOnPopup = true
            };
            
            Notifier.Send(notif);
            SavedAvatarHigh.Value = AvatarHigh.ToString();
            MelonPreferences.Save();
        }
        
        private void SetAvatarMedium() {
            if (Player.RigManager == null) return;
            AvatarMedium = Player.RigManager.AvatarCrate.Barcode;
                var notif = new Notification {
                Title = "Medium avatar set to:",
                Message = AvatarMedium.ID,
                Type = NotificationType.Success,
                PopupLength = 1.25f,
                ShowTitleOnPopup = true
            };
            
            Notifier.Send(notif);
            SavedAvatarMedium.Value = AvatarMedium.ToString();
            MelonPreferences.Save();
        }
        
        private void SetAvatarLow() {
            if (Player.RigManager == null) return;
            AvatarLow = Player.RigManager.AvatarCrate.Barcode;
                var notif = new Notification {
                Title = "Low avatar set to:",
                Message = AvatarLow.ID,
                Type = NotificationType.Success,
                PopupLength = 1.25f,
                ShowTitleOnPopup = true
            };
            
            Notifier.Send(notif);
            SavedAvatarLow.Value = AvatarLow.ToString();
            MelonPreferences.Save();
        }
    }
}