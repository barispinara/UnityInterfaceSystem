using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityInterfaceSystem.Entities
{
    public class ModelsEntity
    {
        public string Name { get; set; }
        public string Directory { get; set; }

        public ModelsEntity(string Name, string Directory)
        {
            this.Name = Name;
            this.Directory = Directory;
        }
    }
}
