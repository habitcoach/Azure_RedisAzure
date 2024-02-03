using System;
using System.Threading;
using StackExchange.Redis;

public class Test
{
    public static void Main()
    {
        Console.WriteLine("Caching");
        Console.WriteLine("********");

        var connectionString = "166174redis.redis.cache.windows.net:6380,password=sdnGRgfCeyubZOR1xZdqohyoeB89mt3JsAzCaBbHujA=,ssl=True,abortConnect=False";

        var redisConnection = ConnectionMultiplexer.Connect(connectionString);

        // Accessing a database
        IDatabase db = redisConnection.GetDatabase();

        

        TimeSpan expiration = TimeSpan.FromMinutes(5);

        // Set a key-value pair
        bool wasSet = db.StringSet("Key01", "cached data01 from redis", expiration);
        Console.WriteLine($"Key 'Key01' set: {wasSet}");

        bool wasSet02 = db.StringSet("Key02", "cached data02 from redis", expiration);
        Console.WriteLine($"Key 'Key02' set: {wasSet02}");

        // Retrieve the value of 'Key01'
        string value = db.StringGet("Key01");
        Console.WriteLine($"Value of 'Key01': {value}");

        string value02 = db.StringGet("Key02");
        Console.WriteLine($"Value of 'Key02': {value02}");

        // Create a batch
        var batch = db.CreateBatch();

        // Delete a key
        batch.KeyDeleteAsync("Key02");
      
        

        

        batch.Execute();


        Console.WriteLine("Key02 deleted");

        // Check if a key exists
        bool keyExists = db.KeyExists("Key01");
        Console.WriteLine($"'Key01' exists: {keyExists}");

        bool keyExists02 = db.KeyExists("Key02");
        Console.WriteLine($"'Key02' exists: {keyExists02}");

        // Set a key's expiration time
        db.KeyExpire("Key01", DateTime.Now.AddMinutes(10));

        // Rename a key
        db.KeyRename("Key01", "NewKey01");
        value = db.StringGet("NewKey01");
        Console.WriteLine($"Value of 'NewKey01': {value}");

        // Get the time to live of a key
        TimeSpan? timeToLive = db.KeyTimeToLive("NewKey01");
        Console.WriteLine($"Time to live of 'NewKey01': {timeToLive}");

        // Get the data type of a key
        RedisType keyType = db.KeyType("NewKey01");
        Console.WriteLine($"Data type of 'NewKey01': {keyType}");
    }
}
