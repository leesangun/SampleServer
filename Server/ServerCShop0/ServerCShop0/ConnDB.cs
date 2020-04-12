using Lib;
using StackExchange.Redis;
using System;
using static Lib.LibMySql;

namespace ServerCShop0
{
    class ConnMySql
    {
        private readonly LibMySql _mysql;
        public ConnMySql()
        {
            _mysql = new LibMySql(Config._optionsMySql); //MySQL 이 여러개인 경우 추가됨
        }

        public void Select0(DelegateResult delegateResult)
        {
            _mysql.CommandReturn(
                "SELECT * FROM logPortfolio LIMIT 0,5",
                delegateResult
            );
        }
    }

    class ConnRedis
    {
        private readonly IDatabase 
            _database0,
            _database1,
            _database2;

        private readonly TimeSpan SESSION_TIME = TimeSpan.FromSeconds(Config.SEC_USER_SESSION);

        public ConnRedis()
        {
            var client = ConnectionMultiplexer.Connect(Config._optionsRedis); //레디스서버가 여러개인 경우 추가됨

            _database0 = client.GetDatabase();
            _database1 = client.GetDatabase(1);
            _database2 = client.GetDatabase(2);
        }

        public void SetUserKey(string userKey,string userId)
        {
            _database0.StringSet(userKey, userId, SESSION_TIME);
        }
        public void SetUserKeyExpire(string userKey)
        {
            _database0.KeyExpire(userKey, SESSION_TIME);
        }
        public string GetUserKey(string userKey)
        {
            return _database0.StringGet(userKey);
        }

        public static void Test()
        {
            ConnRedis conn = new ConnRedis();
            conn.SetUserKey("user0Key","user0");

            string result = conn.GetUserKey("user0Key");
            if(result == null)
            {
                Console.WriteLine("User not found");
            }
            else
            {
                Console.WriteLine(result);
            }
            
        }
    }
}
