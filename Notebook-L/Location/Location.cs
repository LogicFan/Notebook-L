using System;
using MetroLog;

namespace Notebook_L.Location
{
    class Location
    {
        private readonly ILogger log = LogManagerFactory.DefaultLogManager.GetLogger<Location>();

        public Serializable.Location Data { get; }

        public String UIName => Data.Name;
        public String UISource => Data.Source.ToString("G");

        public Location(Serializable.Location data)
        {
            log.Info(String.Format("data.Name = {0}, data.Source = {1}", data.Name, data.Source.ToString("G")));
            this.Data = data;
        }
    }
}
