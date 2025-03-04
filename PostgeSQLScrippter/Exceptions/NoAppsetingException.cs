namespace SqlScrippter.Exceptions
{
    class NoAppsetingException : NoUserAppsetingException
    {
        public override string ErrorMessage { get; } = "File with library configuration not found!";
        public virtual string ErrorCode { get; } = "OrmCritical:NoFile2";
    }
}
