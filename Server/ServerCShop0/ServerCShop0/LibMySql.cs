using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    class LibMySql
    {
        public delegate void DelegateResult(MySqlDataReader rdr);
        private readonly MySqlConnection _mySqlConnection;
        public LibMySql(string options)
        {
            _mySqlConnection = new MySqlConnection(options);
            _mySqlConnection.Open();
        }
        ~LibMySql()
        {
            _mySqlConnection.Close();
        }

        public int Command(string sql)
        {
            MySqlCommand command = new MySqlCommand(sql, _mySqlConnection);
            return command.ExecuteNonQuery();
        }

         public void CommandReturn(string sql, DelegateResult delegateResult)
        {
            using (MySqlCommand command = new MySqlCommand(sql, _mySqlConnection))
            {
                MySqlDataReader rdr = command.ExecuteReader();
                delegateResult(rdr);
                rdr.Close();
            }
        }

        public static void Test()
        {
            LibMySql conn = new LibMySql(Config._optionsMySql);
            conn.CommandReturn(
                "SELECT * FROM logPortfolio LIMIT 0,5",
                (MySqlDataReader rdr) =>
                {
                    string temp = string.Empty;
                    if (rdr == null) temp = "No return";
                    else
                    {
                        while (rdr.Read())
                        {
                            for (int i = 0; i < rdr.FieldCount; i++)
                            {
                                if (i != rdr.FieldCount - 1)
                                    temp += rdr[i] + ";";    // parser 넣어주기
                                else if (i == rdr.FieldCount - 1)
                                    temp += rdr[i] + "\n";
                            }
                        }
                    }
                    Console.WriteLine(temp);
                }
            );
        }
    }
}
