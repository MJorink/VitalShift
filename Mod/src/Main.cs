using MelonLoader;
using UnityEngine;
using BoneLib;
using BoneLib.Notifications;
using Il2CppSLZ.Marrow.Warehouse;
using jlib;

namespace vitalshift
{
	public class VitalShift : MelonMod
	{
		public const string Version = "3.1.0";

		private const string DefaultAvatar = "SLZ.BONELAB.Content.Avatar.FordBW";
		private readonly string[] tiers = { "High", "Medium", "Low" };
		private readonly Color[] tierColors = { Color.green, Color.yellow, Color.red };

		private ModPage menu;
		private MelonPreferences_Entry<bool> enableMod;
		private readonly MelonPreferences_Entry<string>[] avatars = new MelonPreferences_Entry<string>[3];

		public override void OnInitializeMelon()
		{
			menu = JLib.Register("VitalShift", Color.red);
			enableMod = menu.Bool("Enable Mod", true, Color.green);

			for (int i = 0; i < tiers.Length; i++)
			{
				int tier = i;
				avatars[tier] = menu.Hidden("Avatar " + tiers[tier], DefaultAvatar);
				menu.Function($"Set {tiers[tier]} HP Avatar", tierColors[tier], () => SetAvatar(tier));
			}
		}

		public override void OnUpdate()
		{
			var health = JLib.playerHealth;
			if (!enableMod.Value || health == null) return;

			var rig = Player.RigManager;
			string current = rig.AvatarCrate.Barcode.ID;

			// Only manage avatars picked in the menu, so any other avatar stays untouched.
			if (current != avatars[0].Value && current != avatars[1].Value && current != avatars[2].Value) return;

			float ratio = health.curr_Health / health.max_Health;
			string target = avatars[ratio > 0.7f ? 0 : ratio > 0.3f ? 1 : 2].Value;

			if (target != current) rig.SwapAvatarCrate(new Barcode(target));
		}

		private void SetAvatar(int tier)
		{
			string barcode = Player.RigManager.AvatarCrate.Barcode.ID;
			menu.Save(avatars[tier], barcode);

			Notifier.Send(new Notification
			{
				Title = $"{tiers[tier]} tier set to:",
				Message = barcode,
				Type = NotificationType.Success,
				PopupLength = 1f,
				ShowTitleOnPopup = true
			});
		}
	}
}
