using System.Threading.Tasks;

namespace Sample
{
    public interface IARepository
    {
        public int GetMemberCount(int vipLevel);

        public int GetMemberWebCount();

        public int GetMemberMobileCount();
        public Task<int> GetMemberCountAsync(int vipLevel);
    }

    public class ARepository : IARepository
    {
        public int GetMemberCount(int vipLevel)
        {
            return 100;
        }

        public int GetMemberWebCount()
        {
            return 100;
        }

        public int GetMemberMobileCount()
        {
            return 100;
        }

        public async Task<int> GetMemberCountAsync(int vipLevel)
        {
            await Task.Delay(10);
            return 100;
        }

    }
}