using QuantityMeasurementApp.ModelLayer.DTO;

namespace QuantityMeasurementApp.BussinessLayer.Interfaces
{
    /// <summary>
    /// Contract for all quantity measurement operations.
    /// Named IQuantityMeasurementService (full spelling) to match
    /// Program.cs and QuantityMeasurementController.
    /// </summary>
    public interface IQuantityMeasurementService
    {
        bool Compare(QuantityDTO q1, QuantityDTO q2);
        QuantityDTO Convert(QuantityDTO q, string targetUnit);
        QuantityDTO Add(QuantityDTO q1, QuantityDTO q2);
        QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2);
        double Divide(QuantityDTO q1, QuantityDTO q2);
    }
}