using System.Reflection;
using System.Text;
using SqlScrippter.supporters;
namespace XunitTesting
{
    public class AppsetingsTest
    {
        public string configPattern = "config.json";

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Test1(int value)
        {
            string file =prepare(value);
            ConfigSearcher configSearcher = new ConfigSearcher();
            string result = configSearcher.FindConfigFiles();
            Assert.False(result == file);
        }
        /// <summary>
        /// 0 - берет вложенную директорию
        /// 1 - берет директорию родитель
        /// 2+ - берет директорию родитель предудущего номера
        /// </summary>
        /// <param name="depthOfSearch"></param>
        /// <returns></returns>
        public string prepare(int depthOfSearch = 2)
        {

            string configFile =null;
                switch (depthOfSearch)
            {
                case 0: 
                        configFile = Directory.GetDirectories(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))[0];
                        //foreach (var subDirectory in Directory.GetDirectories(parentDirectory))
                        //    configFile = subDirectory;
                    break;
                case 1:
                    configFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    break;
                case 2:
                    configFile = Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName;
                    break;
                case 3:
                    configFile = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName).FullName;
                    break;
                case 4:
                    configFile = Directory.GetParent(Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName).FullName).FullName;
                    break;
                case 5:
                    configFile = Directory.GetParent(Directory.GetParent(
                        Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName).FullName).FullName).FullName;
                    break;
                default:
                    configFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    break;

            }
            using (FileStream fs = File.Create($"{configFile}\\configPattern"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("language:Postgresql; port =5432; username =admin");
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
            return $"{configFile}\\configPattern";
        }
    }
}
