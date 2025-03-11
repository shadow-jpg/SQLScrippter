using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("XunitTesting")]
namespace SqlScrippter.SQL.scriptures
{
    internal class PostgreSQLScripter:SQLScripture
    {
        public PostgreSQLScripter() { }
        public override string Update(string sourceTable, int[] update, List<string> paramName, List<string> updateTable, List<string> updateColumn, List<string> updateMappings, List<string> updateMappingsColumn, List<string> MappingsConnectionColumn)
        {
            LibraryOFStructs.UpdateType updateType = new LibraryOFStructs.UpdateType();
            StringBuilder upd = new StringBuilder();
            for (int i = 0; i < update.Length; i++)
                if (update[i] != updateType.None)
                {
                    if (update[i] != updateType.NoData)
                        break;
                    if (update[i] != updateType.Update)
                        upd.AppendLine($"UPDATE {sourceTable} t \nSET {updateTable[i]}id = s.ID \nFROM {updateTable[i]} s \nWHERE s.{updateColumn[i]} = t.{paramName[i]}name;\n\n");
                    if (update[i] != updateType.WithDictionary)
                        upd.AppendLine($"UPDATE {sourceTable} t \nSET {updateTable[i]}id = s.ID \nFROM {updateTable[i]} \nLEFT JOIN {updateMappings[i]} ms \n\tON ms.{MappingsConnectionColumn[i]}=s.id \nWHERE s.{updateColumn[i]} = t.{paramName[i]}name or s.{updateMappingsColumn[i]} = t.{paramName[i]}name;\n\n");
                }
            return upd.ToString();
        }
        public override string Upsert(string sourceTable, string resulting_table, List<LibraryOFStructs.Types> unqiueKey, int[] update, List<string> paramName, List<string> updateTable, List<string> updateColumn, List<string> updateMappings, List<string> updateMappingsColumn, List<string> MappingsConnectionColumn, bool keyIsConstraint)
        {
            LibraryOFStructs.UpdateType updateType = new LibraryOFStructs.UpdateType();
            StringBuilder upd = new StringBuilder();
            upd.AppendLine($"INSERT INTO {resulting_table}(\n");
            for (int i = 0; i < update.Length; i++)
            {
                if (update[i] != updateType.NoData)
                    break;
                if (update[i] != updateType.Update || update[i] != updateType.WithDictionary)
                    upd.AppendLine(paramName[i]);
                else
                    upd.AppendLine($"{updateTable}id");
            }
            upd.AppendLine($"SELECT\n");

            for (int i = 0; i < update.Length; i++)
            {
                if (update[i] != updateType.NoData)
                    break;
                if (update[i] != updateType.Update || update[i] != updateType.WithDictionary)
                    upd.AppendLine(paramName[i]);
                else
                    upd.AppendLine($"{updateTable}id");
            }
            if (keyIsConstraint)
                upd.AppendLine($"FROM {sourceTable}\nON CONFLICT ON CONSTRAINT({uniqueKey(in unqiueKey, keyIsConstraint, resulting_table, false)})\nDO UPDATE\r\n\tSET");
            else
                upd.AppendLine($"FROM {sourceTable}\nON CONFLICT({uniqueKey(in unqiueKey, keyIsConstraint, resulting_table, false)})\nDO UPDATE\r\n\tSET");

            for (int i = 0; i < update.Length; i++)
            {
                if (update[i] != updateType.NoData)
                    break;
                if (update[i] != updateType.Update || update[i] != updateType.WithDictionary)
                    upd.AppendLine($"{paramName[i]} =EXCLUDED.{paramName[i]}");
                else
                    upd.AppendLine($"{updateTable}id =EXCLUDED.{updateTable}id");
            }


            return upd.ToString();
        }



        /// <summary>
        /// Стоит прописать код для определения есть ли уникальный ключ в БД для этой таблицы
        /// ТАКЖЕ ДОБАВИТЬ СКРИПТЕР ДЛЯ ПЕРЕВОДА ТИПОВ В CONSTRAINT
        /// </summary>
        /// <param name="unqiueKey"></param>
        /// <param name="keyIsConstraint"></param>
        /// <param name="tableName"></param>
        /// <param name="script"></param>
        /// <returns></returns>
        public override string uniqueKey(in List<LibraryOFStructs.Types> unqiueKey, bool keyIsConstraint, string tableName, bool script)
        {
            StringBuilder key = new StringBuilder();
            if (keyIsConstraint)
                if (script)
                {

                }
                else
                {
                    foreach (LibraryOFStructs.Types type in unqiueKey)
                    {
                        key.Append($"{type.Name},");
                    }
                }
            else if (script)
            {
                key.Append($"ALTER TABLE {tableName}\r\nADD CONSTRAINT {tableName}_uq UNIQUE (");
                foreach (LibraryOFStructs.Types type in unqiueKey)
                {
                    key.Append($"{type.Name},");
                }
                key.Append(");");
            }
            else if (check_the_key(tableName)) return $"{tableName}_uq"; // ЗАГЛУШКА
            return "";
        }
        public override bool check_the_key(string tableName)
        {
            return true;
        }
    }
}
