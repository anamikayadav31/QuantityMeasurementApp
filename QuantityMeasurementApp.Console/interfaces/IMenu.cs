namespace QuantityMeasurementApp.ConsoleApp.Interfaces
{
    // ── UC15: N-Tier Architecture – Dependency Inversion ─────────────────
    // The application's entry point (Program.cs) depends on this interface,
    // not the concrete Menu class.
    // This makes it easy to replace the menu with a GUI or web UI later.
    public interface IMenu
    {
        /// <summary>
        /// Show the menu and loop until the user chooses to exit.
        /// </summary>
        void Show();
    }
}