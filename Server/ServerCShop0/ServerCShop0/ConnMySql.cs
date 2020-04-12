using Lib;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lib.LibMySql;

namespace ServerCShop0
{
    class ConnMySql
    {
        private readonly LibMySql _mysql;
        public ConnMySql()
        {
            _mysql = new LibMySql(Config._optionsMySql);
        }

        public void Select0(DelegateResult delegateResult)
        {
            _mysql.CommandReturn(
                "SELECT * FROM logPortfolio LIMIT 0,5",
                delegateResult
            );
        }
    }
}
