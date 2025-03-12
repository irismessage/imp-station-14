using Content.Shared.Damage;
using Robust.Shared.GameStates;

namespace Content.Shared._Impstation.CosmicCult.Components;

/// <summary>
/// Makes the target take damage over time.
/// Meant to be used in conjunction with statusEffectSystem.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
[AutoGenerateComponentState]
public sealed partial class CosmicEntropyDebuffComponent : Component
{
    [AutoPausedField] public TimeSpan CheckTimer = default!;
    [DataField, AutoNetworkedField] public TimeSpan CheckWait = TimeSpan.FromSeconds(1);
    /// <summary>
    /// The debuff applied while the component is present.
    /// </summary>
    [DataField]
    public DamageSpecifier Degen = new()
    {
        DamageDict = new()
        {
            { "Cold", 0.25},
            { "Asphyxiation", 1.25},
        }
    };
}
