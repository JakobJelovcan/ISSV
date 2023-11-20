using ISSV.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Services.Maps;

namespace ISSV.Services
{
    public static class MapLocationService
    {
        static MapLocationService()
        {
            mapLocationCache = new Dictionary<string, MapLocation>();
        }

        private static readonly Dictionary<string, MapLocation> mapLocationCache;

        private static async Task<MapLocation> FindMapLocationAsync(string address)
        {
            var res = await MapLocationFinder.FindLocationsAsync(address, null, 1);
            if (res.Status == MapLocationFinderStatus.Success)
            {
                return res.Locations.FirstOrDefault();
            }
            return null;
        }

        public static async Task BuildCacheAsync()
        {
        }

        public static async Task<MapLocation> GetMapLocationAsync(string address)
        {
            if (!mapLocationCache.ContainsKey(address))
            {
                mapLocationCache[address] = await FindMapLocationAsync(address);
            }
            return mapLocationCache[address];
        }
    }
}
