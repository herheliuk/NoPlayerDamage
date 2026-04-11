namespace Oxide.Plugins;

[Info("No PvP", "&anhe", "1.1.9")]
[Description("Prevents PvP damage unless teammates.")]
public class NoPvP : RustPlugin
{
    private object OnEntityTakeDamage(BaseCombatEntity target, HitInfo info) =>
        // Prevent if
        (
            // Attacker is Player
            info.InitiatorPlayer is {} attacker &&
            (
                // PvP
                (
                    // Target is Player
                    target is BasePlayer victim &&
                    // Not an NPC
                    !victim.IsNpc &&
                    // Not a team
                    (
                        attacker.currentTeam == 0 ||
                        attacker.currentTeam != victim.currentTeam
                    )
                ) ||
                // Body gathering
                (
                    // Target is a Corpse
                    target is LootableCorpse corpse &&
                    // Not an NPC
                    corpse.playerSteamID > 76561197960265728 &&
                    // Not self
                    attacker.userID != corpse.playerSteamID &&
                    // Not a team
                    RelationshipManager.ServerInstance.FindTeam(attacker.currentTeam)
                        ?.members.Contains(corpse.playerSteamID) != true
                )
            ) &&
            // Not admin
            !attacker.IsAdmin
        )
            ? false : null;
}