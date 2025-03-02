using Robust.Shared.GameStates;
using Content.Shared.StatusIcon;
using Robust.Shared.Prototypes;
using Robust.Shared.Audio;
using Content.Shared.Damage;
using Content.Shared._Impstation.CosmicCult.Prototypes;
using Content.Shared.Damage.Prototypes;

namespace Content.Shared._Impstation.CosmicCult.Components;

/// <summary>
/// Added to entities to tag that they are a cosmic cultist. Holds nearly all cultist-relevant data! Removal of this component is used to call for a deconversion
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class CosmicCultComponent : Component
{
    #region Housekeeping

    /// <summary>
    /// The status icon prototype displayed for cosmic cultists.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<FactionIconPrototype> StatusIcon { get; set; } = "CosmicCultIcon";

    [DataField] public bool IsConstruct = false;

    #endregion

    #region Ability Data
    [DataField]
    [AutoNetworkedField]
    public HashSet<ProtoId<InfluencePrototype>> UnlockedInfluences =
    [
        "InfluenceAberrantLapse",
        "InfluenceNullGlare", // This says Glare but it's currently Shunt/Blank
        "InfluenceEschewMetabolism",
    ];

    [DataField]
    [AutoNetworkedField]
    public HashSet<ProtoId<EntityPrototype>> CosmicCultActions =
    [
        "ActionCosmicSiphon",
        "ActionCosmicGlare", // set back to ActionCosmicBlank if playtest go bad
    ];
    public HashSet<EntityUid?> ActionEntities = [];

    [DataField]
    [AutoNetworkedField]
    public HashSet<ProtoId<InfluencePrototype>> OwnedInfluences = [];

    /// <summary>
    /// The duration of the doAfter for Siphon Entropy
    /// </summary>
    [DataField, AutoNetworkedField] public TimeSpan CosmicSiphonDelay = TimeSpan.FromSeconds(2);

    /// <summary>
    /// The duration of the doAfter for Shunt Subjectivity
    /// </summary>
    [DataField, AutoNetworkedField] public TimeSpan CosmicBlankDelay = TimeSpan.FromSeconds(0.6f);

    /// <summary>
    /// The duration of Shunt Subjectivity's trip to the cosmic void
    /// </summary>
    [DataField, AutoNetworkedField] public TimeSpan CosmicBlankDuration = TimeSpan.FromSeconds(22);

    /// <summary>
    /// The range of Null Glare
    /// </summary>
    [DataField, AutoNetworkedField] public int CosmicGlareRange = 10;

    /// <summary>
    /// The amount of Entropy generated by Siphon Entropy
    /// </summary>
    [DataField, AutoNetworkedField] public int CosmicSiphonQuantity = 1;
    /// <summary>
    /// The amount of Entropy the user is allowed to spend at The Monument.
    /// </summary>
    [DataField, AutoNetworkedField] public int EntropyBudget = 0;

    /// <summary>
    /// Wether or not this cultist has been empowered by a Malign Rift.
    /// </summary>
    [DataField, AutoNetworkedField] public bool CosmicEmpowered = false;

    /// <summary>
    /// A string for storing what damage container this cultist had upon conversion.
    /// </summary>
    [DataField, AutoNetworkedField] public ProtoId<DamageContainerPrototype> StoredDamageContainer = "Biological";

    /// <summary>
    /// The asphyx damage to apply upon a successful Siphon Entropy
    /// </summary>
    [DataField, AutoNetworkedField]
    public DamageSpecifier SiphonAsphyxDamage = new()
    {
        DamageDict = new() {
            { "Asphyxiation", 15 }
        }
    };
    /// <summary>
    /// The cold damage to apply upon a successful Siphon Entropy. WTF IS THE CORRECT WAY TO INVOKE A DAMAGEDICT WITH MULTIPLE TYPES!? AAGH. This would save one line of code and this damage specifier entry.
    /// </summary>
    [DataField, AutoNetworkedField]
    public DamageSpecifier SiphonColdDamage = new()
    {
        DamageDict = new() {
            { "Cold", 6 }
        }
    };
    #endregion

    #region VFX & SFX

    [DataField] public EntProtoId SpawnWisp = "MobCosmicWisp";
    [DataField] public EntProtoId LapseVFX = "CosmicLapseAbilityVFX";
    [DataField] public EntProtoId BlankVFX = "CosmicBlankAbilityVFX";
    [DataField] public EntProtoId GlareVFX = "CosmicGlareAbilityVFX";
    [DataField] public EntProtoId AbsorbVFX = "CosmicGenericVFX";
    [DataField] public EntProtoId ImpositionVFX = "CosmicImpositionAbilityVFX";
    [DataField] public SoundSpecifier BlankSFX = new SoundPathSpecifier("/Audio/_Impstation/CosmicCult/ability_blank.ogg");
    [DataField] public SoundSpecifier IngressSFX = new SoundPathSpecifier("/Audio/_Impstation/CosmicCult/ability_ingress.ogg");
    [DataField] public SoundSpecifier GlareSFX = new SoundPathSpecifier("/Audio/_Impstation/CosmicCult/ability_glare.ogg");
    [DataField] public SoundSpecifier NovaCastSFX = new SoundPathSpecifier("/Audio/_Impstation/CosmicCult/ability_nova_cast.ogg");
    [DataField] public SoundSpecifier ImpositionSFX = new SoundPathSpecifier("/Audio/_Impstation/CosmicCult/ability_imposition.ogg");

    #endregion
}
