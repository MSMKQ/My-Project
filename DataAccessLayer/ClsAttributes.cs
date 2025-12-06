using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ClsIgnoreAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ClsTableAttribute : Attribute
    {
        public string Name { get; }
        public ClsTableAttribute(string name) => Name = name;

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ClsKeyAttribute : Attribute
    {
        public string Name { get; }
        public ClsKeyAttribute(string name) => Name = name;
    }
}
