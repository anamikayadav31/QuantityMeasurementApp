using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Enums;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.BussinessLayer.Interfaces;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.ConsoleApp.Controllers
{
    public class QuantityMeasurementController
    {
        private readonly IQuantityMeasurementService _service;
        private readonly IQuantityMeasurementRepository _repository;

        public QuantityMeasurementController(
            IQuantityMeasurementService service,
            IQuantityMeasurementRepository repository)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // ── COMPARE ─────────────────────────────────────────

        public void PerformCompareLength()
        {
            var (q1, q2) = ReadTwo("length", "FEET, INCHES, YARDS, CENTIMETERS", MeasurementType.LENGTH);
            RunCompare(q1, q2, "Compare Length");
        }

        public void PerformCompareWeight()
        {
            var (q1, q2) = ReadTwo("weight", "KILOGRAM, GRAM, POUND", MeasurementType.WEIGHT);
            RunCompare(q1, q2, "Compare Weight");
        }

        public void PerformCompareVolume()
        {
            var (q1, q2) = ReadTwo("volume", "LITRE, MILLILITRE, GALLON", MeasurementType.VOLUME);
            RunCompare(q1, q2, "Compare Volume");
        }

        public void PerformCompareTemperature()
        {
            var (q1, q2) = ReadTwo("temperature", "CELSIUS, FAHRENHEIT, KELVIN", MeasurementType.TEMPERATURE);
            RunCompare(q1, q2, "Compare Temperature");
        }

        // ── CONVERT ─────────────────────────────────────────

        public void PerformConvertLength()
        {
            var (q, to) = ReadOneAndTarget("length", "FEET, INCHES, YARDS, CENTIMETERS", MeasurementType.LENGTH);
            RunConvert(q, to, "Convert Length");
        }

        public void PerformConvertWeight()
        {
            var (q, to) = ReadOneAndTarget("weight", "KILOGRAM, GRAM, POUND", MeasurementType.WEIGHT);
            RunConvert(q, to, "Convert Weight");
        }

        public void PerformConvertVolume()
        {
            var (q, to) = ReadOneAndTarget("volume", "LITRE, MILLILITRE, GALLON", MeasurementType.VOLUME);
            RunConvert(q, to, "Convert Volume");
        }

        public void PerformConvertTemperature()
        {
            var (q, to) = ReadOneAndTarget("temperature", "CELSIUS, FAHRENHEIT, KELVIN", MeasurementType.TEMPERATURE);
            RunConvert(q, to, "Convert Temperature");
        }

        // ── ADD ─────────────────────────────────────────

        public void PerformAddLength()
        {
            var (q1, q2) = ReadTwo("length", "FEET, INCHES, YARDS, CENTIMETERS", MeasurementType.LENGTH);
            RunAdd(q1, q2, "Add Length");
        }

        public void PerformAddWeight()
        {
            var (q1, q2) = ReadTwo("weight", "KILOGRAM, GRAM, POUND", MeasurementType.WEIGHT);
            RunAdd(q1, q2, "Add Weight");
        }

        public void PerformAddVolume()
        {
            var (q1, q2) = ReadTwo("volume", "LITRE, MILLILITRE, GALLON", MeasurementType.VOLUME);
            RunAdd(q1, q2, "Add Volume");
        }

        // ── SUBTRACT ─────────────────────────────────────────

        public void PerformSubtractLength()
        {
            var (q1, q2) = ReadTwo("length", "FEET, INCHES, YARDS, CENTIMETERS", MeasurementType.LENGTH);
            RunSubtract(q1, q2, "Subtract Length");
        }

        public void PerformSubtractWeight()
        {
            var (q1, q2) = ReadTwo("weight", "KILOGRAM, GRAM, POUND", MeasurementType.WEIGHT);
            RunSubtract(q1, q2, "Subtract Weight");
        }

        public void PerformSubtractVolume()
        {
            var (q1, q2) = ReadTwo("volume", "LITRE, MILLILITRE, GALLON", MeasurementType.VOLUME);
            RunSubtract(q1, q2, "Subtract Volume");
        }

        // ── DIVIDE ─────────────────────────────────────────

        public void PerformDivideLength()
        {
            var (q1, q2) = ReadTwo("length", "FEET, INCHES, YARDS, CENTIMETERS", MeasurementType.LENGTH);
            RunDivide(q1, q2, "Divide Length");
        }

        public void PerformDivideWeight()
        {
            var (q1, q2) = ReadTwo("weight", "KILOGRAM, GRAM, POUND", MeasurementType.WEIGHT);
            RunDivide(q1, q2, "Divide Weight");
        }

        public void PerformDivideVolume()
        {
            var (q1, q2) = ReadTwo("volume", "LITRE, MILLILITRE, GALLON", MeasurementType.VOLUME);
            RunDivide(q1, q2, "Divide Volume");
        }

        // ── HISTORY ─────────────────────────────────────────

        public void ShowHistory()
        {
            var records = _repository.GetAllAsync();

            Console.WriteLine($"\n──── Operation History ({records.Count} records) ────");

            if (records.Count == 0)
            {
                Console.WriteLine("No operations recorded yet.");
                return;
            }

            foreach (var r in records)
                Console.WriteLine(r);
        }

        // ── RUN METHODS ─────────────────────────────────────────

        private void RunCompare(QuantityDTO q1, QuantityDTO q2, string op)
        {
            try
            {
                bool result = _service.Compare(q1, q2);

                Console.WriteLine($"\nResult: {q1} == {q2} ? → {result}");

                var entity = new QuantityMeasurementEntity(op, q1.ToString(), q2.ToString(), result.ToString());
                _repository.SaveAsync(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR] {ex.Message}");

                var entity = new QuantityMeasurementEntity(op, q1.ToString(), q2.ToString(), ex.Message, true);
                _repository.SaveAsync(entity);
            }
        }

        private void RunConvert(QuantityDTO q, string targetUnit, string op)
        {
            try
            {
                QuantityDTO result = _service.Convert(q, targetUnit);

                Console.WriteLine($"\nResult: {q} = {result}");

                var entity = new QuantityMeasurementEntity(op, q.ToString(), targetUnit, result.ToString());
                _repository.SaveAsync(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR] {ex.Message}");

                var entity = new QuantityMeasurementEntity(op, q.ToString(), null, ex.Message, true);
                _repository.SaveAsync(entity);
            }
        }

        private void RunAdd(QuantityDTO q1, QuantityDTO q2, string op)
        {
            try
            {
                QuantityDTO result = _service.Add(q1, q2);

                Console.WriteLine($"\nResult: {q1} + {q2} = {result}");

                var entity = new QuantityMeasurementEntity(op, q1.ToString(), q2.ToString(), result.ToString());
                _repository.SaveAsync(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR] {ex.Message}");

                var entity = new QuantityMeasurementEntity(op, q1.ToString(), q2.ToString(), ex.Message, true);
                _repository.SaveAsync(entity);
            }
        }

        private void RunSubtract(QuantityDTO q1, QuantityDTO q2, string op)
        {
            try
            {
                QuantityDTO result = _service.Subtract(q1, q2);

                Console.WriteLine($"\nResult: {q1} - {q2} = {result}");

                var entity = new QuantityMeasurementEntity(op, q1.ToString(), q2.ToString(), result.ToString());
                _repository.SaveAsync(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR] {ex.Message}");

                var entity = new QuantityMeasurementEntity(op, q1.ToString(), q2.ToString(), ex.Message, true);
                _repository.SaveAsync(entity);
            }
        }

        private void RunDivide(QuantityDTO q1, QuantityDTO q2, string op)
        {
            try
            {
                double result = _service.Divide(q1, q2);

                Console.WriteLine($"\nResult: {q1} ÷ {q2} = {Math.Round(result, 4)}");

                var entity = new QuantityMeasurementEntity(op, q1.ToString(), q2.ToString(), result.ToString("F4"));
                _repository.SaveAsync(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR] {ex.Message}");

                var entity = new QuantityMeasurementEntity(op, q1.ToString(), q2.ToString(), ex.Message, true);
                _repository.SaveAsync(entity);
            }
        }

        // ── INPUT HELPERS ─────────────────────────────────────────

        private (QuantityDTO q1, QuantityDTO q2) ReadTwo(string typeName, string hint, MeasurementType mt)
        {
            Console.WriteLine($"\nEnter first {typeName} value:");
            double v1 = double.Parse(Console.ReadLine()!);

            Console.WriteLine($"Enter first {typeName} unit ({hint}):");
            string u1 = ReadLine();

            Console.WriteLine($"\nEnter second {typeName} value:");
            double v2 = double.Parse(Console.ReadLine()!);

            Console.WriteLine($"Enter second {typeName} unit ({hint}):");
            string u2 = ReadLine();

            return (new QuantityDTO(v1, u1, mt), new QuantityDTO(v2, u2, mt));
        }

        private (QuantityDTO q, string targetUnit) ReadOneAndTarget(string typeName, string hint, MeasurementType mt)
        {
            Console.WriteLine($"\nEnter {typeName} value:");
            double v = double.Parse(Console.ReadLine()!);

            Console.WriteLine($"Enter unit to convert FROM ({hint}):");
            string from = ReadLine();

            Console.WriteLine($"Enter unit to convert TO ({hint}):");
            string to = ReadLine();

            return (new QuantityDTO(v, from, mt), to);
        }

        private string ReadLine()
        {
            return (Console.ReadLine() ?? "").Trim().ToUpperInvariant();
        }
    }
}