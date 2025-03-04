using Microsoft.Extensions.Configuration;
using SqlScrippter.Exceptions;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("XunitTesting")]
namespace SqlScrippter.supporters
{
    internal class ConfigSearcher
    {
        private string configFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private int depth;
        private string checkedFile;
        public ConfigSearcher()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsetings.json", optional: true, reloadOnChange: true);
            IConfiguration config = builder.Build();

            int.TryParse(config["AppSettings:SearchDepth"], out depth); 

            FindConfigFiles();
        }
        public ConfigSearcher(IConfiguration config)
        {
            int depth = int.Parse(config["AppSettings:SearchDepth"]);
            FindConfigFiles();
        }
        public string FindConfigFiles(string searchPattern = "config.*")
        {
            for (int i = 0; i < depth; i++)
            {
                checkedFile = configFile;
                configFile = Directory.GetParent(configFile).FullName;


                if (!Directory.Exists(configFile))
                {
                    throw new DirectoryNotFoundException($"Директория не найдена: {configFile}");
                }

                if (SearchDirectory(configFile, searchPattern, checkedFile))
                    return configFile;
            }
            throw new NoUserAppsetingException(depth);
        }

        private bool SearchDirectory(string directoryPath, string searchPattern, string checkedFile)
        {
            try
            {
                foreach (var file in Directory.GetFiles(directoryPath, searchPattern))
                {
                    configFile = file;
                    return true;
                }

                foreach (var subDirectory in Directory.GetDirectories(directoryPath))
                {
                    if (subDirectory != checkedFile && SearchDirectory(subDirectory, searchPattern, checkedFile))
                        return true;
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"Нет доступа к директории: {directoryPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске в директории {directoryPath}: {ex.Message}");
            }

            return false;
        }
    }
}
