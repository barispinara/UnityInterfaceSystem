using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UnityInterfaceSystem.Entities;

namespace UnityInterfaceSystem.ScenePages
{
    /// <summary>
    /// SavedSceneNewTabUserController.xaml etkileşim mantığı
    /// </summary>
    public partial class SavedSceneNewTabUserController : UserControl
    {
        public List<SavedSceneRowTABLEUserControl> new_tab_scene_list = new List<SavedSceneRowTABLEUserControl>();

        public string Model_Name { get; set; }
        public SavedSceneNewTabUserController(SavedSceneEntity saved_scene)
        {
            InitializeComponent();
            this.DataContext = saved_scene;
            ModelNameBlock.Text = saved_scene.task_list[0].SubTaskItems[0].Model_name;
            fill_scene_table(saved_scene);
        }

        #region It provides to fill operation Saved Table with data ----> fill_scene_table
        private void fill_scene_table(SavedSceneEntity saved_scene)
        {
            for (int i = 0; i < saved_scene.task_list.Count; i++)
            {
                SavedSceneRowTABLEUserControl new_task_row = new SavedSceneRowTABLEUserControl(saved_scene.task_list[i]);
                new_tab_scene_list.Add(new_task_row);

                NewTabSceneListView.ItemsSource = null;
                NewTabSceneListView.Items.Clear();
                NewTabSceneListView.ItemsSource = new_tab_scene_list;
            }
        }
        #endregion

        #region It provides to manage vertically scrolling with mouse wheel on Saved Table  ----> ScrollViewer_PreviewMouseWheel

        //
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
        #endregion 
    }
}
