using MelonLoader;
using UnityEngine;

namespace VitalShift {
    public partial class Core : MelonMod {

        MelonPreferences_Category category;

        MelonPreferences_Entry<bool> KnockedEntry;
        MelonPreferences_Entry<float> DeadDurationEntry;
        MelonPreferences_Entry<float> KnockedDurationEntry;

        private bool IsKnocked = false;
        private bool IsDead = false;
        private float KnockedTime;
        private float DeathTime;
        private bool ragdollingknocked = false;
        private bool ragdollingdead = false;
        private float ragdollknockedstart;
        private float ragdolldeadstart;

    }
}