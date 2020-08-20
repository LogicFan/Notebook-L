using Integrative.Encryption;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.Serialization;

namespace Notebook_L.FileSystem
{
    [Serializable]
    public sealed class LocationData : ISerializable
    {
        public enum SourceType
        {
            Local,
            OneDrive
        }

        private String m_name;
        private SourceType m_source;
        private Byte[] m_credential;

        public String Name { 
            get { return m_name; } 
            set { m_name = value; } 
        }
        public SourceType Source { 
            get { return m_source; } 
            set { m_source = value; } 
        }
        public String Credential { 
            get
            {
                Byte[] bytes = CrossProtect.Unprotect(m_credential, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(bytes);
            }
            set
            {
                Byte[] bytes = Encoding.UTF8.GetBytes(value);
                m_credential = CrossProtect.Protect(bytes, null, DataProtectionScope.CurrentUser);
            }
        }

        public LocationData() { }

        public LocationData(SerializationInfo info, StreamingContext context)
        {
            m_name = (String)info.GetValue("Name", typeof(String));
            m_source = (SourceType)info.GetValue("Source", typeof(SourceType));
            m_credential = (Byte[]) info.GetValue("Credential", typeof(Byte[]));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", m_name, typeof(String));
            info.AddValue("Source", m_source, typeof(SourceType));
            info.AddValue("Credential", m_credential, typeof(Byte[]));
        }
    }
}
