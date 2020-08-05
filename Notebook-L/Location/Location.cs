using System;
using Notebook_L;

namespace Notebook_L.Location
{
    class Location
    {
        public Serializable.Location Data { get; set; }

        public String UIName => Data.Name;
        public String UISource => Data.Source.ToString("G");

        public Location(Serializable.Location data)
        {
            this.Data = data;
        }
    }
}
