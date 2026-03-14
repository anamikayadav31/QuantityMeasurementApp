using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Enums;
using QuantityMeasurementApp.BussinessLayer.Interfaces;
using QuantityMeasurementApp.BussinessLayer.Converters;

namespace QuantityMeasurementApp.BussinessLayer.Services
{
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private const double EPSILON = 0.0001;

        // ── COMPARE ──────────────────────────────────────────────────────────

        public bool Compare(QuantityDTO q1, QuantityDTO q2)
        {
            ValidateNotNull(q1, nameof(q1));
            ValidateNotNull(q2, nameof(q2));
            ValidateSameCategory(q1, q2, "Compare");
            return Math.Abs(ToBase(q1) - ToBase(q2)) < EPSILON;
        }

        // ── CONVERT ──────────────────────────────────────────────────────────

        public QuantityDTO Convert(QuantityDTO q, string targetUnit)
        {
            ValidateNotNull(q, nameof(q));
            if (string.IsNullOrWhiteSpace(targetUnit))
                throw new ArgumentException("Target unit cannot be null or empty.");

            MeasurementType targetType = UnitParser.GetMeasurementType(targetUnit);
            if (targetType != q.MeasurementType)
                throw new InvalidOperationException(
                    $"Cannot convert {q.MeasurementType} to unit '{targetUnit}' " +
                    $"which belongs to {targetType}.");

            double converted = FromBase(q.MeasurementType, targetUnit, ToBase(q));
            return new QuantityDTO(converted, targetUnit, q.MeasurementType);
        }

        // ── ADD ───────────────────────────────────────────────────────────────

        public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2)
        {
            ValidateNotNull(q1, nameof(q1));
            ValidateNotNull(q2, nameof(q2));
            ValidateSameCategory(q1, q2, "Add");
            ValidateArithmeticSupported(q1, "Add");

            double result = FromBase(q1.MeasurementType, q1.Unit, ToBase(q1) + ToBase(q2));
            return new QuantityDTO(result, q1.Unit, q1.MeasurementType);
        }

        // ── SUBTRACT ─────────────────────────────────────────────────────────

        public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2)
        {
            ValidateNotNull(q1, nameof(q1));
            ValidateNotNull(q2, nameof(q2));
            ValidateSameCategory(q1, q2, "Subtract");
            ValidateArithmeticSupported(q1, "Subtract");

            double result = FromBase(q1.MeasurementType, q1.Unit, ToBase(q1) - ToBase(q2));
            return new QuantityDTO(result, q1.Unit, q1.MeasurementType);
        }

        // ── DIVIDE ────────────────────────────────────────────────────────────

        public double Divide(QuantityDTO q1, QuantityDTO q2)
        {
            ValidateNotNull(q1, nameof(q1));
            ValidateNotNull(q2, nameof(q2));
            ValidateSameCategory(q1, q2, "Divide");
            ValidateArithmeticSupported(q1, "Divide");

            double base2 = ToBase(q2);
            if (Math.Abs(base2) < EPSILON)
                throw new DivideByZeroException(
                    "Division by zero: the second quantity evaluates to zero.");

            return ToBase(q1) / base2;
        }

        // ── PRIVATE: conversion dispatch ──────────────────────────────────────

        private double ToBase(QuantityDTO q)
        {
            return q.MeasurementType switch
            {
                MeasurementType.LENGTH =>
                    LengthUnitConverter.ToBase(UnitParser.ParseLength(q.Unit), q.Value),
                MeasurementType.WEIGHT =>
                    WeightUnitConverter.ToBase(UnitParser.ParseWeight(q.Unit), q.Value),
                MeasurementType.VOLUME =>
                    VolumeUnitConverter.ToBase(UnitParser.ParseVolume(q.Unit), q.Value),
                MeasurementType.TEMPERATURE =>
                    TemperatureUnitConverter.ToBase(UnitParser.ParseTemperature(q.Unit), q.Value),
                _ => throw new InvalidOperationException(
                        $"Unsupported MeasurementType: {q.MeasurementType}")
            };
        }

        private double FromBase(MeasurementType type, string targetUnit, double baseValue)
        {
            return type switch
            {
                MeasurementType.LENGTH =>
                    LengthUnitConverter.FromBase(UnitParser.ParseLength(targetUnit), baseValue),
                MeasurementType.WEIGHT =>
                    WeightUnitConverter.FromBase(UnitParser.ParseWeight(targetUnit), baseValue),
                MeasurementType.VOLUME =>
                    VolumeUnitConverter.FromBase(UnitParser.ParseVolume(targetUnit), baseValue),
                MeasurementType.TEMPERATURE =>
                    TemperatureUnitConverter.FromBase(UnitParser.ParseTemperature(targetUnit), baseValue),
                _ => throw new InvalidOperationException(
                        $"Unsupported MeasurementType: {type}")
            };
        }

        // ── PRIVATE: validation ───────────────────────────────────────────────

        private static void ValidateNotNull(QuantityDTO? q, string paramName)
        {
            if (q == null)
                throw new ArgumentNullException(paramName, "QuantityDTO cannot be null.");
        }

        private static void ValidateSameCategory(QuantityDTO q1, QuantityDTO q2, string op)
        {
            if (q1.MeasurementType != q2.MeasurementType)
                throw new InvalidOperationException(
                    $"Cannot perform '{op}' across different measurement types: " +
                    $"{q1.MeasurementType} and {q2.MeasurementType}.");
        }

        private static void ValidateArithmeticSupported(QuantityDTO q, string op)
        {
            if (q.MeasurementType == MeasurementType.TEMPERATURE)
                TemperatureUnitConverter.ValidateArithmeticNotSupported(op);
        }
    }
}