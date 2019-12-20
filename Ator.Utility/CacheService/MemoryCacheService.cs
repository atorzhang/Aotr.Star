using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ator.Utility.CacheService
{
    public class MemoryCacheService : ICacheService
    {
        public static MemoryCacheService Instance = new MemoryCacheService(new MemoryCache(new MemoryCacheOptions()));
        protected IMemoryCache _cache;
        public const int DefaultexpirationTime = 60;//默认过期时间60s
        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }
        #region 验证缓存项是否存在
        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
                //throw new ArgumentNullException(nameof(key));
            }
            object cached;
            return _cache.TryGetValue(key, out cached);
        }
        #endregion

        #region 获取缓存
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
                //throw new ArgumentNullException(nameof(key));
            }
            var obj = _cache.Get(key);
            return obj != null ? obj as T : default(T);
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
                //throw new ArgumentNullException(nameof(key));
            }
            return _cache.Get(key);
        }
        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            var dict = new Dictionary<string, object>();

            keys.ToList().ForEach(item => dict.Add(item, _cache.Get(item)));

            return dict;
        }
        #endregion

        #region 添加缓存
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <returns></returns>
        public bool Add(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            if (value == null)
            {
                return false;
            }
            _cache.Set(key, value);
            return Exists(key);
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Add(string key, object value, int expiresSliding, int expiressAbsoulte)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            if (value == null)
            {
                return false;
            }
            _cache.Set(key, value,
                    new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(expiresSliding))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(expiressAbsoulte))
                    );

            return Exists(key);
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public bool Add(string key, object value, int expiresIn = DefaultexpirationTime, bool isSliding = false)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            if (value == null)
            {
                return false;
            }
            if (isSliding)
                _cache.Set(key, value,
                    new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(expiresIn))
                    );
            else
                _cache.Set(key, value,
                new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(expiresIn))
                );

            return Exists(key);
        }
        #endregion

        #region 删除缓存
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            if (Exists(key))
            {
                _cache.Remove(key);
            }
            return !Exists(key);
        }
        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="key">缓存Key集合</param>
        /// <returns></returns>
        public bool RemoveAll(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                return false;
            }
            keys.ToList().ForEach(item => _cache.Remove(item));
            return true;
        }
        #endregion

        #region 修改缓存
        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <returns></returns>
        public bool Replace(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            if (value == null)
            {
                return false;
            }
            if (Exists(key))
                if (!Remove(key)) return false;

            return Add(key, value);

        }
        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Replace(string key, object value, int expiresSliding, int expiressAbsoulte)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            if (value == null)
            {
                return false;
            }
            if (Exists(key))
                if (!Remove(key)) return false;

            return Add(key, value, expiresSliding, expiressAbsoulte);
        }
        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public bool Replace(string key, object value, int expiresIn = DefaultexpirationTime, bool isSliding = false)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            if (value == null)
            {
                return false;
            }
            if (Exists(key))
                if (!Remove(key)) return false;

            return Add(key, value, expiresIn, isSliding);
        }
        #endregion

        public void Dispose()
        {
            if (_cache != null)
                _cache.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
