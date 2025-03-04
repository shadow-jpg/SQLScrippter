using System.Reflection;
using System.Text;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI.Common;
using SqlScrippter.supporters;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static SqlScrippter.handler.ORMClass;
namespace XunitTesting
{
    public class AppsetingsTest
    {
        public string configPattern = "config.json";

        [Fact]
        public void AppSettingsSection_Exists()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetings.json", optional: true, reloadOnChange: true);
            IConfiguration config = builder.Build();
            var appSettings = config.GetSection("AppSettings");

            Assert.True(appSettings.Exists() && appSettings.GetChildren().Any(), "Секция 'AppSettings' не найдена или пуста.");
        }

        [Theory]
        [InlineData("SearchDepth","int")]
        [InlineData("configure", "string")]
        [InlineData("criticalErrorIsNecessary", "bool")]
        [InlineData("language", "string")]
        public void TestInnerConfigure(string key, string expectedType)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsetings.json", optional: true, reloadOnChange: true);
            IConfiguration config = builder.Build();
            var appSettings = config.GetSection("AppSettings");
            if (appSettings.Exists())
            {
                var value = appSettings[key];
                //Assert.True(!string.IsNullOrWhiteSpace(value), $"Значение для ключа '{key}' отсутствует или пустое.");

                switch (expectedType.ToLower())
                {
                    case "int":
                        Assert.True(int.TryParse(value, out _), $"Значение для ключа '{key}' не является целым числом.");
                        break;
                    case "bool":
                        Assert.True(bool.TryParse(value, out _), $"Значение для ключа '{key}' не является булевым значением.");
                        break;
                    case "string":
                        break;
                    default:
                        throw new ArgumentException($"Неизвестный тип данных: {expectedType}");
                }
            }
            else
            {
                // Если секция AppSettings не существует, тест должен провалиться
                Assert.True(false, "Секция 'AppSettings' не найдена в конфигурации.");
            }
        }
   
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void TestFindUsersConfigure(int value)
        {
            string file =prepareUserConfigure(value);
            ConfigSearcher configSearcher = new ConfigSearcher();
            string result = configSearcher.FindConfigFiles(LibAppsetings.getConfigurePattern());
            Assert.True(result == file, $"Test not Found Users Configure resullt: {result} ,\nfile {file}");
        }


        /// <summary>
        /// 0 - берет вложенную директорию
        /// 1 - берет директорию родитель
        /// 2+ - берет директорию родитель предудущего номера
        /// </summary>
        /// <param name="depthOfSearch"></param>
        /// <returns></returns>
        public string prepareUserConfigure(int depthOfSearch = 2)
        {

            string configFile =null;
                switch (depthOfSearch)
            {
                case 0: 
                        configFile = Directory.GetDirectories(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))[0];
                    break;
                default:
                    if (depthOfSearch is int)
                    {
                        string parentFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        for (int i = 1; i < depthOfSearch; i++)
                        {
                            configFile = Directory.GetParent(parentFile).FullName;
                            parentFile = new string(configFile);
                        }
                        configFile = parentFile;
                    }
                    break;

            }
            using (FileStream fs = File.Create($"{configFile}\\{configPattern}"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("language:Postgresql; port =5432; username =admin");
                fs.Write(info, 0, info.Length);
            }
            return $"{configFile}\\{configPattern}";
        }
    }
}
