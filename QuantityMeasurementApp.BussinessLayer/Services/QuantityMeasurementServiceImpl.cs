using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Enums;
using QuantityMeasurementApp.BussinessLayer.Interfaces;
using QuantityMeasurementApp.BussinessLayer.Converters;

namespace QuantityMeasurementApp.BussinessLayer.Services
{
    // ── UC13: Centralized Arithmetic Logic (DRY) ─────────────────────────
    // ── UC14: Temperature – Selective Arithmetic ─────────────────────────
    // ── UC15: Service Layer ──────────────────────────────────────────────
    //
    // This is the ONLY place that performs calculations.
    // Steps for every operation:
    //   1. Validate inputs (not null, same category)
    //   2. Convert each operand to its base unit (ToBase)
    //   3. Perform arithmetic in base units
    //   4. Convert result back to the desired output unit (FromBase)
    //   5. Return a new QuantityDTO (immutable result)
    //
    // "Base unit" means:
    //   LENGTH      → FEET
    //   WEIGHT      → KILOGRAM
    //   VOLUME      → LITRE
    //   TEMPERATURE → CELSIUS
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        // Floating-point tolerance: values closer than this are "equal"
        private const double EPSILON = 0.0001;

        // ── COMPARE ──────────────────────────────────────────────────────

        public bool Compare(QuantityDTO q1, QuantityDTO q2)
        {
            ValidateNotNull(q1, nameof(q1));
            ValidateNotNull(q2, nameof(q2));
            ValidateSameCategory(q1, q2, "Compare");
            // Convert both to base units then check difference
            return Math.Abs(ToBase(q1) - ToBase(q2)) < EPSILON;
        }

        // ── CONVERT ──────────────────────────────────────────────────────

        public QuantityDTO Convert(QuantityDTO q, string targetUnit)
        {
            ValidateNotNull(q, nameof(q));
            if (string.IsNullOrWhiteSpace(targetUnit))
                throw new ArgumentException("Target unit cannot be empty.");

            // Check the target unit belongs to the same category
            MeasurementType targetType = UnitParser.GetMeasurementType(targetUnit);
            if (targetType != q.MeasurementType)
                throw new InvalidOperationException(
                    $"Cannot convert {q.MeasurementType} to '{targetUnit}' " +
                    $"which is a {targetType} unit.");

            double converted = FromBase(q.MeasurementType, targetUnit, ToBase(q));
            return new QuantityDTO(converted, targetUnit, q.MeasurementType);
        }

        // ── ADD ───────────────────────────────────────────────────────────

        public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2)
        {
            ValidateNotNull(q1, nameof(q1));
            ValidateNotNull(q2, nameof(q2));
            ValidateSameCategory(q1, q2, "Add");
            ValidateArithmeticAllowed(q1, "Add");   // blocks Temperature

            double sumInBase = ToBase(q1) + ToBase(q2);
            double result    = FromBase(q1.MeasurementType, q1.Unit, sumInBase);
            return new QuantityDTO(result, q1.Unit, q1.MeasurementType);
        }

        // ── SUBTRACT ─────────────────────────────────────────────────────

        public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2)
        {
            ValidateNotNull(q1, nameof(q1));
            ValidateNotNull(q2, nameof(q2));
            ValidateSameCategory(q1, q2, "Subtract");
            ValidateArithmeticAllowed(q1, "Subtract");

            double diffInBase = ToBase(q1) - ToBase(q2);
            double result     = FromBase(q1.MeasurementType, q1.Unit, diffInBase);
            return new QuantityDTO(result, q1.Unit, q1.MeasurementType);
        }

        // ── DIVIDE ────────────────────────────────────────────────────────

        public double Divide(QuantityDTO q1, QuantityDTO q2)
        {
            ValidateNotNull(q1, nameof(q1));
            ValidateNotNull(q2, nameof(q2));
            ValidateSameCategory(q1, q2, "Divide");
            ValidateArithmeticAllowed(q1, "Divide");

            double base2 = ToBase(q2);
            if (Math.Abs(base2) < EPSILON)
                throw new DivideByZeroException(
                    "Cannot divide by zero. The second quantity has a value of zero.");

            return ToBase(q1) / base2;
        }

        // ── PRIVATE: convert a QuantityDTO to its base unit ───────────────

        private static double ToBase(QuantityDTO q) => q.MeasurementType switch
        {
            MeasurementType.LENGTH =>
                LengthUnitConverter.ToBase(UnitParser.ParseLength(q.Unit), q.Value),
            MeasurementType.WEIGHT =>
                WeightUnitConverter.ToBase(UnitParser.ParseWeight(q.Unit), q.Value),
            MeasurementType.VOLUME =>
                VolumeUnitConverter.ToBase(UnitParser.ParseVolume(q.Unit), q.Value),
            MeasurementType.TEMPERATURE =>
                TemperatureUnitConverter.ToBase(UnitParser.ParseTemperature(q.Unit), q.Value),
            _ => throw new InvalidOperationException($"Unsupported category: {q.MeasurementType}")
        };

        // ── PRIVATE: convert a base-unit value back to any target unit ────

        private static double FromBase(MeasurementType type, string unit, double baseValue)
            => type switch
        {
            MeasurementType.LENGTH =>
                LengthUnitConverter.FromBase(UnitParser.ParseLength(unit), baseValue),
            MeasurementType.WEIGHT =>
                WeightUnitConverter.FromBase(UnitParser.ParseWeight(unit), baseValue),
            MeasurementType.VOLUME =>
                VolumeUnitConverter.FromBase(UnitParser.ParseVolume(unit), baseValue),
            MeasurementType.TEMPERATURE =>
                TemperatureUnitConverter.FromBase(UnitParser.ParseTemperature(unit), baseValue),
            _ => throw new InvalidOperationException($"Unsupported category: {type}")
        };

        // ── PRIVATE: validation helpers ───────────────────────────────────

        private static void ValidateNotNull(QuantityDTO? q, string name)
        {
            if (q is null)
                throw new ArgumentNullException(name, "QuantityDTO cannot be null.");
        }

        private static void ValidateSameCategory(QuantityDTO q1, QuantityDTO q2, string op)
        {
            if (q1.MeasurementType != q2.MeasurementType)
                throw new InvalidOperationException(
                    $"Cannot perform '{op}' between {q1.MeasurementType} and {q2.MeasurementType}. " +
                    "Both quantities must belong to the same measurement category.");
        }

        private static void ValidateArithmeticAllowed(QuantityDTO q, string op)
        {
            if (q.MeasurementType == MeasurementType.TEMPERATURE)
                TemperatureUnitConverter.ValidateArithmeticNotSupported(op);
        }
    }
}