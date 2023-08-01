using Radarcord.Errors;

namespace Radarcord.Enums;

public enum IntervalPreset
{
    /// <summary>
    /// 120 seconds.
    /// </summary>
    Default,
    /// <summary>
    /// 180 seconds.
    /// </summary>
    Safe,
    /// <summary>
    /// 240 seconds.
    /// </summary>
    SuperSafe,
    /// <summary>
    /// 300 seconds.
    /// </summary>
    ExtraSafe,
    /// <summary>
    /// 360 seconds.
    /// </summary>
    SuperOmegaSafe
}

internal static class IntervalPresetMethods
{
    public static int GetInterval(IntervalPreset preset)
    {
        return preset switch
        {
            IntervalPreset.Default => 120,
            IntervalPreset.Safe => 180,
            IntervalPreset.SuperSafe => 240,
            IntervalPreset.ExtraSafe => 300,
            IntervalPreset.SuperOmegaSafe => 360,
            _ => throw new RadarcordException("Invalid interval preset."),
        };
    }
}
