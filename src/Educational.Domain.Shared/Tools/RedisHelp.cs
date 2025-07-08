using CSRedis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Educational.Tools
{
    /// <summary>
    /// Redis帮助类，提供基于泛型的Redis缓存操作
    /// </summary>
    /// <typeparam name="T">缓存数据类型</typeparam>
    public class RedisHelp<T>
    {
        private readonly CSRedisClient redis;

        /// <summary>
        /// 构造函数，注入CSRedisClient实例
        /// </summary>
        /// <param name="redis">CSRedisClient实例</param>
        public RedisHelp(CSRedisClient redis)
        {
            this.redis = redis;
        }

        /// <summary>
        /// 获取Redis列表数据，如果缓存不存在则调用指定函数获取数据并缓存
        /// </summary>
        /// <param name="key">Redis缓存键</param>
        /// <param name="func">当缓存不存在时，用于获取数据的异步函数</param>
        /// <param name="time">缓存过期时间（秒）</param>
        /// <returns>返回数据列表</returns>
        public async Task<List<T>> GetRedisList(string key, Func<Task<List<T>>> func, int time)
        {
            // 尝试从Redis获取缓存数据
            var redisinfo = await redis.GetAsync<List<T>>(key);

            // 如果缓存存在，直接返回缓存数据
            if (redisinfo != null)
            {
                return redisinfo;
            }

            // 缓存不存在，调用传入的函数获取数据
            var list = await func();

            // 将获取到的数据存入Redis缓存
            await redis.SetAsync(key, list, time);

            // 返回数据
            return list;
        }
    }
}
