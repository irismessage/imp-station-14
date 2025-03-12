using Content.Shared.Damage;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared._Impstation.CosmicCult.Components;

/// <summary>
/// Component for Cosmic Cult equipment items.
/// </summary>
[NetworkedComponent, RegisterComponent]
public sealed partial class CosmicEquipmentComponent : Component
{
    public DamageSpecifier RepelDamage = new()
    {
        DamageDict = new() {
            { "Asphyxiation", 10 }
        }
    };

}
