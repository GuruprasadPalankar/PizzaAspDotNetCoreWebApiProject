using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaAspDotNetCoreWebApiProject.NLog
{
    public interface IPizzaNLog
    {
        void Information(string logMessage);

        void Error(string logMessage);
    }
}
