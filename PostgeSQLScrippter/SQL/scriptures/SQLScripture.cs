using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScrippter.SQL.scriptures
{
    public abstract class SQLScripture
    {
        //public abstract string Update(string sourceTable, int[] update, List<string> paramName, List<string> updateTable, List<string> updateColumn, List<string> updateMappings, List<string> updateMappingsColumn, List<string> MappingsConnectionColumn);
        //public abstract string Upsert(string sourceTable, string resulting_table, List<LibraryOFStructs.Types> unqiueKey, int[] update, List<string> paramName, List<string> updateTable, List<string> updateColumn, List<string> updateMappings, List<string> updateMappingsColumn, List<string> MappingsConnectionColumn, bool keyIsConstraint);

        //public  abstract string uniqueKey(in List<LibraryOFStructs.Types> unqiueKey, bool keyIsConstraint, string tableName, bool script);
        //public static abstract bool check_the_key(string tableName);
    }
}
