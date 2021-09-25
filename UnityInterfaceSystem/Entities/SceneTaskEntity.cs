using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using UnityInterfaceSystem.Entities;

namespace UnityInterfaceSystem.Entities
{
    public class SceneTaskEntity
    {
        public string Header { get; set; }

        public int RowCounter { get; set; }
        public List<SceneSubTaskEntity> SubTaskItems { get; set; }

        public UserControl Screen { get; set; }

        public SceneTaskEntity(string header, List<SceneSubTaskEntity> stepItems, int rowCounter)
        {
            this.Header = header;
            this.SubTaskItems = stepItems;
            this.RowCounter = rowCounter;
        }

    }
}
