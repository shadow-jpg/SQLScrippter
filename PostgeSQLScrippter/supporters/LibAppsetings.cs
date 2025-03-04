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
        public LibAppsetings(int SearchDepth, string configure, bool criticalErrorIsNecessary, string language,string configurePattern)
        {
            LibAppsetings.SearchDepth = SearchDepth;
            LibAppsetings.configure = configure;
            LibAppsetings.configurePattern = configurePattern;
            LibAppsetings.criticalErrorIsNecessary = criticalErrorIsNecessary;
            LibAppsetings.language = language;
        }
        private static string Position = "AppSettings";
        private static string Section = "Orm";

        private static int SearchDepth  = 4;
        private static string configure  = String.Empty;
        private static string configurePattern = "config.*";
        private static bool criticalErrorIsNecessary = true;
        private static string language = String.Empty;

        public static string getPosition()
        {
            return Position;
        }
        public static string getSection()
        {
            return Section;
        }
        public static string getConfigurePattern()
        {
            return configurePattern;
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
