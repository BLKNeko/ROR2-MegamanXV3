using RoR2;
using RoR2.Achievements;

namespace MegamanXMod.Survivors.X.Achievements
{
    // "Capsule Hunter": como X, tenha pelo menos 20 itens ao mesmo tempo em uma run. Libera: Second (Giga) Armor.
    [RegisterAchievement(identifier, unlockableIdentifier, null, 5, typeof(ServerAchievement))]
    public class XCapsuleHunterAchievement : XServerTrackedAchievement
    {
        public const string identifier = XSurvivor.MEGAMAN_x_PREFIX + "CapsuleHunterAchievement";
        public const string unlockableIdentifier = XSurvivor.MEGAMAN_x_PREFIX + "SecondArmorUnlockable";

        public const int requiredItems = 20;

        // Itens "de verdade" (sem itens escondidos/consumidos, que não têm tier visível)
        private static readonly ItemTier[] countedTiers =
        {
            ItemTier.Tier1, ItemTier.Tier2, ItemTier.Tier3, ItemTier.Boss, ItemTier.Lunar,
            ItemTier.VoidTier1, ItemTier.VoidTier2, ItemTier.VoidTier3, ItemTier.VoidBoss,
        };

        private class ServerAchievement : BaseServerAchievement
        {
            public override void OnInstall()
            {
                base.OnInstall();
                Inventory.onInventoryChangedGlobal += OnInventoryChanged;
            }

            public override void OnUninstall()
            {
                Inventory.onInventoryChangedGlobal -= OnInventoryChanged;
                base.OnUninstall();
            }

            private void OnInventoryChanged(Inventory inventory)
            {
                CharacterBody body = GetCurrentBody();
                if (!body || body.inventory != inventory)
                    return;

                int total = 0;
                foreach (ItemTier tier in countedTiers)
                    total += inventory.GetTotalItemCountOfTier(tier);

                if (total >= requiredItems)
                    Grant();
            }
        }
    }
}
