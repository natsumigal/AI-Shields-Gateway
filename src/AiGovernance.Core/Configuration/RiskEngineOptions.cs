namespace AiGovernance.Core.Configuration;

public sealed class RiskEngineOptions
{
    public const string SectionName = "RiskEngine";

    public decimal LowThreshold { get; set; } = 0.25m;

    public decimal MediumThreshold { get; set; } = 0.5m;

    public decimal HighThreshold { get; set; } = 0.75m;

    public decimal CriticalThreshold { get; set; } = 0.9m;
}
