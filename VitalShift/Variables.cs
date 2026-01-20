using MelonLoader;
using UnityEngine;
using Il2CppSLZ.Marrow.Warehouse;


namespace VitalShift {
    public partial class Core : MelonMod {

        MelonPreferences_Category category;

        MelonPreferences_Entry<bool> KnockedEntry;
        MelonPreferences_Entry<float> DeadDurationEntry;
        MelonPreferences_Entry<float> KnockedDurationEntry;

        //MelonPreferences_Entry<string> SavedAvatarHigh;
        //MelonPreferences_Entry<string> SavedAvatarMedium;
        //MelonPreferences_Entry<string> SavedAvatarLow;

        //Set to Ford by default
        private Barcode AvatarHigh;
        private Barcode AvatarMedium;
        private Barcode AvatarLow;

        private float HighHealthThreshold;
        private float MediumHealthThreshold;

        private bool IsKnocked = false;
        private bool IsDead = false;
        private bool RagdollingKnocked = false;
        private bool RagdollingDead = false;
        private float RagdollKnockedStart;
        private float RagdollDeadStart;



    }
}