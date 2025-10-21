# 📋 Master Plan CSV Schema

## Overview
Schema for Cassini mission master plan CSV file containing mission observation data.

## File Information
- **Filename**: `master_plan.csv`
- **Total Columns**: 10
- **Estimated Rows**: ~61,874
- **Format**: CSV (Comma-separated values)

## Column Definitions

| Column | Name | Type | Description | Example |
|--------|------|------|-------------|---------|
| 1 | `start_time_utc` | String (DateTime) | Start time in UTC format (YYYY-DDDTHH:MM:SS) | `2004-135T18:40:00` |
| 2 | `duration` | String (Duration) | Duration in format DDDTHH:MM:SS | `000T09:22:00` |
| 3 | `date` | String (Date) | Date in format DD-MMM-YY | `14-May-04` |
| 4 | `team` | String | Team/instrument identifier | `CAPS`, `CDA`, `MAG`, `ISS` |
| 5 | `spass_type` | String | SPASS classification type | `Non-SPASS`, `Prime`, `SPASS Rider` |
| 6 | `target` | String | Observation target | `Saturn`, `rings(general)`, `Earth` |
| 7 | `request_name` | String | Request type identifier | `SURVEY`, `DIFSATSRC`, `TEMPSIT` |
| 8 | `library_definition` | String | Library definition/category | `Magnetospheric survey`, `Diffuse ring satellite search` |
| 9 | `title` | String | Observation title | `MAPS Survey`, `Early search for E Ring` |
| 10 | `description` | String | Detailed description (may contain commas) | `"MAPS magnetospheric survey"` |

## Data Types (SQL/Database)

```sql
CREATE TABLE master_plan (
    start_time_utc TEXT NOT NULL,
    duration TEXT NOT NULL,
    date TEXT NOT NULL,
    team TEXT NOT NULL,
    spass_type TEXT NOT NULL,
    target TEXT NOT NULL,
    request_name TEXT NOT NULL,
    library_definition TEXT NOT NULL,
    title TEXT NOT NULL,
    description TEXT
);
```

## Data Types (C#/Entity Framework)

```csharp
/// <summary>
/// Represents a Cassini mission master plan observation entry
/// </summary>
public class MasterPlanEntry
{
    /// <summary>
    /// Start time in UTC (format: YYYY-DDDTHH:MM:SS)
    /// </summary>
    public string StartTimeUtc { get; set; }
    
    /// <summary>
    /// Duration of observation (format: DDDTHH:MM:SS)
    /// </summary>
    public string Duration { get; set; }
    
    /// <summary>
    /// Date in format DD-MMM-YY
    /// </summary>
    public string Date { get; set; }
    
    /// <summary>
    /// Team or instrument identifier
    /// </summary>
    public string Team { get; set; }
    
    /// <summary>
    /// SPASS classification type
    /// </summary>
    public string SpassType { get; set; }
    
    /// <summary>
    /// Observation target
    /// </summary>
    public string Target { get; set; }
    
    /// <summary>
    /// Request type identifier
    /// </summary>
    public string RequestName { get; set; }
    
    /// <summary>
    /// Library definition or category
    /// </summary>
    public string LibraryDefinition { get; set; }
    
    /// <summary>
    /// Observation title
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Detailed description
    /// </summary>
    public string Description { get; set; }
}
```

## Enumerated Values (Sample)

### Teams
- `CAPS` - Cassini Plasma Spectrometer
- `CDA` - Cosmic Dust Analyzer
- `CIRS` - Composite Infrared Spectrometer
- `ISS` - Imaging Science Subsystem
- `MAG` - Magnetometer
- `MIMI` - Magnetospheric Imaging Instrument
- `RPWS` - Radio and Plasma Wave Science
- `RSS` - Radio Science Subsystem
- `UVIS` - Ultraviolet Imaging Spectrograph
- `VIMS` - Visual and Infrared Mapping Spectrometer

### SPASS Types
- `Prime` - Primary observation
- `SPASS Rider` - Secondary observation riding on a SPASS
- `Non-SPASS` - Non-SPASS observation

### Common Targets
- `Saturn`
- `rings(general)`
- `Earth`
- `InstrumentCalibration`
- `SolarWind`
- `DustRAM direction`
- `co-rotation`
- Satellite names (e.g., Phoebe, Titan, etc.)

## Special Notes

### CSV Parsing Considerations
- **Quoted Fields**: The `description` column contains commas and should be properly quoted
- **Encoding**: UTF-8 recommended
- **Line Endings**: Mixed (may need normalization)
- **Empty Fields**: Some fields may be empty (not shown in sample)

### Date/Time Format Details
- **start_time_utc**: Uses day-of-year format (YYYY-DDD) where DDD is 001-366
- **duration**: Format DDDTHH:MM:SS where DDD is number of days
- **date**: Human-readable format with abbreviated month name

### Data Validation Rules
1. All columns must be present
2. Each row must have exactly 10 columns
3. Quoted fields must have matching opening/closing quotes
4. DateTime and duration fields should follow specified formats
