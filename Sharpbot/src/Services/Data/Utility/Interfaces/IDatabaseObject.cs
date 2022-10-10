using System.Threading.Tasks;

namespace Sharpbot.Services.Data.Utility.Interfaces
{
    public interface IDatabaseObject
    {

        public Task PostData();

        public Task UpdateData();


    }
}
