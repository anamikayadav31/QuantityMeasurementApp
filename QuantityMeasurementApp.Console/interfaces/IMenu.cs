namespace QuantityMeasurementApp.ConsoleApp.Interfaces
{
    /// <summary>
    /// Interface for any Menu class in the Quantity Measurement Console Application.
    /// Allows different menu implementations to be swapped easily.
    /// </summary>
    public interface IMenu
    {
        /// <summary>
        /// Displays the menu, reads user input, and executes the selected operation.
        /// Loops until the user chooses to exit.
        /// </summary>
        void Show();
    }
}