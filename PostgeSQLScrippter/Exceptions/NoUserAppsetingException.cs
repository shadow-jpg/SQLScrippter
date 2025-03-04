namespace SqlScrippter.Exceptions
{
    class NoUserAppsetingException : NoAppsetingException
    {
        public override string ErrorMessage { get; } = "File with configuration not found!";

    }
}
