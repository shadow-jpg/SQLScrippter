using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScrippter.supporters
{
    struct LibAppsetings
    {
        public LibAppsetings()
        {

        }
        public LibAppsetings(int SearchDepth, string configure, bool criticalErrorIsNecessary, string language)
        {
            LibAppsetings.SearchDepth = SearchDepth;
            LibAppsetings.configure = configure;
            LibAppsetings.criticalErrorIsNecessary = criticalErrorIsNecessary;
            LibAppsetings.language = language;
        }
        private string Position = "AppSettings";

        private static int SearchDepth  = 4;
        private static string configure  = String.Empty;
        private static bool criticalErrorIsNecessary = true;
        private static string language = String.Empty;

        public string getPosition()
        {
            return Position;
        }
    }
    struct AppsetingUser
    {
        public AppsetingUser()
        {

        }
        public AppsetingUser(string Position, string language)
        {
            this.Position = Position;
            this.language = language;
        }
        private string Position = "";

        public string language { get; set; } = String.Empty;
        public string getPosition()
        {
            return Position;
        }
    }
}
