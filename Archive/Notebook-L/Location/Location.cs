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
            log.Info(String.Format("Create object, Name = {0}", data.Name));
            this.Data = data;
        }

        public override String ToString()
        {
            return String.Format("Location(Name = {0}, Source = {1})", UIName, UISource);
        }
    }
}
