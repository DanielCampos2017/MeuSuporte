using System;
using System.Collections;
using System.Collections.Generic;
using System.Management;

namespace MeuSuporte
{
    public class Class_SystemRestorePoints : IList
    {
        private List<Class_PointItem> systemRestorePoints = new List<Class_PointItem>();

        public Class_SystemRestorePoints()
        {
            ManagementClass mc = new ManagementClass(@"root\default:SystemRestore");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (Environment.OSVersion.Version.Major < 6)
                {
                    if (Convert.ToUInt32(mo.GetPropertyValue("RestorePointType").ToString()) != 13)
                    {
                        systemRestorePoints.Add(new Class_PointItem(
                            mo.GetPropertyValue("Description").ToString(),
                            Convert.ToUInt32(mo.GetPropertyValue("RestorePointType").ToString()),
                            Convert.ToUInt32(mo.GetPropertyValue("EventType").ToString()),
                            Convert.ToUInt32(mo.GetPropertyValue("SequenceNumber").ToString()),
                            mo.GetPropertyValue("CreationTime").ToString()));
                    }
                }
                else
                {
                    systemRestorePoints.Add(new Class_PointItem(
                        mo.GetPropertyValue("Description").ToString(),
                        Convert.ToUInt32(mo.GetPropertyValue("RestorePointType").ToString()),
                        Convert.ToUInt32(mo.GetPropertyValue("EventType").ToString()),
                        Convert.ToUInt32(mo.GetPropertyValue("SequenceNumber").ToString()),
                        mo.GetPropertyValue("CreationTime").ToString()));
                }
            }
        }

        #region Codigo gerado por membros do IList

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public object this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Codigo gerado por membros do ICollection

        public int Count
        {
            get { return systemRestorePoints.Count; }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Codigo gerado por membros IEnumerable

        public IEnumerator GetEnumerator()
        {
            return systemRestorePoints.GetEnumerator();
        }

        #endregion
    }
}
