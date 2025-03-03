namespace SqlScrippter.Exceptions
{
    public class CriticalException : Exception
    {

        public DateTime ErrorTime { get; } = DateTime.UtcNow;
        public virtual int ErrorCode { get; }

        public virtual string ErrorMessage { get; } = "Application terminated due to a critical error.";

        private readonly TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

        public CriticalException()
        {
        }

        public CriticalException(string message)
            : base(message)
        {
            Console.WriteLine($"Critical mistake {this.GetType()}, message: {message}");
        }

        public CriticalException(string message, Exception innerException)
            : base(message, innerException)
        {
            Console.WriteLine($"Critical mistake{this.GetType()}, message: {message}");
        }

        async virtual public void ExceptionHandler<T>(T controlVariable)
        {
            if (controlVariable is int intVal)
                TriggerFailFastAfterDelayAsync(intVal);
            else if (controlVariable is string message)
                WaitForNotificationAsync(message);
            else
                Environment.FailFast(ErrorMessage);
        }
        public async Task TriggerFailFastAfterDelayAsync(int delayMilliseconds)
        {
            await Task.Delay(delayMilliseconds);
            Environment.FailFast(ErrorMessage);
        }
        public async Task WaitForNotificationAsync(string message)
        {
            await _tcs.Task;
            if (!string.IsNullOrWhiteSpace(message))
                Console.WriteLine($"Critical mistake {this.GetType()}, message: {message}");
            Environment.FailFast(ErrorMessage);
        }
    }
}
