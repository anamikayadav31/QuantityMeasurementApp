using QuantityMeasurementApp.ModelLayer.DTO;

namespace QuantityMeasurementApp.BussinessLayer.Interfaces
{
    // ── UC15: N-Tier Architecture ────────────────────────────────────────
    // Dependency Inversion Principle:
    //   The controller depends on THIS interface, not the concrete class.
    //   This means we can swap the implementation without changing the controller.
    //
    // All five operations the app supports:
    //   Compare   – are two quantities equal? returns bool
    //   Convert   – change unit (e.g. FEET → INCHES)
    //   Add       – sum two quantities, result in first quantity's unit
    //   Subtract  – difference, result in first quantity's unit
    //   Divide    – ratio (dimensionless double, not a QuantityDTO)
    public interface IQuantityMeasurementService
    {
        /// <summary>Returns true if both quantities represent the same physical amount.</summary>
        bool Compare(QuantityDTO q1, QuantityDTO q2);

        /// <summary>Converts a quantity to the specified target unit.</summary>
        QuantityDTO Convert(QuantityDTO q, string targetUnit);

        /// <summary>Adds two quantities; result is in q1's unit.</summary>
        QuantityDTO Add(QuantityDTO q1, QuantityDTO q2);

        /// <summary>Subtracts q2 from q1; result is in q1's unit.</summary>
        QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2);

        /// <summary>Divides q1 by q2; returns a dimensionless ratio.</summary>
        double Divide(QuantityDTO q1, QuantityDTO q2);
    }
}