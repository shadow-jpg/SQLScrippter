using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScrippter.SQL
{
    internal class LibraryOFStructs
    {
        public struct Configuration
        {
            public string timezones = "time stamp wihtout time zone";
            public string doubles = "double precision";
            public string ints = "bigint";
            public string ids = "bigint";
            public string chars = "character varyiyng";

            public Configuration() { }
            public Configuration(string timezones, string doubles, string ints, string ids, string chars)
            {

                this.timezones = timezones;
                this.doubles = doubles;
                this.ints = ints;
                this.ids = ids;
                this.chars = chars;

            }
        }
        public struct UpdateType
        {
            public UpdateType()
            {
            }
            private int noData = -1;
            private int none = 0;
            private int update = 1;
            private int withDictionary = 2;

            public int NoData { get => noData; }
            public int None { get => none; }
            public int Update { get => update; }
            public int WithDictionary { get => withDictionary; }
        }

        public struct Types
        {
            public Types()
            {
            }
            public Types(string type, string name, bool isForeign, string table)
            {
                this.type = type;
                this.name = name;
                this.isForeign = isForeign;
                this.table = table;
            }
            private string type;
            private string name;
            private bool isForeign;
            private string table;
            public string Type { get => type; }
            public string Name { get => name; }
            public bool IsForeign { get => isForeign; }
            public string Table { get => table; }

        }
    }
}
