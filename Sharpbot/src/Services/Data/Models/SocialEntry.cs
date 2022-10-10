using Sharpbot.Services.Data.Utility;
using Sharpbot.Services.Data.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sharpbot.Services.Data.Models
{
    public class SocialEntry : IDatabaseObject
    {
        public ulong Id { get; private set; }
        public ulong BotUser { get; private set; }
        public SocialType socialType { get; private set; }
        public string Title { get; set; }
        public string Data { get; set; }
        public bool Share { get; set; }

        public Task PostData()
        {
            throw new NotImplementedException();
        }

        public Task UpdateData()
        {
            throw new NotImplementedException();
        }
    }
}