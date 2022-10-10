using Sharpbot.Services.Data.Models;
using System;
using System.Threading.Tasks;

namespace Sharpbot.Services.Data.SQLStatements.Starboard
{
    class StarEntryUpdate : Statment
    {
        private StarEntry starEntry;

        public StarEntryUpdate(StarEntry starEntry)
        {
            this.starEntry = starEntry;
        }

        public override Task Execute()
        {
            throw new NotImplementedException();
        }
    }
}
