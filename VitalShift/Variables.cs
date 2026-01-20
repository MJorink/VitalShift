using MelonLoader;
using UnityEngine;

namespace VitalShift {
    public partial class Core : MelonMod {

        MelonPreferences_Category category;

        MelonPreferences_Entry<bool> ImmortalEntry;
        MelonPreferences_Entry<float> RagdollDurationEntry;

        private bool NeedsHeal = false;
        private float DeathTime;
        private bool ragdolling = false;
        private float ragdollstart;

    }
}