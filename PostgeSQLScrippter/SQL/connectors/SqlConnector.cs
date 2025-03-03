namespace SqlScrippter.SQL.connectors
{
    abstract class SqlConnector
    {
        private readonly string _connectionString;
        abstract public int ExecuteNonQuery(string sql);
        abstract public List<Dictionary<string, object>> ExecuteQuery(string sql);
        abstract public int ExecuteNonQueryWithParameters(string sql, Dictionary<string, object> parameters);
        abstract public List<Dictionary<string, object>> ExecuteQueryWithParameters(string sql, Dictionary<string, object> parameters);
    }
}
