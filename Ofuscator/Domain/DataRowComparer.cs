using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Obfuscator.Domain
{
    internal class DataRowComparer : IEqualityComparer<DataRow>
    {
        public bool Equals(DataRow x, DataRow y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null)) return false;

            if (x.ItemArray.Count() != y.ItemArray.Count()) return false;

            for (int i = 0; i < x.ItemArray.Count(); i++)
                if (!x.ItemArray[i].Equals(y.ItemArray[i])) return false;

            return true;
        }

        public int GetHashCode(DataRow dataRow)
        {
            if (Object.ReferenceEquals(dataRow, null)) return 0;
            if (dataRow.ItemArray.Count() == 0) return dataRow.GetHashCode();

            int hashCode = 0;

            for (int i = 0; i < dataRow.ItemArray.Count(); i++)
                hashCode ^= (dataRow.ItemArray[i] == null ? 0 : dataRow.ItemArray[i].GetHashCode());

            return hashCode;
        }
    }
}
