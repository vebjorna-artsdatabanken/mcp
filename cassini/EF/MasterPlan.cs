using System;
using System.Collections.Generic;

namespace cassini.EF;

/// <summary>
/// Represents a Cassini mission master plan observation entry
/// </summary>
public partial class MasterPlan
{
    /// <summary>
    /// Primary key identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Start time in UTC (format: YYYY-DDDTHH:MM:SS)
    /// </summary>
    public string StartTimeUtc { get; set; } = null!;

    /// <summary>
    /// Duration of observation (format: DDDTHH:MM:SS)
    /// </summary>
    public string? Duration { get; set; }

    /// <summary>
    /// Date in format DD-MMM-YY
    /// </summary>
    public string? Date { get; set; }

    /// <summary>
    /// Team or instrument identifier (e.g., CAPS, CDA, MAG, ISS, MIMI)
    /// </summary>
    public string? Team { get; set; }

    /// <summary>
    /// SPASS classification type (e.g., Non-SPASS, Prime, SPASS Rider)
    /// </summary>
    public string? SpassType { get; set; }

    /// <summary>
    /// Observation target (e.g., Saturn, rings, moons)
    /// </summary>
    public string? Target { get; set; }

    /// <summary>
    /// Request type identifier (e.g., SURVEY, DIFSATSRC, TEMPSIT)
    /// </summary>
    public string? RequestName { get; set; }

    /// <summary>
    /// Library definition or category
    /// </summary>
    public string? LibraryDefinition { get; set; }

    /// <summary>
    /// Observation title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Detailed description of the observation
    /// </summary>
    public string? Description { get; set; }
}
