using System;
using System.Threading.Tasks;

namespace Sample
{
    public interface IAService
    {
        public int GetMemberCountByVIPLevel(int vipLevel);
        public int GetMemberCountByPlatform(PlatformEnum platform);
        public Task<int> GetMemberCountByVIPLevelAsync(int vipLevel);
        public void ThrowException();
        public string GetDatetime();
        public string GetTest();
    }

    public class AService : IAService
    {
        private readonly IARepository _aRepository;

        public AService(IARepository aRepository)
        {
            _aRepository = aRepository;
        }

        /// <summary>
        /// 取玩家數量
        /// </summary>
        /// <param name="vipLevel">VIP等級</param>
        /// <returns>玩家數量</returns>
        public int GetMemberCountByVIPLevel(int vipLevel)
        {
            return _aRepository.GetMemberCount(vipLevel) + 1;
        }

        /// <summary>
        /// 取玩家數量V2
        /// </summary>
        /// <param name="platform">平台</param>
        /// <returns>玩家數量</returns>
        public int GetMemberCountByPlatform(PlatformEnum platform)
        {
            return platform == PlatformEnum.Web ?
                    _aRepository.GetMemberWebCount() :
                    _aRepository.GetMemberMobileCount();
        }

        public async Task<int> GetMemberCountByVIPLevelAsync(int vipLevel)
        {
            return await _aRepository.GetMemberCountAsync(vipLevel) + 1;
        }

        public void ThrowException()
        {
            throw new Exception("123");
        }

        public string GetDatetime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public string GetTest()
        {
            return Helper.Test();
        }
    }

    public class A
    {
    }

    public static class Helper
    {
        public static string Test() => "123";
    }
}
