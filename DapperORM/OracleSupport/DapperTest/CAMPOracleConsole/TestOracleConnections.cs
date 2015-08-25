using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAMPOracleDataAccess.Data.Repository;
using CAMPOracleDataAccess.OracleUtility;

namespace CAMPOracleConsole
{
    public class TestOracleConnections
    {
        GenericRepository gr = new GenericRepository();

        public void SingleRecordset()
        {
            IEnumerable<dynamic> result = null;
            var spname = "Test2";

            result = gr.QueryDynamic<dynamic>
                    (spname,
                       null,
                       commandType: CommandType.StoredProcedure
                    );
        }


        public void MultipleRecordset()
        {
            IEnumerable<dynamic> result = null;
            var spname = "Test";
            var timeout = 100;

            result = gr.ExecuteDynamic<dynamic>
                    (spname,
                       null,
                       timeout: timeout,
                       commandType: CommandType.StoredProcedure
                    );

        }
    }


}
