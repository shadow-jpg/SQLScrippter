using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;
using MyApp;

namespace XunitTesting
{
    public class MainClassTest
    {
        [Theory]
        [InlineData("true", "english")] 
        [InlineData("false", "ru")]     
        [InlineData("invalid", "en")]
        public void TestProgramInitialization(string criticalErrorValue, string languageValue)
        {
            var config = Substitute.For<IConfiguration>();
            config["AppSettings:criticalErrorIsNecessary"].Returns(criticalErrorValue);
            config["AppSettings:language"].Returns(languageValue);

            var program = new Program();


        }
    }
}
