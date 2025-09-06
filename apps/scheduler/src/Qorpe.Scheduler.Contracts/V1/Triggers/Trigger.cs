namespace Qorpe.Scheduler.Contracts.V1.Triggers;

/// <summary>Base trigger dto with discriminator.</summary>
public abstract record Trigger(
    TriggerType Type, 
    string Name, 
    string Group, 
    DateTimeOffset? StartAtUtc, 
    DateTimeOffset? EndAtUtc);
