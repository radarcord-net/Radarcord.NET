using Radarcord.Errors;

namespace Radarcord.Enums;

public enum IntervalPreset
{
    Default,
    Safe,
    SuperSafe,
    ExtraSafe,
    SuperOmegaSafe
}

public static class IntervalPresetMethods
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
