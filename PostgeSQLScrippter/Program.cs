using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlScrippter.Exceptions;
using SqlScrippter.SQL;
using SqlScrippter.SQL.scriptures;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System.Data;

namespace MyApp // Note: actual namespace depends on the project name.
{
    /// <summary>
    /// criticalErrorIsNecessary = true because handling  for fatal errors of ORM
    /// lanuages accaptable for errors  English(main), Spanish, Portuguese, Russian,German
    /// </summary>
    class Program
    {
        private static ILogger<Program> logger;
        private static readonly ResourceManager ResourceManager = new ResourceManager("SQLScrippter.Properties.Resources", typeof(Program).Assembly);

        public async static Task Main()
        {

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddNLog("nlog.config");
            });

            logger = loggerFactory.CreateLogger<Program>();


            logger.LogInformation(Directory.GetCurrentDirectory());
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsetings.json", optional: true, reloadOnChange: true);
            IConfiguration configJson = builder.Build(); 
            var appSettings = configJson.GetSection("AppSettings"); 
            if (!bool.TryParse(configJson["AppSettings:criticalErrorIsNecessary"], out bool criticalErrorIsNecessary))
            {
                #if DEBUG
                var st = new StackTrace(true);
                var frame = st.GetFrame(0);
                int line = frame.GetFileLineNumber()-2;
                logger.LogWarning(ResourceManager.GetString("NoDataInJSON") + "main class. line:" + line);
                #endif
            }

            // установка языка
            string languageForComments = string.IsNullOrWhiteSpace(configJson["AppSettings:language"]) ? "english" : configJson["AppSettings:language"].ToLower();
            SetLanguage(languageForComments);

            //все критические ошибки валят библиотеку по умолчанию
            if (criticalErrorIsNecessary)
                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    if (args.ExceptionObject is CriticalException ex)
                    {
                        logger.LogError(ResourceManager.GetString("CriticalError"));

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
            string index = Console.ReadLine();
            if (index.Contains("c"))
                constraint = true;
            else if (index.Contains("i"))
                constraint = false;
            else throw new Exception("no i or c included");
            ConstraintConsole(constraint);
            string key = Console.ReadLine();
            while (key != "0")
            {
                string foreignTable = "";
                string type = "";
                bool isId = false;
                Console.WriteLine("тип поля дата d,  дробное f, i for целые, k если это foreign key");
                string typeOfString = Console.ReadLine();
                if (typeOfString.ToLower().Contains("d"))
                    type = config.timezones;
                else if (typeOfString.ToLower().Contains("f"))
                    type = config.doubles;
                else if (typeOfString.ToLower().Contains("i"))
                    type = config.ints;
                else if (typeOfString.ToLower().Contains("k"))
                {
                    type = config.ids;
                    isId = true;
                    Console.WriteLine("напишите table на который указывает");
                    foreignTable = Console.ReadLine();
                }
                else
                    throw new Exception(" тип не соответсувет возможным");

                uq_Key.Add(new LibraryOFStructs.Types(type, key, isId, foreignTable));
                ConstraintConsole(constraint);
                key = Console.ReadLine();
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
            if (updateTable.Count != 0)
                Console.WriteLine(PotgreSricpt.Update("source_temp", update, paramName, updateTable, updateColumn, updateMappings, updateMappingsColumn, MappingsConnectionColumn));
            Console.WriteLine(PotgreSricpt.Upsert("source_temp", result_table, uq_Key, update, paramName, updateTable, updateColumn, updateMappings, updateMappingsColumn, MappingsConnectionColumn, constraint));

            return;
        }
        private static void SetLanguage(string language)
        {
            try
            {
                CultureInfo current = new CultureInfo(language.ToLower().Substring(0, 2));
                Thread.CurrentThread.CurrentCulture = current;
                Thread.CurrentThread.CurrentUICulture = current;
            }
            catch(Exception e)
            {
                var culture = new CultureInfo("en-EN");
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                logger.LogError("ошибка такого языка нет:{lang}", language);

            }
        }
        public static void ConstraintConsole(bool constraint)
        {
            if (constraint)
                Console.WriteLine("введите поля входящие в ключ или 0 для завершения ключа:");
            else
                Console.WriteLine("введите поля входящие в unique key или 0 для завершения ключа:");
        }

    }
}