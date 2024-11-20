using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Text;
using System.Diagnostics;

public class LogInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        // Metot üzerinde [Log] attribute'u var mı?
        var hasLogAttribute = invocation.Method.GetCustomAttributes(typeof(LogAttribute), true).Any();

        if (hasLogAttribute)
        {
            var methodName = invocation.Method.Name;
            var parameters = invocation.Arguments;
            var parameterNames = invocation.Method.GetParameters().Select(p => p.Name).ToArray();
            var logMessageBuilder = new StringBuilder();

            logMessageBuilder.AppendLine($"[Log] Method '{methodName}' started at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            if (parameters.Any())
            {
                logMessageBuilder.AppendLine("[Log] Parameters:");
                for (int i = 0; i < parameters.Length; i++)
                {
                    logMessageBuilder.AppendLine($"  - {parameterNames[i]}: {parameters[i]}");
                }
            }

            // Start stopwatch to measure execution time
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Metodu çalıştır
                invocation.Proceed();

                stopwatch.Stop();
                logMessageBuilder.AppendLine($"[Log] Method '{methodName}' finished successfully at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                logMessageBuilder.AppendLine($"[Log] Execution time: {stopwatch.ElapsedMilliseconds} ms");

                // Log the return value if applicable
                if (invocation.ReturnValue != null)
                {
                    logMessageBuilder.AppendLine($"[Log] Return Value: {invocation.ReturnValue}");
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                logMessageBuilder.AppendLine($"[Log] Method '{methodName}' threw an exception: {ex.Message}");
                logMessageBuilder.AppendLine($"[Log] Exception Details: {ex}");
                logMessageBuilder.AppendLine($"[Log] Execution time: {stopwatch.ElapsedMilliseconds} ms");

                throw;
            }

            // Output the detailed log message
            Console.WriteLine(logMessageBuilder.ToString());
        }
        else
        {
            // Loglanmayacaksa doğrudan çalıştır
            invocation.Proceed();
        }
    }
}
