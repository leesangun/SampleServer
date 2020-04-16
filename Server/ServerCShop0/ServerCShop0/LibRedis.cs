using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

//NuGet StackExchange.Redis

//실제 사용하지 않음 ConnDB.cs 참조
namespace Lib
{
    class LibRedis
    {
        protected IDatabase _currDatabase;
        private Dictionary<int, IDatabase> _databases = new Dictionary<int, IDatabase>();

        public LibRedis(ConfigurationOptions options, params int[] dbs)
        {
            //var client = ConnectionMultiplexer.Connect("localhost");
            var client = ConnectionMultiplexer.Connect(options);

            if (dbs.Length == 0)
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
                ArgumentException argEx = new System.ArgumentException("Index is out of range", "index");
                throw argEx;
            }

        }

        
        public virtual bool KeyExists(string key)
        {
            return _currDatabase.KeyExists(key);
        }


        public virtual bool StringSet(string key, string value)
        {
            return _currDatabase.StringSet(key, value);
        }


        public virtual bool StringSet(string key, byte[] value)
        {
            return _currDatabase.StringSet(key, value);
        }


        public virtual bool StringSet(string key, byte[] value, double timeSec)
        {
            return _currDatabase.StringSet(key, value, TimeSpan.FromSeconds(timeSec));
        }


        public virtual byte[] StringGet(string key)
        {
            return _currDatabase.StringGet(key);
        }


        public virtual string StringGetString(string key)
        {
            //return _currDatabase.StringGetAsync(key).ToString();
            return _currDatabase.StringGet(key).ToString();
        }


        public virtual byte[] StringGetSet(string key, string value)
        {
            return _currDatabase.StringGetSet(key, value);
        }


        public virtual bool KeyDelete(string key)
        {
            return _currDatabase.KeyDelete(key);
        }


        public virtual bool KeyExpire(string key, double timeSec)
        {
            return _currDatabase.KeyExpire(key, TimeSpan.FromSeconds(timeSec));
        }


        public virtual TimeSpan? KeyTimeToLive(string key)
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
            return this.StringSet(key, bytes);
        }

        public T Get<T>(string key) where T : class
        {
            var readOnlySpan = new ReadOnlySpan<byte>(this.StringGet(key));
            return JsonSerializer.Deserialize<T>(readOnlySpan);
        }

        public static void Test()
        {
            LibRedis r = new LibRedis(Config._optionsRedis, 0);
            r.StringSet("key00", "value00");

            Console.WriteLine(r.StringGetString("key00"));

            ServerCShop0.ProtocolObject._resMessage.key = Protocol.EnumKey.resMessage;
            ServerCShop0.ProtocolObject._resMessage.result = Protocol.EnumResResult.success;
            ServerCShop0.ProtocolObject._resMessage.message = "응답111";
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(ServerCShop0.ProtocolObject._resMessage, Config._jsonSerializerOptions);
            r.StringSet("key01", bytes);

            bytes = r.StringGet("key01");
            var readOnlySpan = new ReadOnlySpan<byte>(bytes);
            Protocol.ResMessage res = JsonSerializer.Deserialize<Protocol.ResMessage>(readOnlySpan);
            Console.WriteLine(res.message);

            ServerCShop0.ProtocolObject._resMessage.key = Protocol.EnumKey.resMessage;
            ServerCShop0.ProtocolObject._resMessage.result = Protocol.EnumResResult.success;
            ServerCShop0.ProtocolObject._resMessage.message = "응답222";
            r.Set<Protocol.ResMessage>("key02", ServerCShop0.ProtocolObject._resMessage);

            res = r.Get<Protocol.ResMessage>("key02");
            Console.WriteLine(res.message);


            Console.WriteLine(r.KeyExists("key02"));
        }

        public static void TestReset()
        {
            LibRedis r = new LibRedis(Config._optionsRedis, 0);
            r.KeyDelete("key00");
            r.KeyDelete("key01");
            r.KeyDelete("key02");
        }
    }

    class LibRedisAsync : LibRedis
    {
        public LibRedisAsync(ConfigurationOptions options, params int[] dbs) : base(options, dbs)
        {
            
        }

        public override bool KeyExists(string key)
        {
            Task<bool> redisValue = _currDatabase.KeyExistsAsync(key);
            redisValue.Wait();
            return redisValue.Result;
        }

        public override bool StringSet(string key, string value)
        {
            Task<bool> redisValue = _currDatabase.StringSetAsync(key, value);
            redisValue.Wait();
            return redisValue.Result;
        }

        public override bool StringSet(string key, byte[] value, double timeSec)
        {
            Task<bool> redisValue = _currDatabase.StringSetAsync(key, value, TimeSpan.FromSeconds(timeSec));
            redisValue.Wait();
            return redisValue.Result;
        }

        public override bool StringSet(string key, byte[] value)
        {
            Task<bool> redisValue = _currDatabase.StringSetAsync(key, value);
            redisValue.Wait();
            return redisValue.Result;
        }

        public override byte[] StringGet(string key)
        {
            Task<RedisValue> redisValue = _currDatabase.StringGetAsync(key);
            redisValue.Wait();
            return redisValue.Result;
        }

        public override string StringGetString(string key)
        {
            Task<RedisValue> redisValue = _currDatabase.StringGetAsync(key);
            redisValue.Wait();
            return redisValue.Result.ToString();
        }

        // KEY : VALUE를 설정하고, KEY 에 대해 이전 값을 가져옵니다.
        public override byte[] StringGetSet(string key, string value)
        {
            Task<RedisValue> redisValue = _currDatabase.StringGetSetAsync(key, value);
            redisValue.Wait();
            return redisValue.Result;
        }

        public override bool KeyDelete(string key)
        {
            Task<bool> redisValue = _currDatabase.KeyDeleteAsync(key);
            redisValue.Wait();
            return redisValue.Result;
        }

        public override bool KeyExpire(string key, double timeSec)
        {
            Task<bool> redisValue = _currDatabase.KeyExpireAsync(key, TimeSpan.FromSeconds(timeSec));
            redisValue.Wait();
            return redisValue.Result;
        }

        public override TimeSpan? KeyTimeToLive(string key)
        {
            Task<TimeSpan?> redisValue = _currDatabase.KeyTimeToLiveAsync(key);
            redisValue.Wait();
            return redisValue.Result;
        }

    }
}
