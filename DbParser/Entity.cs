using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DbParser
{
    public class Entity
    {
        public string Name { get; set; }
        public List<string> PropertyName { get; set; }
        public List<string> PropertyType { get; set; }

        public Entity(string name)
        {
            Name = name;
            PropertyName = new List<string>();
            PropertyType = new List<string>();
        }
    }
}
