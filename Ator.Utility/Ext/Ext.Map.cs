using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Utility.Ext
{
    public static partial class Ext
    {
        public static TMap Map<TSource, TMap>(this object source) where TSource : class
        {
            if (source == null) return default;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TMap>());
            var mapper = config.CreateMapper();
            return mapper.Map<TMap>(source);
        }
        public static List<TMap> Map<TSource, TMap>(this IList<TSource> source) where TSource : class
        {
            if (source == null) return default;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TMap>());
            var mapper = config.CreateMapper();
            return mapper.Map<List<TMap>>(source);
        }

    }
}
