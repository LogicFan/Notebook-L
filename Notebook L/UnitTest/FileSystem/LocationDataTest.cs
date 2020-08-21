using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.Serialization;
using Notebook_L.FileSystem;
using Newtonsoft.Json;
using System.Text;

namespace UnitTest.FileSystem
{
    [TestClass]
    public class LocationDataTest
    {
        private const String TestName = "ynNWTQI5R1i9A1KM";
        private const String TestCredential = "FdfEdNQdR9bHCwI1";
        private const LocationData.SourceType TestSource = LocationData.SourceType.OneDrive;

        [TestMethod]
        public void JsonSerialization()
        {
            LocationData orig = new LocationData
            {

                Name = TestName,
                Source = TestSource,
                Credential = TestCredential
            };
            String str = JsonConvert.SerializeObject(orig);
            LocationData targ = JsonConvert.DeserializeObject<LocationData>(str);

            Assert.AreEqual(orig.Name, targ.Name);
            Assert.AreEqual(orig.Source, targ.Source);
            Assert.AreEqual(orig.Credential, targ.Credential);
        }

        [TestMethod]
        public void CredentialEncryption()
        {
            LocationData orig = new LocationData
            {
                Name = TestName,
                Source = TestSource,
                Credential = TestCredential
            };

            String str = JsonConvert.SerializeObject(orig);
            MockLocationData targ = JsonConvert.DeserializeObject<MockLocationData>(str);

            Assert.AreNotEqual(orig.Credential, targ.Credential);
        }
    }

    [Serializable]
    class MockLocationData : ISerializable
    {
        private readonly Byte[] m_credential;

        public String Credential => Encoding.UTF8.GetString(m_credential);
        
        public MockLocationData(SerializationInfo info, StreamingContext context)
        {
            m_credential = (Byte[])info.GetValue("Credential", typeof(Byte[]));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
