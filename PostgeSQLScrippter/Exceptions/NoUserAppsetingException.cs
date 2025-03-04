namespace SqlScrippter.Exceptions
{
    class NoUserAppsetingException : CriticalException
    {
        public override string ErrorMessage { get; } = "File with user configuration not found!";
        int depth;

        public NoUserAppsetingException() : base()
        {

        }
        public NoUserAppsetingException(int depth) : base()
        {
            this.depth = depth;
        }
        public NoUserAppsetingException(string message, int depth) : base(message)
        {
            this.depth = depth;
        }
        public NoUserAppsetingException(string message, Exception innerException, int Depth) : base(message, innerException)
        {
            this.depth = depth;
        }
    }
}
