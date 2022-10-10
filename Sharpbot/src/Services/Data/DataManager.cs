using Sharpbot.Services.Data.Models;
using Sharpbot.Services.Data.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sharpbot.Services.Data
{
    public static class DataManager
    {
        public static Queue<IDatabaseObject> DatabaseCommits { get; private set; }

        public static void Build()
        {
            DatabaseCommits = new Queue<IDatabaseObject>();
        }

        internal static void Close()
        {
            throw new NotImplementedException();
        }

    }
}
