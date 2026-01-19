using MelonLoader;
using UnityEngine;

namespace VitalShift {
    public partial class Core : MelonMod {

        MelonPreferences_Category category;

        MelonPreferences_Entry<bool> ImmortalEntry;

        private bool NeedsHeal = false;
        private float RespawnTime;

    }
}