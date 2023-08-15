using System;

namespace Bytehide.Shield.Client.Models
{
    public static class ShieldConfigurationPresets
    {
        [Flags]
        public  enum Presets { Maximum = 0, Balance = 1, Optimized = 2 }
        public static Presets ToPreset(this string s)
        {
            return s.ToLower() switch
            {
                "maximum" => Presets.Maximum,
                "balance" => Presets.Balance,
                "optimized" => Presets.Optimized,
                _ => Presets.Balance
            };
        }
        public static string ToPresetString(this Presets s)
        {
            return s switch
            {
                Presets.Maximum => "maximum",
                Presets.Balance => "balance",
                Presets.Optimized => "optimized",
                _ => "custom"
            };
        }
    }
}
