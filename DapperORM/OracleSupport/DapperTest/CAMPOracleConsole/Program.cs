using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMPOracleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            new TestOracleConnections().SingleRecordset();
            new TestOracleConnections().MultipleRecordset();
        }
    }
}
