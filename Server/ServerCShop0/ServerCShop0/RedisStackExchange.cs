using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

//NuGet StackExchange.Redis
namespace Lib
{
    class RedisStackExchange
    {
        private IDatabase _currDatabase;
        private Dictionary<int, IDatabase> _databases = new Dictionary<int, IDatabase>();

        public RedisStackExchange(ConfigurationOptions options,params int[] dbs)
        {
            //var client = ConnectionMultiplexer.Connect("localhost");
            var client = ConnectionMultiplexer.Connect(options);

            if(dbs.Length == 0)
            {
                _currDatabase = client.GetDatabase();
            }
            else
            {
                foreach (int db in dbs)
                {
                    _databases.Add(db, client.GetDatabase(db));
                }
                _currDatabase = _databases[dbs[0]];
            }
            
        }

        public void Select(int db)
        {
            if (_databases.ContainsKey(db))
            {
                _currDatabase = _databases[db];
            }
            else
            {
                System.ArgumentException argEx = new System.ArgumentException("Index is out of range", "index");
                throw argEx;
            }
            
        }

        public bool KeyExistsAsync(string key)
        {
            Task<bool> redisValue = _currDatabase.KeyExistsAsync(key);
            redisValue.Wait();
            return redisValue.Result;
        }
        public bool KeyExists(string key)
        {
            return _currDatabase.KeyExists(key);
        }

        public bool StringSetAsync(string key,string value)
        {
            Task<bool> redisValue = _currDatabase.StringSetAsync(key, value);
            redisValue.Wait();
            return redisValue.Result;
        }
        public bool StringSet(string key, string value)
        {
            return _currDatabase.StringSet(key, value);
        }

        public bool StringSetAsync(string key, byte[] value)
        {
            Task<bool> redisValue = _currDatabase.StringSetAsync(key, value);
            redisValue.Wait();
            return redisValue.Result;
        }
        public bool StringSet(string key, byte[] value)
        {
            return _currDatabase.StringSet(key, value);
        }

        public bool StringSetAsync(string key, byte[] value, double timeSec)
        {
            Task<bool> redisValue = _currDatabase.StringSetAsync(key, value, TimeSpan.FromSeconds(timeSec));
            redisValue.Wait();
            return redisValue.Result;
        }
        public bool StringSet(string key, byte[] value, double timeSec)
        {
            return _currDatabase.StringSet(key, value, TimeSpan.FromSeconds(timeSec));
        }

        public byte[] StringGetAsync(string key)
        {
            Task<RedisValue> redisValue = _currDatabase.StringGetAsync(key);
            redisValue.Wait();
            return redisValue.Result;
        }
        public byte[] StringGet(string key)
        {
            return _currDatabase.StringGet(key);
        }

        public string StringGetAsyncString(string key)
        {
            Task<RedisValue> redisValue = _currDatabase.StringGetAsync(key);
            redisValue.Wait();
            return redisValue.Result.ToString();
        }
        public string StringGetString(string key)
        {
            return _currDatabase.StringGetAsync(key).ToString();
        }

        // KEY : VALUE를 설정하고, KEY 에 대해 이전 값을 가져옵니다.
        public byte[] StringGetSetAsync(string key, string value)
        {
            Task<RedisValue> redisValue = _currDatabase.StringGetSetAsync(key, value);
            redisValue.Wait();
            return redisValue.Result;
        }
        public byte[] StringGetSet(string key, string value)
        {
            return _currDatabase.StringGetSet(key, value);
        }

        public bool KeyDeleteAsync(string key)
        {
            Task<bool> redisValue = _currDatabase.KeyDeleteAsync(key);
            redisValue.Wait();
            return redisValue.Result;
        }
        public bool KeyDelete(string key)
        {
            return _currDatabase.KeyDelete(key);
        }

        public bool KeyExpireAsync(string key,double timeSec)
        {
            Task<bool> redisValue = _currDatabase.KeyExpireAsync(key, TimeSpan.FromSeconds(timeSec));
            redisValue.Wait();
            return redisValue.Result;
        }
        public bool KeyExpire(string key, double timeSec)
        {
            return _currDatabase.KeyExpire(key, TimeSpan.FromSeconds(timeSec));
        }

        public TimeSpan? KeyTimeToLiveAsync(string key)
        {
            Task<TimeSpan?> redisValue = _currDatabase.KeyTimeToLiveAsync(key);
            redisValue.Wait();
            return redisValue.Result;
        }
        public TimeSpan? KeyTimeToLive(string key)
        {
            return _currDatabase.KeyTimeToLive(key);
        }

        public bool Set<T>(string key, T value, DateTimeOffset expiresAt) where T : class
        {
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(value, Config._jsonSerializerOptions);
            var expiration = expiresAt.Subtract(DateTimeOffset.Now);

            return _currDatabase.StringSet(key, bytes, expiration);
        }

        public bool Set<T>(string key, T value, double timeSec) where T : class
        {
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(value, Config._jsonSerializerOptions);
            // return _database.StringSet(key, bytes, TimeSpan.FromSeconds(timeSec));
            return this.StringSet(key, bytes, timeSec);
        }

        public bool Set<T>(string key, T value) where T : class
        {
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(value, Config._jsonSerializerOptions);
            return this.StringSet(key,bytes);
        }

        public T Get<T>(string key) where T : class
        {
            var readOnlySpan = new ReadOnlySpan<byte>(this.StringGetAsync(key));
            return JsonSerializer.Deserialize<T>(readOnlySpan);
        }

        public static void Test()
        {
            RedisStackExchange r = new RedisStackExchange(Config._optionsRedis,0);
            r.StringSet("key00", "value00");

            Console.WriteLine(r.StringGetAsyncString("key00"));

            ServerCShop0.ProtocolObject._resMessage.key = Protocol.EnumKey.resMessage;
            ServerCShop0.ProtocolObject._resMessage.result = Protocol.EnumResResult.SUCCESS;
            ServerCShop0.ProtocolObject._resMessage.message = "응답111";
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(ServerCShop0.ProtocolObject._resMessage, Config._jsonSerializerOptions);
            r.StringSet("key01", bytes);

            bytes = r.StringGetAsync("key01");
            var readOnlySpan = new ReadOnlySpan<byte>(bytes);
            Protocol.ResMessage res = JsonSerializer.Deserialize<Protocol.ResMessage>(readOnlySpan);
            Console.WriteLine(res.message);

            ServerCShop0.ProtocolObject._resMessage.key = Protocol.EnumKey.resMessage;
            ServerCShop0.ProtocolObject._resMessage.result = Protocol.EnumResResult.SUCCESS;
            ServerCShop0.ProtocolObject._resMessage.message = "응답222";
            r.Set<Protocol.ResMessage>("key02", ServerCShop0.ProtocolObject._resMessage);

            res = r.Get<Protocol.ResMessage>("key02");
            Console.WriteLine(res.message);


            Console.WriteLine(r.KeyExists("key02"));
        }

        public static void TestReset()
        {
            RedisStackExchange r = new RedisStackExchange(Config._optionsRedis, 0);
            r.KeyDelete("key00");
            r.KeyDelete("key01");
            r.KeyDelete("key02");
        }
    }
}
