using QuantityMeasurementApp.ModelLayer.Enums;

namespace QuantityMeasurementApp.BussinessLayer.Converters
{
    /// <summary>
    /// Parses unit strings from QuantityDTO into strongly-typed enum values,
    /// and resolves the MeasurementType for any unit string.
    ///
    /// Lives in the Business Layer because it contains logic —
    /// it is not a plain data holder.
    /// </summary>
    public static class UnitParser
    {
        /// <summary>
        /// Determines the MeasurementType for a unit string.
        /// Tries each enum in order; throws if no match found.
        /// </summary>
        public static MeasurementType GetMeasurementType(string unit)
        {
            string u = unit.ToUpperInvariant();
            if (Enum.TryParse<LengthUnit>(u, out _))      return MeasurementType.LENGTH;
            if (Enum.TryParse<WeightUnit>(u, out _))      return MeasurementType.WEIGHT;
            if (Enum.TryParse<VolumeUnit>(u, out _))      return MeasurementType.VOLUME;
            if (Enum.TryParse<TemperatureUnit>(u, out _)) return MeasurementType.TEMPERATURE;
            throw new InvalidOperationException($"Unknown unit: '{unit}'");
        }

        public static LengthUnit ParseLength(string unit)
        {
            if (Enum.TryParse<LengthUnit>(unit.ToUpperInvariant(), out var r)) return r;
            throw new InvalidOperationException($"Invalid LengthUnit: '{unit}'");
        }

        public static WeightUnit ParseWeight(string unit)
        {
            if (Enum.TryParse<WeightUnit>(unit.ToUpperInvariant(), out var r)) return r;
            throw new InvalidOperationException($"Invalid WeightUnit: '{unit}'");
        }

        public static VolumeUnit ParseVolume(string unit)
        {
            if (Enum.TryParse<VolumeUnit>(unit.ToUpperInvariant(), out var r)) return r;
            throw new InvalidOperationException($"Invalid VolumeUnit: '{unit}'");
        }

        public static TemperatureUnit ParseTemperature(string unit)
        {
            if (Enum.TryParse<TemperatureUnit>(unit.ToUpperInvariant(), out var r)) return r;
            throw new InvalidOperationException($"Invalid TemperatureUnit: '{unit}'");
        }
    }
}
