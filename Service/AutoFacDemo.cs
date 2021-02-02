using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Service
{
    public class AutoFacDemo
    {
        public interface IOutput
        {
            string Write(string content);
        }


        public class ConsoleOutput : IOutput
        {
            public string Write(string content)
            {
               return content;
            }
        }


        public interface IDateWriter
        {
            string WriteDate();
        }


        public class TodayWriter : IDateWriter
        {
            private IOutput _output;
            public TodayWriter(IOutput output)
            {
                this._output = output;
            }

            public string WriteDate()
            {
               return this._output.Write(DateTime.Today.ToShortDateString());
            }
        }
    }
}
