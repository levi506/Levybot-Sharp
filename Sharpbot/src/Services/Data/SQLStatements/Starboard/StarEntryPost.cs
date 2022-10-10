using Sharpbot.Services.Data.Models;
using System.Threading.Tasks;

namespace Sharpbot.Services.Data.SQLStatements.Starboard
{
    public class StarEntryPost : Statment
    {
        public StarEntry Entry { get; private set; }

        public StarEntryPost(StarEntry entry)
        {
            Entry = entry;
        }

        public override Task Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
