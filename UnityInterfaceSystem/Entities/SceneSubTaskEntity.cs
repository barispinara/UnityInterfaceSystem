using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UnityInterfaceSystem.Entities
{
    public class SceneSubTaskEntity
    {
        public string Name { get; set; }

        public string Model_name { get; set; }

        public UserControl Screen { get; set; }

        public string Number { get; set; }

        public int start { get; set; }

        public int finish { get; set; }
        public string piece_name { get; set; }

        public string color { get; set; }

        public SceneSubTaskEntity(string name, string number, UserControl screen = null)
        {
            this.Name = name;
            this.Screen = screen;
            this.Number = number;
            this.color = "#ffffff";
        }

        public SceneSubTaskEntity(string name, string modelName, string number, string pieceName, UserControl screen = null)
        {
            this.Name = name;
            this.Model_name = modelName;
            this.Screen = screen;
            this.Number = number;
            this.piece_name = pieceName;

            this.color = "#ffffff";


        }



    }
}
