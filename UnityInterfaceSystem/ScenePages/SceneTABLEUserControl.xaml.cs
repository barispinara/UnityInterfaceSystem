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
using System.Windows.Controls.Primitives;

namespace UnityInterfaceSystem.ScenePages
{
    /// <summary>
    /// SceneTABLEUserControl.xaml etkileşim mantığı
    /// </summary>
    public partial class SceneTABLEUserControl : UserControl
    {
        public SceneTaskEntity Task { get; set; }

        public Button add_test { get; set; }

        public MainWindow main_window { get; set; }

        bool is_text_changed = false;

        public SceneTABLEUserControl(SceneTaskEntity task , MainWindow main)
        {
            InitializeComponent();

            ExpanderMenu.Visibility = task.SubTaskItems == null ? Visibility.Collapsed : Visibility.Visible;
            this.Task = task;
            this.DataContext = task;
            this.main_window = main;
        }

        #region It contains operations when ExpanderMenu is expanded ----> ExpanderMenu_Expanded
        private void ExpanderMenu_Expanded(object sender, RoutedEventArgs e)
        {


            AddSubTaskBtn.Visibility = Visibility.Collapsed;
            DeleteTaskBtn.Visibility = Visibility.Collapsed;

        }
        #endregion

        #region It contains operations when ExpanderMenu is collapsed ----> ExpanderMenu_Collapsed
        private void ExpanderMenu_Collapsed(object sender, RoutedEventArgs e)
        {

            AddSubTaskBtn.Visibility = Visibility.Visible;
            DeleteTaskBtn.Visibility = Visibility.Visible;


        }
        #endregion 

        #region It contains click event of DeleteSubTaskBtn where is in SubTask rows ----> DeleteSubTaskBtn_Click
        private void DeleteSubTaskBtn_Click(object sender, RoutedEventArgs e)
        {

            Button coming_button = sender as Button;

            int? temp_index = null;

            int? task_index = null;

            for (int i = 0; i < Task.SubTaskItems.Count; i++)
            {
                if (Task.SubTaskItems[i].Name.Equals(coming_button.Tag.ToString()))
                {
                    temp_index = i;
                }
            }

            for (int i = 0; i < main_window.scene_list.Count; i++)
            {
                if (main_window.scene_list[i].Task == Task)
                {
                    task_index = i;
                }
            }

            if (temp_index != null)
            {
                ListViewMenu.ItemsSource = null;
                Task.SubTaskItems.RemoveAt((int)temp_index);
                Task = sub_task_item_number_updater(Task);
                ListViewMenu.Items.Clear();
                ListViewMenu.ItemsSource = main_window.scene_list[(int)task_index].Task.SubTaskItems;
            }
            main_window.UpdateLayout();
            ListViewMenu.Items.Refresh();
        }
        #endregion

        #region It provides a update on number and label names of sub tasks when user delete a row on scene table ----> sub_task_item_number_updater
        private SceneTaskEntity sub_task_item_number_updater(SceneTaskEntity Task)
        {
            for (int i = 0; i < Task.SubTaskItems.Count; i++)
            {
                Task.SubTaskItems[i].Number = Task.RowCounter.ToString() + "." + (i + 1).ToString();
                if (Task.SubTaskItems[i].Name.Contains("New SubTask"))
                {
                    Console.WriteLine(Task.SubTaskItems[i].Name);
                    Task.SubTaskItems[i].Name = "New SubTask " + Task.SubTaskItems[i].Number;
                }
            }

            return Task;
        }
        #endregion 

        #region  It contains KeyDown event of SceneNameTextBox (Its just for 'Enter' KeyDown)----> SceneNameTextBox_KeyDown
        private void SceneNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {

                main_window.scene_list.Find(SceneTABLEUserControl => SceneTABLEUserControl.Task.Header == Task.Header).Tag = SceneNameTextBox.Text;
                SceneNameTextBox.Tag = SceneNameTextBox.Text;
                Task.Header = SceneNameTextBox.Text;
                AddSubTaskBtn.Tag = SceneNameTextBox.Text;
                SceneNameTextBox.IsReadOnly = true;
                SceneNameTextBox.BorderBrush = Brushes.Red;


            }
        }
        #endregion

        #region It contains DoubleClick event of SceneNameTextBox ----> SceneNameTextBox_MouseDoubleClick
        private void SceneNameTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SceneNameTextBox.IsReadOnly = false;
            SceneNameTextBox.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF2196F3");
        }
        #endregion

        #region It contains click event of AddPieceSubTaskBtn ----> AddPieceSubTaskBtn_Click_1
        private void AddPieceSubTaskBtn_Click_1(object sender, RoutedEventArgs e)
        {
            if (is_text_changed)
            {
                MessageBox.Show("You didn't save name changes, please click textbox and press enter to save", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            else
            {
                Button clicked_subtask_button = sender as Button;
                main_window.ModelTAB.IsSelected = true;
                main_window.SubTaskBtn.Visibility = Visibility.Visible;
                main_window.SubTaskBtn.Tag = clicked_subtask_button.Tag;
            }
        }
        #endregion

        #region It contains KeyDown event of SubTaskTextBox (Its just for 'Enter' KeyDown)s ----> TextBox_KeyDown
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox updated_textbox_text = sender as TextBox;

            if (e.Key == Key.Return)
            {
                for (int i = 0; i < Task.SubTaskItems.Count; i++)
                {
                    if (Task.SubTaskItems[i].Name.Equals(updated_textbox_text.Tag))
                    {
                        Task.SubTaskItems[i].Name = updated_textbox_text.Text;
                    }
                }

                ListViewMenu.ItemsSource = null;
                ListViewMenu.Items.Clear();
                ListViewMenu.ItemsSource = Task.SubTaskItems;
                is_text_changed = false;

            }



        }
        #endregion

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            is_text_changed = true;
        }

        private void SceneNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            main_window.scene_list.Find(SceneTABLEUserControl => SceneTABLEUserControl.Task.Header == Task.Header).Tag = SceneNameTextBox.Text;
            SceneNameTextBox.Tag = SceneNameTextBox.Text;
            Task.Header = SceneNameTextBox.Text;
            AddSubTaskBtn.Tag = SceneNameTextBox.Text;
        }
    }
}
