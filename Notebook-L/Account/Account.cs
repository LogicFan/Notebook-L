using System;
using Notebook_L;

namespace Notebook_L.Account
{
    class Account
    {
        public Serializable.Account Data { get; set; }

        public String UIName => Data.Name;
        public String UISource => Data.Source.ToString("G");

        public Account(Serializable.Account data)
        {
            this.Data = data;
        }
    }
}
