﻿using BenchmarkDotNet.Attributes;
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
        private string sectionName;
        public ConfigSearcher(string sectionName = "Orm")
        {
            this.sectionName = sectionName;

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsetings.json", optional: true, reloadOnChange: true);
            IConfiguration config = builder.Build();

            int.TryParse(config[$"{LibAppsetings.getPosition()}:SearchDepth"], out depth); 

            FindConfigFiles(LibAppsetings.getConfigurePattern());
        }
        public ConfigSearcher(IConfiguration config, string sectionName = "Orm")
        {
            this.sectionName = sectionName;
            int depth = int.Parse(config[$"{LibAppsetings.getPosition()}:SearchDepth"]);
            FindConfigFiles(LibAppsetings.getConfigurePattern());
        }


        public string FindConfigFiles(string searchPattern )
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
                foreach (var file in Directory.GetFiles(directoryPath, LibAppsetings.getSection()))
                {
                    if (ContainsRequiredSection(file, "AppSettings"))
                    {
                        if (ContainsRequiredSection(file, sectionName))
                            configFile = file;
                        return true;
                    }
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



        private bool ContainsRequiredSection(string filePath, string sectionName)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Path.GetDirectoryName(filePath))
                    .AddJsonFile(Path.GetFileName(filePath), optional: true, reloadOnChange: true);

                IConfiguration config = builder.Build();

                return FindSectionRecursive(config, sectionName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке конфигурации из файла {filePath}: {ex.Message}");
                return false;
            }
        }

        private bool FindSectionRecursive(IConfiguration config, string sectionName)
        {
            var section = config.GetSection(sectionName);
            if (section.Exists() && section.GetChildren().Any())
                return true;

            foreach (var child in config.GetChildren())
                if (FindSectionRecursive(child, sectionName))
                    return true;

            return false;
        }
    }
}
