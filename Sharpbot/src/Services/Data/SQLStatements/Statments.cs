using Npgsql;
using System.Threading.Tasks;

namespace Sharpbot.Services.Data.SQLStatements
{
    public abstract class Statment
    {
        protected NpgsqlCommand Statement { get; set; }


        public abstract Task Execute();
    }
}
