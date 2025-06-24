using CSRedis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Educational.Tools
{
    public class RedisHelp<T>
    {
        CSRedisClient redis;

        public RedisHelp(CSRedisClient redis)
        {
            this.redis = redis;
        }

        public async Task<IList<T>> GetRedisList(string key, Func<Task<IList<T>>> func, int time)
        {
            var redisinfo = await redis.GetAsync<IList<T>>(key);
            if (redisinfo != null)
            {
                return redisinfo;
            }
            var list = await func();
            await redis.SetAsync(key, list, time);
            return list;
        }
    }
}
