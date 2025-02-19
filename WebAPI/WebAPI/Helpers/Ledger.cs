namespace WebAPI.Helpers
{
    public class Ledger
    {

        public static void LogError(string msg)
        {
            //_logger.LogInformation("Processing Employee: {EmployeeCode}");
            //_logger.LogWarning("Something looks off...");
            //_logger.LogError("An error occurred: {ErrorMessage}");

            //System.Diagnostics.Debug.WriteLine("Debugging: Attendance record processing...");


            System.Diagnostics.Debug.WriteLine("________________________________________________________\n\n");
            System.Diagnostics.Debug.WriteLine("Log:\t" + msg);
            System.Diagnostics.Debug.WriteLine("\n\n________________________________________________________\n");
        }
    }
}
