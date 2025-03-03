using System.Globalization;
using System.Resources;
using System.Text;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Utilities;
using SqlScrippter.Exceptions;
using SqlScrippter.SQL;
using SqlScrippter.SQL.scriptures;
namespace MyApp // Note: actual namespace depends on the project name.
{
    /// <summary>
    /// criticalErrorIsNecessary = true because handling  for fatal errors of ORM
    /// </summary>
    class Program
    {
        private static readonly ResourceManager ResourceManager = new ResourceManager("SQLScrippter.Properties.Resources", typeof(Program).Assembly);
        public static int Main()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configJson = builder.Build();
            bool criticalErrorIsNecessary = bool.Parse(configJson["AppSettings:criticalErrorIsNecessary"]);

            // установка языка
            string languageForComments = string.IsNullOrWhiteSpace( configJson["AppSettings:language"]) ? "english":  configJson["AppSettings:language"].ToLower();
            SetLanguage(languageForComments);

            //все критические ошибки валят библиотеку по умолчанию
            if (criticalErrorIsNecessary)
                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    if (args.ExceptionObject is CriticalException ex)
                    {
                        Console.WriteLine(ResourceManager.GetString("CriticalError"));                        

                        //Валим всю прогу
                        Environment.FailFast(ex.ErrorMessage, ex);
                    }
                };


            LibraryOFStructs.Configuration config = new LibraryOFStructs.Configuration();
            LibraryOFStructs.UpdateType updateType = new LibraryOFStructs.UpdateType();
            PostgreSQLScripter PotgreSricpt = new PostgreSQLScripter();


            Console.WriteLine("имя функции:");
            string funcName = Console.ReadLine();
            Console.WriteLine("type name:");
            string typeName = Console.ReadLine();

            int[] update = new int[100];
            for (int j = 0; j < update.Length; j++)
                update[j] = -1;
            List<string> paramName = new();
            List<string> updateTable = new();
            List<string> updateColumn = new();
            List<string> updateMappings = new();
            List<string> updateMappingsColumn = new();
            List<string> MappingsConnectionColumn = new();
            List<LibraryOFStructs.Types> uq_Key = new();
            bool constraint;
            int i = 0;
            Console.WriteLine("таблица для вставки:");
            string result_table = Console.ReadLine();

            Console.WriteLine("ключ constraint / index: c or i");
            if (Console.ReadLine().Contains("c"))
                constraint = true;
            else if (Console.ReadLine().Contains("i"))
                constraint = false;
            else throw new Exception("no i or c included");
            if (constraint)
                Console.WriteLine("введите поля входящие в ключ или 0 для завершения ключа:");
            else
                Console.WriteLine("введите поля входящие в unique key или 0 для завершения ключа:");
            string key = Console.ReadLine();
            while (key != "0")
            {
                string type = "";
                bool isId = false;
                Console.WriteLine("тип поля дата d,  дробное d, i for целые, f если это foreign key");
                if (Console.ReadLine().Contains("d"))
                    type = config.timezones;
                else if (Console.ReadLine().Contains("d"))
                    type = config.doubles;
                else if (Console.ReadLine().Contains("i"))
                    type = config.ints;
             

                string foreignTable = "";
                if (Console.ReadLine().Contains("f"))
                {
                    type = config.ids;
                    isId = true;
                    Console.WriteLine("напишите table на который указывает");
                    foreignTable = Console.ReadLine();
                }
                else
                    throw new Exception(" тип не соответсувет возможным");

                uq_Key.Add(new LibraryOFStructs.Types(type, key, isId, foreignTable));
            }


            while (true)
            {
                Console.WriteLine("построчный ввод нажмите ноль на вводе: другое чтобы продолжить");
                if (Console.ReadLine() == "0")
                    break;
                ++i;
                Console.WriteLine("paramName: пример gtp итог: gtpName если с update без также gtp");
                paramName.Add(Console.ReadLine());

                Console.WriteLine("update with mappings =2 yes=1  no =0:");
                update[i] = (Int32.Parse(Console.ReadLine()));
                if (update[i] == updateType.Update || update[i] == updateType.WithDictionary)
                {
                    Console.WriteLine("таблица для обновления:");
                    updateTable.Add(Console.ReadLine());
                    Console.WriteLine("столбец:");
                    updateColumn.Add(Console.ReadLine());
                    if (update[i] == updateType.WithDictionary)
                    {
                        Console.WriteLine("таблица для dictionary:");
                        updateMappings.Add(Console.ReadLine());
                        Console.WriteLine("столбец mapping:");
                        updateMappingsColumn.Add(Console.ReadLine());
                        Console.WriteLine("столбец связки  с dictionary:");
                        MappingsConnectionColumn.Add(Console.ReadLine());
                    }
                    else
                    {
                        updateMappings.Add("");
                        updateMappingsColumn.Add("");
                        MappingsConnectionColumn.Add("");
                    }
                }
                else
                {
                    updateTable.Add("");
                    updateColumn.Add("");
                    updateMappings.Add("");
                    updateMappingsColumn.Add("");
                    MappingsConnectionColumn.Add("");
                }
            }
            Console.WriteLine(PotgreSricpt.Update("source_temp", update, paramName, updateTable, updateColumn, updateMappings, updateMappingsColumn, MappingsConnectionColumn));
            Console.WriteLine(PotgreSricpt.Upsert("source_temp", result_table, uq_Key, update, paramName, updateTable, updateColumn, updateMappings, updateMappingsColumn, MappingsConnectionColumn, constraint));
            
            return 0;
        }
        private static void SetLanguage(string language)
        {
            // Установка культуры для текущего потока
            var culture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

    }
}