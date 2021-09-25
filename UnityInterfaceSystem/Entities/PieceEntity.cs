using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;

namespace UnityInterfaceSystem.Entities
{
    public class PieceEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public int Start { get; set; }
        public int Finish { get; set; }


        public PieceEntity(int id, string Name, string Model, int Start, int Finish)
        {
            this.ID = id;
            this.Name = Name;
            this.Model = Model;
            this.Start = Start;
            this.Finish = Finish;
        }
    }
}
