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
using MaterialDesignThemes.Wpf;
using MaterialDesignColors.Recommended;
using UnityInterfaceSystem.Entities;
using UnityInterfaceSystem.ScenePages;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Win32;
using Dragablz;

namespace UnityInterfaceSystem
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        /*-----------------------------------
         * Variable: (const string) sql_connection
         * Function: send_To_DataBase()
         * This variable helps to make connection
         * between system and database
         * You can take a look this link for taking help
         * https://docs.microsoft.com/tr-tr/dotnet/api/system.data.sqlclient.sqlconnection?view=dotnet-plat-ext-5.0
         * ---------------------------------*/

        private const string sql_connection = "SQL Connection";


        public List<ModelsEntity> model_list = new List<ModelsEntity>();

        public List<PieceEntity> piece_list = new List<PieceEntity>();

        public List<PieceEntity> piece_table_row_list = new List<PieceEntity>();


        public List<SceneTABLEUserControl> scene_list = new List<SceneTABLEUserControl>();

        public List<SavedSceneEntity> saved_scene_list = new List<SavedSceneEntity>();

        private int Piece_Table_ID_Counter = 1;

        #region Gathering data part from database ----> Gather_Data_From_DB
        /*----------------------------------------------
         * Gather data from Database and adding into listes
         * {model_list , piece_list , req_list}
         * ---------------------------------------------------*/

        private void Gather_Data_From_DB()
        {

            //Gather Models Data
            try
            {
                SqlConnection connection = new SqlConnection(sql_connection);
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM dbo.Models", connection);
                SqlDataReader reader = command.ExecuteReader();
                model_list.Clear();
                while (reader.Read())
                {
                    ModelsEntity model = new ModelsEntity((string)reader["Name"], (string)reader["Directory"]);
                    model_list.Add(model);
                }
                connection.Close();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }

            //Gather Pieces Data
            try
            {
                SqlConnection connection = new SqlConnection(sql_connection);
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM dbo.Pieces", connection);
                SqlDataReader reader = command.ExecuteReader();
                piece_list.Clear();
                while (reader.Read())
                {
                    PieceEntity piece = new PieceEntity(Piece_Table_ID_Counter, (string)reader["Name"], (string)reader["Model"], (int)reader["Start"], (int)reader["Finish"]);
                    Piece_Table_ID_Counter++;
                    piece_list.Add(piece);
                }
                connection.Close();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }

            //Gather Saved Scene Data
            try
            {
                SqlConnection connection = new SqlConnection(sql_connection);
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM dbo.Scene", connection);
                SqlDataReader reader = command.ExecuteReader();
                saved_scene_list.Clear();
                int counter = 0;
                string scene_name = "";
                while (reader.Read())
                {
                    if (saved_scene_list.Count == 0)
                    {
                        List<SceneSubTaskEntity> saved_sub_task_list = new List<SceneSubTaskEntity>();
                        List<SceneTaskEntity> saved_task_list = new List<SceneTaskEntity>();
                        SavedSceneEntityForDB saved_scene_for_db = new SavedSceneEntityForDB((string)reader["Scene_Name"], (string)reader["Task_Name"], (string)reader["ID"]);
                        SceneTaskEntity saved_task = new SceneTaskEntity(saved_scene_for_db.Task_Name, saved_sub_task_list, int.Parse(saved_scene_for_db.ID));
                        saved_task_list.Add(saved_task);
                        scene_name = saved_scene_for_db.Scene_Name;
                        SavedSceneEntity saved_scene = new SavedSceneEntity(saved_scene_for_db.Scene_Name, saved_task_list);
                        saved_scene_list.Add(saved_scene);
                    }
                    else if (scene_name.Equals((string)reader["Scene_Name"]))
                    {
                        if (Convert.IsDBNull(reader["Subtask_Name"]))
                        {
                            List<SceneSubTaskEntity> saved_sub_task_list = new List<SceneSubTaskEntity>();
                            SavedSceneEntityForDB saved_scene_for_db = new SavedSceneEntityForDB((string)reader["Scene_Name"], (string)reader["Task_Name"], (string)reader["ID"]);
                            SceneTaskEntity saved_task = new SceneTaskEntity(saved_scene_for_db.Task_Name, saved_sub_task_list, int.Parse(saved_scene_for_db.ID));
                            saved_scene_list[counter].task_list.Add(saved_task);

                        }
                        else
                        {
                            SavedSceneEntityForDB saved_scene_for_db = new SavedSceneEntityForDB((string)reader["Scene_Name"], (string)reader["Task_Name"], Convert.ToString(reader["Subtask_Name"]), Convert.ToString(reader["ID"]), Convert.ToString(reader["Model_Name"]), Convert.ToString(reader["Piece_Name"]));
                            for (int i = 0; i < saved_scene_list[counter].task_list.Count; i++)
                            {
                                if (saved_scene_list[counter].task_list[i].Header.Equals((string)reader["Task_Name"]))
                                {
                                    SceneSubTaskEntity saved_subtask = new SceneSubTaskEntity(saved_scene_for_db.Subtask_Name, saved_scene_for_db.Model_Name, saved_scene_for_db.ID, saved_scene_for_db.Piece_Name);
                                    saved_scene_list[counter].task_list[i].SubTaskItems.Add(saved_subtask);
                                }
                            }
                        }
                    }
                    else if (!scene_name.Equals((string)reader["Scene_Name"]))
                    {
                        counter++;
                        List<SceneSubTaskEntity> saved_sub_task_list = new List<SceneSubTaskEntity>();
                        List<SceneTaskEntity> saved_task_list = new List<SceneTaskEntity>();
                        SavedSceneEntityForDB saved_scene_for_db = new SavedSceneEntityForDB((string)reader["Scene_Name"], (string)reader["Task_Name"], (string)reader["ID"]);
                        SceneTaskEntity saved_task = new SceneTaskEntity(saved_scene_for_db.Task_Name, saved_sub_task_list, int.Parse(saved_scene_for_db.ID));
                        saved_task_list.Add(saved_task);
                        scene_name = saved_scene_for_db.Scene_Name;
                        SavedSceneEntity saved_scene = new SavedSceneEntity(saved_scene_for_db.Scene_Name, saved_task_list);
                        saved_scene_list.Add(saved_scene);
                    }

                }
                connection.Close();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }


        }
        #endregion

        #region Send Query To Database Part (Single Query) ----> Send_To_DataBase
        private void Send_To_DataBase(string sql_query)
        {
            try
            {
                SqlConnection connection = new SqlConnection(sql_connection);
                connection.Open();
                SqlCommand command = new SqlCommand(sql_query, connection);
                command.ExecuteNonQuery();
                connection.Close();
                if (sql_query.Contains("INSERT"))

                    MessageBox.Show("Data inserted successfully");

                else if (sql_query.Contains("DELETE"))

                    MessageBox.Show("Data deleted successfully");
                else if (sql_query.Contains("DROP"))
                    MessageBox.Show("Data deleted successfully");
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        #endregion

        #region Send Query To Database Part (Multiple Query) ----> Send_To_DataBase
        private void Send_To_DataBase(List<string> sql_query_list)
        {
            try
            {
                SqlConnection connection = new SqlConnection(sql_connection);
                for (int i = 0; i < sql_query_list.Count; i++)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql_query_list[i], connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                if (sql_query_list[0].Contains("INSERT"))
                {
                    MessageBox.Show("Data inserted successfully");
                }
                else if (sql_query_list[0].Contains("DELETE"))

                    MessageBox.Show("Data deleted successfully");

            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        #endregion
        public MainWindow()
        {
            Gather_Data_From_DB();
            InitializeComponent();
            Fill_Model_Table(model_list);
            PieceTable.DataContext = piece_table_row_list;
            Fill_Saved_Scene_Table(saved_scene_list);

        }

        #region It provides fill operation for Saved Scene Table with data ----> Fill_Saved_Scene_Table
        private void Fill_Saved_Scene_Table(List<SavedSceneEntity> saved_list)
        {
            SceneSavedListView.Items.Clear();
            for (int i = 0; i < saved_list.Count; i++)
            {
                DockPanel dp = new DockPanel();
                SavedSceneUserControl new_saved_scene_tab = new SavedSceneUserControl(saved_list[i]);
                new_saved_scene_tab.DeleteSelectedSceneBtn.Click += new RoutedEventHandler(DeleteSelectScene_Click);
                new_saved_scene_tab.LoadSelectedSceneBtn.Click += new RoutedEventHandler(LoadSelectedScene_Click);
                new_saved_scene_tab.ShowSelectedSceneBtn.Click += new RoutedEventHandler(ShowSelectedSceneBtn_Click);
                dp.Children.Add(new_saved_scene_tab);
                dp.LastChildFill = true;
                dp.Margin = new Thickness(0, 5, 0, 0);
                SceneSavedListView.Items.Add(dp);
            }
        }
        #endregion 

        #region It provides fill operation for Model Table with datas ----> Fill_Model_Table
        private void Fill_Model_Table(List<ModelsEntity> method_model_list)
        {
            ModelListView.Items.Clear();
            for (int i = 0; i < method_model_list.Count; i++)
            {
                DockPanel dp = new DockPanel();
                ModelTABLEUserControl new_model_tab = new ModelTABLEUserControl(method_model_list[i]);
                new_model_tab.ModelTableShowPieceBtn.Click += new RoutedEventHandler(ModelTableShowPieceBtn_Click);
                new_model_tab.ModelTableDeleteBtn.Click += new RoutedEventHandler(ModelTableDeleteBtn_Click);
                new_model_tab.ModelTableEditBtn.Click += new RoutedEventHandler(ModelTableEditBtn_Click);
                dp.Children.Add(new_model_tab);
                dp.LastChildFill = true;
                dp.Margin = new Thickness(0, 5, 0, 0);
                ModelListView.Items.Add(dp);
            }
        }
        #endregion

        #region It contains click event of BtnMinimize ----> BtnMinimize_Click
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        #endregion

        #region It provides to move of desktop application with mouse cursor ----> Window_MouseDown
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        #endregion

        #region It contains click event of BtnFullScreen ----> BtnFullScreen_Click
        private void BtnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState.Equals(WindowState.Maximized))
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }
        #endregion

        #region It contains click event of BtnClose ----> BtnClose_Click
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {

            Console.WriteLine(TabMENU.Items[0]);
            this.Close();
        }
        #endregion

        #region It provides transition between side bar pages ----This method is currently not active due to main page design is not completed---- ----> ListView_SelectionChanged
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewSideMenu.SelectedIndex;
            MoveCursorMenu(index);

            switch (index)
            {
                case 0:
                    if (HomePAGE != null && SceneTabPAGE != null)
                    {
                        HomePAGE.Visibility = Visibility.Collapsed;
                        SceneTabPAGE.Visibility = Visibility.Visible;
                    }
                    break;
                case 1:
                    if (HomePAGE != null && SceneTabPAGE != null)
                    {

                        SceneTabPAGE.Visibility = Visibility.Visible;

                    }
                    break;
                default:
                    break;

            }

        }
        #endregion

        #region It provides location calculation of little blue rectangle which is in side bar ----> MoveCursorMenu
        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (100 + (60 * index)), 0, 0);
        }
        #endregion

        #region It provides to add hover effect on BtnMinimize  ----> BtnMinimize_MouseEnter , BtnMinimize_MouseLeave
        private void BtnMinimize_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnMinimize.Background = new SolidColorBrush(Color.FromRgb(57, 62, 70));
        }

        private void BtnMinimize_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnMinimize.Background = Brushes.Transparent;
        }
        #endregion

        #region It provides to add hover effect on BtnFullScreen ----> BtnFullScreen_MouseEnter , BtnFullScreen_MouseLeave
        private void BtnFullScreen_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnFullScreen.Background = new SolidColorBrush(Color.FromRgb(57, 62, 70));
        }

        private void BtnFullScreen_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnFullScreen.Background = Brushes.Transparent;
        }
        #endregion

        #region It provides to add hover effect on BtnClose ----> BtnClose_MouseEnter , BtnClose_MouseLeave
        private void BtnClose_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnClose.Background = Brushes.Red;
        }

        private void BtnClose_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnClose.Background = Brushes.Transparent;
        }
        #endregion 

        #region It contains click event of BtnAddScene ----> BtnAddScene_Click
        private void BtnAddScene_Click(object sender, RoutedEventArgs e)
        {

            List<SceneSubTaskEntity> subtask_list = new List<SceneSubTaskEntity>();
            SceneTaskEntity created_task = new SceneTaskEntity("New Task " + (scene_list.Count + 1), subtask_list, scene_list.Count + 1);
            SceneTABLEUserControl new_scene_tab = new SceneTABLEUserControl(created_task, this);
            new_scene_tab.DeleteTaskBtn.Click += new RoutedEventHandler(DeleteTaskBtn_Click);
            new_scene_tab.AddSubTaskBtn.Click += new RoutedEventHandler(AddSubTaskBtn_Click);

            new_scene_tab.Tag = created_task.Header;
            scene_list.Add(new_scene_tab);
            SceneListView.ItemsSource = null;
            SceneListView.Items.Clear();
            SceneListView.ItemsSource = scene_list;
        }
        #endregion 

        #region It provides to use scrollbars with mouse wheel ----> ScrollViewer_PreviewMouseWheel
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
        #endregion

        #region It contains click event of SceneSearchBtn  ----> SceneSearchBtn_Click
        private void SceneSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            List<SavedSceneEntity> filtered_new_saved_scene_list = new List<SavedSceneEntity>();
            if (SceneSearchBAR.Text.Equals("Search Saved Scene") || SceneSearchBAR.Text.Equals(""))
            {
                MessageBox.Show("You must give a name for searching", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                for (int i = 0; i < saved_scene_list.Count; i++)
                {
                    if (saved_scene_list[i].Scene_Name.Contains(SceneSearchBAR.Text))
                    {
                        filtered_new_saved_scene_list.Add(saved_scene_list[i]);
                    }
                }
                Fill_Saved_Scene_Table(filtered_new_saved_scene_list);
            }
        }
        #endregion 

        #region It contains click event of SceneSaveBtn ----> SceneSaveBtn_Click
        private void SceneSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SceneSaveBar.Text.Equals("Give name to scene") || SceneSaveBar.Text.Equals(""))
            {
                MessageBox.Show("You must give a name to scene before save", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (scene_list.Count == 0)
            {
                MessageBox.Show("Scene table is empty", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            for (int i = 0; i < scene_list.Count; i++)
            {
                if (scene_list[i].Task.SubTaskItems.Count == 0)
                {
                    MessageBox.Show(scene_list[i].Task.Header + " doesn't have a subtask please fill or delete it", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    for (int j = 0; j < scene_list[i].Task.SubTaskItems.Count; j++)
                    {
                        if (scene_list[i].Task.SubTaskItems[j].Model_name == null)
                        {
                            MessageBox.Show(scene_list[i].Task.SubTaskItems[j].Name + " doesn't have a piece please fill or delete it", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
            }
            for (int x = 0; x < saved_scene_list.Count; x++)
            {
                if (saved_scene_list[x].Scene_Name.Equals(SceneSaveBar.Text))
                {
                    MessageBox.Show(SceneSaveBar.Text + " is already exist in saved scene list please give another name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            List<string> sql_list = new List<string>();

            for (int i = 0; i < scene_list.Count; i++)
            {
                string sql = "INSERT INTO Scene(Scene_Name , Task_Name , ID)" +
                    " VALUES('" + SceneSaveBar.Text + "', '" + scene_list[i].Task.Header + "', '" + scene_list[i].Task.RowCounter + "');";
                sql_list.Add(sql);
                for (int j = 0; j < scene_list[i].Task.SubTaskItems.Count; j++)
                {
                    string sql1 = "INSERT INTO Scene(Scene_Name , Task_Name , Subtask_Name , ID , Model_Name , Piece_Name)" +
                        " VALUES('" + SceneSaveBar.Text + "', '" + scene_list[i].Task.Header + "', '" + scene_list[i].Task.SubTaskItems[j].Name + "', '" + scene_list[i].Task.SubTaskItems[j].Number + "' , '" +
                        scene_list[i].Task.SubTaskItems[j].Model_name + "', '" + scene_list[i].Task.SubTaskItems[j].piece_name + "');";
                    sql_list.Add(sql1);

                }


            }
            Send_To_DataBase(sql_list);
            Gather_Data_From_DB();
            Fill_Saved_Scene_Table(saved_scene_list);
            scene_list.Clear();
            SceneListView.ItemsSource = null;
            SceneListView.Items.Clear();
        }
        #endregion 

        #region  It contains click event of SendUnityBtn ----> SendUnityBtn_Click / Send_Files_To_Unity_Resources
        private void SendUnityBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SceneListView.Items.Count == 0)
            {
                MessageBox.Show("Scene List is empty please fill before send unity", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            for (int i = 0; i < scene_list.Count; i++)
            {
                if (scene_list[i].Task.SubTaskItems.Count == 0)
                {
                    MessageBox.Show(scene_list[i].Task.Header + " doesn't have a subtask please fill or delete it", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    for (int j = 0; j < scene_list[i].Task.SubTaskItems.Count; j++)
                    {
                        if (scene_list[i].Task.SubTaskItems[j].Model_name == null)
                        {
                            MessageBox.Show(scene_list[i].Task.SubTaskItems[j].Name + " doesn't have a piece please fill or delete it", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
            }
            TextWriter txt = new StreamWriter(Directory.GetCurrentDirectory() + "\\UnitySender.txt");
            txt.Write("Models");
            txt.Write(Environment.NewLine);

            ModelsEntity selected_model = null;
            for (int i = 0; i < model_list.Count; i++)
            {
                if (model_list[i].Name.Equals(scene_list[0].Task.SubTaskItems[0].Model_name))
                {
                    selected_model = model_list[i];
                }
            }
            if (selected_model != null)
            {
                txt.Write("Model|" + selected_model.Name + "|" + selected_model.Directory);
                txt.Write(Environment.NewLine);
                Send_Files_To_Unity_Resources(selected_model.Name, selected_model.Directory);
            }
            txt.Write("Scene");
            txt.Write(Environment.NewLine);
            for (int i = 0; i < scene_list.Count; i++)
            {
                txt.Write("Task|" + scene_list[i].Task.RowCounter + "|" + scene_list[i].Task.Header);
                txt.Write(Environment.NewLine);
                for (int x = 0; x < scene_list[i].Task.SubTaskItems.Count; x++)
                {
                    txt.Write("SubTask|" + scene_list[i].Task.SubTaskItems[x].Name + "|" + scene_list[i].Task.SubTaskItems[x].piece_name + "|" + scene_list[i].Task.SubTaskItems[x].Model_name + "|" + scene_list[i].Task.SubTaskItems[x].start.ToString() + "|" + scene_list[i].Task.SubTaskItems[x].finish.ToString());
                    txt.Write(Environment.NewLine);
                }
                txt.Write("-----------------------------------------------");
                txt.Write(Environment.NewLine);
            }
            txt.Close();


            MessageBox.Show("Scene Demo is sended to Unity successfully, Unity Project Name: Created Scene", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Send_Files_To_Unity_Resources(string model_name, string directory)
        {
            if (!directory.Equals(""))
            {
                string current_path = Directory.GetCurrentDirectory();
                string[] current_path_splitter = current_path.Split('\\');

                string target_directory = current_path_splitter[0] + "\\" + current_path_splitter[1] + "\\" + current_path_splitter[2] + "\\CreatedScene\\Assets\\Resources\\" + model_name;
                Console.WriteLine(target_directory);
                string animation_target_directory = target_directory + "\\Animasyon";
                Directory.CreateDirectory(target_directory);
                Directory.CreateDirectory(animation_target_directory);
                string target_file = target_directory + "\\" + model_name + ".blend";
                File.Copy(directory, target_file, true);
            }
        }
        #endregion  

        #region It provides to add hover effect on SendUnityBtn ----> SendUnityBtn_MouseEnter , SendUnityBtn_MouseLeave
        private void SendUnityBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            SendUnityBtn.Background = Brushes.Green;
        }

        private void SendUnityBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            SendUnityBtn.Background = new SolidColorBrush(Color.FromRgb(34, 40, 49));
        }
        #endregion

        #region It contains click event of ModelTableSearchBtn ----> ModelTableSearchBtn_Click
        private void ModelTableSearchBtn_Click(object sender, RoutedEventArgs e)
        {

            if (ModelTableSearchBAR.Text.ToLower().Equals("search model"))
            {
                MessageBox.Show("Aramak İstediğiniz Modeli Yazın", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                List<ModelsEntity> filtered_model_list = model_list.Where((row) =>
                {
                    return
                         (row.Name.ToString().ToLower().Contains(ModelTableSearchBAR.Text.ToLower()));

                }).ToList();
                Fill_Model_Table(filtered_model_list);
            }

        }
        #endregion

        #region It contains click event of ModelTableShowPieceBtns  ----> ModelTableShowPieceBtn_Click
        private void ModelTableShowPieceBtn_Click(object sender, RoutedEventArgs e)
        {
            Button coming_show_button = sender as Button;
            List<PieceEntity> filtered_piece_list = piece_list.Where((row) =>
            {
                return
                     (row.Model.ToString().Contains(coming_show_button.Tag.ToString()));
            }).ToList();

            Gather_Data_From_DB();
            piece_table_row_list.Clear();
            piece_table_row_list = filtered_piece_list;
            PieceTable.ItemsSource = null;
            PieceTable.Items.Clear();
            Piece_Table_ID_Counter = 1;
            PieceTable.ItemsSource = piece_table_row_list;
            PieceTable.Items.Refresh();
            PieceTable.UpdateLayout();
        }
        #endregion

        #region It contains click event of ModelTableDeleteBtn ----> ModelTableDeleteBtn_Click
        private void ModelTableDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Button model_delete_button = sender as Button;
            String sql = "DELETE FROM Models Where Name = '" + model_delete_button.Tag + "'";
            Send_To_DataBase(sql);
            Gather_Data_From_DB();
            Fill_Model_Table(model_list);
        }
        #endregion

        #region It contains click event of ModelTableEditBtn ----> ModelTableEditBtn_Click
        private void ModelTableEditBtn_Click(object sender, RoutedEventArgs e)
        {
        }
        #endregion

        #region It contains click event of ModelTableAddBtn ----> ModelTableAddBtn_Click
        private void ModelTableAddBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file_explorer = new OpenFileDialog
            {
                DefaultExt = ".fbx",
                Filter = "FBX Files(*.fbx)|*.fbx|Blend Files(*.blend)|*.blend"
            };

            Nullable<bool> result = file_explorer.ShowDialog();

            if (result == true)
            {
                string[] splitter = file_explorer.SafeFileName.Split('.');
                string added_model_name = splitter[0];
                string added_model_path = file_explorer.FileName;
                String sql = "INSERT INTO Models(Name , Directory)" +
                    " VALUES('" + added_model_name + "', '" + added_model_path + "');";
                Send_To_DataBase(sql);
                Gather_Data_From_DB();
                Fill_Model_Table(model_list);
            }

        }
        #endregion

        #region It contains click event of PieceTableAddBtn ----> PieceTableAddBtn_Click
        private void PieceTableAddBtn_Click(object sender, RoutedEventArgs e)
        {
            TabItem createdTab = new TabItem();
            HeaderWithCloseViewModel optinal_close_button_header = new HeaderWithCloseViewModel("Add Piece");
            createdTab.Header = new ClosableHeaderTabUserControl(optinal_close_button_header);
            createdTab.Content = new AddPieceUserControl();

            TabMENU.Items.Add(createdTab);
            createdTab.IsSelected = true;
        }
        #endregion

        #region It contains click event of PieceTableSearchBarBtn ----> PieceTableSearchBarBtn_Click
        private void PieceTableSearchBarBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PieceTableSearchBAR.Text.ToLower().Equals("search piece"))
            {
                MessageBox.Show("Aramak İstediğiniz Parçayı Yazın", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                List<PieceEntity> filtered_piece_list = piece_list.Where((row) =>
                {
                    return
                         (row.Name.ToString().ToLower().Contains(PieceTableSearchBAR.Text.ToLower()));

                }).ToList();

                piece_table_row_list.Clear();
                piece_table_row_list = filtered_piece_list;
                PieceTable.ItemsSource = piece_table_row_list;
                PieceTable.Items.Refresh();
                PieceTable.UpdateLayout();
            }
        }
        #endregion 

        #region It contains click event of PieceTableInfoBtn ----> PieceTableInfoBtn_Click
        private void PieceTableInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            Button coming_button = sender as Button;

            TabItem createdTab = new TabItem();
            HeaderWithCloseViewModel optional_close_button_header = new HeaderWithCloseViewModel(coming_button.Tag.ToString());
            createdTab.Header = new ClosableHeaderTabUserControl(optional_close_button_header);
            createdTab.Content = new RequirementPAGEUserControl(piece_list.Find(PieceEntity => PieceEntity.Name == coming_button.Tag.ToString()));

            TabMENU.Items.Add(createdTab);
            createdTab.IsSelected = true;
        }
        #endregion

        #region It contains click event of DeleteTaskBtn and update method ----> DeleteTaskBtn_Click / task_number_updater
        private void DeleteTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            Button coming_button = sender as Button;
            int? temp_index = null;

            for (int i = 0; i < scene_list.Count; i++)
            {

                if (scene_list[i].Tag.Equals(coming_button.Tag))
                {
                    temp_index = i;
                }
            }

            if (temp_index != null)
            {
                scene_list.RemoveAt((int)temp_index);
                scene_list = task_number_updater(scene_list);
                SceneListView.ItemsSource = null;
                SceneListView.Items.Clear();
                SceneListView.ItemsSource = scene_list;
                SceneListView.Items.Refresh();

                for (int i = 0; i < scene_list.Count; i++)
                {
                    scene_list[i].ListViewMenu.ItemsSource = null;
                    scene_list[i].ListViewMenu.Items.Clear();
                    scene_list[i].ListViewMenu.ItemsSource = scene_list[i].Task.SubTaskItems;
                    scene_list[i].ListViewMenu.Items.Refresh();
                }
            }



        }
        //----------------------------------------------------------
        //task_number_updater provides to update operation for number and label texts on tasks
        //-------------------------------------------------------------
        private List<SceneTABLEUserControl> task_number_updater(List<SceneTABLEUserControl> SCENE_list)
        {
            for (int i = 0; i < SCENE_list.Count; i++)
            {
                SCENE_list[i].Task.RowCounter = (i + 1);
                SCENE_list[i].RowCountField.Text = SCENE_list[i].Task.RowCounter.ToString();

                if (SCENE_list[i].Task.Header.Contains("New Task"))
                {
                    SCENE_list[i].Task.Header = "New Task " + SCENE_list[i].Task.RowCounter;
                    SCENE_list[i].SceneNameTextBox.Text = SCENE_list[i].Task.Header;
                }

                for (int x = 0; x < SCENE_list[i].Task.SubTaskItems.Count; x++)
                {
                    SCENE_list[i].Task.SubTaskItems[x].Number = SCENE_list[i].Task.RowCounter.ToString() + "." + (x + 1).ToString();
                    if (SCENE_list[i].Task.SubTaskItems[x].Name.Contains("New SubTask"))
                    {
                        SCENE_list[i].Task.SubTaskItems[x].Name = "New SubTask " + SCENE_list[i].Task.SubTaskItems[x].Number;
                    }

                }
            }

            return SCENE_list;
        }
        #endregion

        #region It contains click event of AddSubTaskBtn ----> AddSubTaskBtn_Click
        private void AddSubTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            Button coming_button = sender as Button;

            int? temp_index = null;

            for (int i = 0; i < scene_list.Count; i++)
            {

                if (scene_list[i].Tag.Equals(coming_button.Tag))
                {
                    temp_index = i;
                }
            }

            if (temp_index != null)
            {

                scene_list[(int)temp_index].Task.SubTaskItems.Add(new SceneSubTaskEntity("New SubTask " + (scene_list[(int)temp_index].Task.RowCounter) + "." + (scene_list[(int)temp_index].Task.SubTaskItems.Count + 1).ToString(), ((scene_list[(int)temp_index].Task.RowCounter) + "." + (scene_list[(int)temp_index].Task.SubTaskItems.Count + 1).ToString())));

                SceneListView.ItemsSource = null;
                SceneListView.Items.Clear();
                SceneListView.ItemsSource = scene_list;

                scene_list[(int)temp_index].ListViewMenu.ItemsSource = null;
                scene_list[(int)temp_index].ListViewMenu.Items.Refresh();
                scene_list[(int)temp_index].ListViewMenu.ItemsSource = scene_list[(int)temp_index].Task.SubTaskItems;

                SceneListView.Items.Refresh();
            }
        }


        #endregion

        #region It contains click event of SubTaskBtn  ----> SubTaskBtn_Click
        private void SubTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            Button clicked_button = sender as Button;
            PieceEntity added_subtask = PieceTable.SelectedValue as PieceEntity;
            if (PieceTable.SelectedIndex == -1)
            {
                MessageBox.Show("You have to choose 1 piece", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {

                for (int i = 0; i < scene_list.Count; i++)
                {
                    string temp_model_name = "a";
                    for (int x = 0; x < scene_list[i].Task.SubTaskItems.Count; x++)
                    {
                        if (scene_list[i].Task.SubTaskItems[x].Model_name != null)
                        {
                            temp_model_name = scene_list[i].Task.SubTaskItems[x].Model_name;
                        }
                    }
                    if (temp_model_name == "a")
                    {
                        if (scene_list[i].Task.SubTaskItems.Find(SceneSubTaskEntity => SceneSubTaskEntity.Name == clicked_button.Tag.ToString()) != null)
                        {
                            scene_list[i].Task.SubTaskItems.Find(SceneSubTaskEntity => SceneSubTaskEntity.Name == clicked_button.Tag.ToString()).Model_name = "";
                            scene_list[i].Task.SubTaskItems.Find(SceneSubTaskEntity => SceneSubTaskEntity.Name == clicked_button.Tag.ToString()).Model_name = added_subtask.Model;
                            scene_list[i].Task.SubTaskItems.Find(SceneSubTaskEntity => SceneSubTaskEntity.Name == clicked_button.Tag.ToString()).piece_name = added_subtask.Name;
                            scene_list[i].Task.SubTaskItems.Find(SceneSubTaskEntity => SceneSubTaskEntity.Name == clicked_button.Tag.ToString()).start = added_subtask.Start;
                            scene_list[i].Task.SubTaskItems.Find(SceneSubTaskEntity => SceneSubTaskEntity.Name == clicked_button.Tag.ToString()).finish = added_subtask.Finish;
                            scene_list[i].Task.SubTaskItems.Find(SceneSubTaskEntity => SceneSubTaskEntity.Name == clicked_button.Tag.ToString()).color = "#00ADB5";
                            scene_list[i].ListViewMenu.ItemsSource = null;
                            scene_list[i].ListViewMenu.Items.Clear();
                            scene_list[i].ListViewMenu.ItemsSource = scene_list[i].Task.SubTaskItems;
                            scene_list[i].ListViewMenu.Items.Refresh();
                            MessageBox.Show(added_subtask.Name + " Successfully added in to " + clicked_button.Tag, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            SubTaskBtn.Visibility = Visibility.Hidden;
                        }
                        else
                            continue;
                    }
                    else if (temp_model_name != "a")
                    {
                        if (!temp_model_name.Equals(added_subtask.Model))
                        {
                            MessageBox.Show("You have to choose this " + temp_model_name + " model pieces for this scene", "ERROR", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }
                        else
                        {
                            if (scene_list[i].Task.SubTaskItems.Find(SceneSubTaskEntity => SceneSubTaskEntity.Name == clicked_button.Tag.ToString()) != null)
                            {
                                scene_list[i].Task.SubTaskItems.Find(SceneSubTaskEntity => SceneSubTaskEntity.Name == clicked_button.Tag.ToString()).Model_name = "";
                                scene_list[i].Task.SubTaskItems.Find(SceneSubTaskEntity => SceneSubTaskEntity.Name == clicked_button.Tag.ToString()).Model_name = added_subtask.Model;
                                scene_list[i].Task.SubTaskItems.Find(SceneSubTaskEntity => SceneSubTaskEntity.Name == clicked_button.Tag.ToString()).piece_name = added_subtask.Name;
                                scene_list[i].Task.SubTaskItems.Find(SceneSubTaskEntity => SceneSubTaskEntity.Name == clicked_button.Tag.ToString()).start = added_subtask.Start;
                                scene_list[i].Task.SubTaskItems.Find(SceneSubTaskEntity => SceneSubTaskEntity.Name == clicked_button.Tag.ToString()).finish = added_subtask.Finish;
                                scene_list[i].Task.SubTaskItems.Find(SceneSubTaskEntity => SceneSubTaskEntity.Name == clicked_button.Tag.ToString()).color = "#00ADB5";
                                scene_list[i].ListViewMenu.ItemsSource = null;
                                scene_list[i].ListViewMenu.Items.Clear();
                                scene_list[i].ListViewMenu.ItemsSource = scene_list[i].Task.SubTaskItems;
                                scene_list[i].ListViewMenu.Items.Refresh();
                                MessageBox.Show(added_subtask.Name + " Successfully added in to " + clicked_button.Tag, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                                SubTaskBtn.Visibility = Visibility.Hidden;
                            }
                            else
                                continue;
                        }
                    }

                }
                SceneTAB.IsSelected = true;
            }

        }
        #endregion

        #region It contains click event of ShowSelectedSceneBtn  ----> ShowSelectedSceneBtn_Click
        private void ShowSelectedSceneBtn_Click(object sender, RoutedEventArgs e)
        {
            Gather_Data_From_DB();
            Button coming_button = sender as Button;
            TabItem createdTab = new TabItem();
            HeaderWithCloseViewModel optional_close_button_header = new HeaderWithCloseViewModel(coming_button.Tag.ToString());
            createdTab.Header = new ClosableHeaderTabUserControl(optional_close_button_header);
            createdTab.Content = new SavedSceneNewTabUserController(saved_scene_list.Find(SavedSceneEntity => SavedSceneEntity.Scene_Name == coming_button.Tag.ToString()));

            TabMENU.Items.Add(createdTab);
            createdTab.IsSelected = true;
        }
        #endregion

        #region It contains click event of LoadSelectedScene  ----> LoadSelectedScene_Click
        private void LoadSelectedScene_Click(object sender, RoutedEventArgs e)
        {
            Gather_Data_From_DB();
            if (scene_list.Count != 0)
            {
                if (MessageBox.Show("Unsaved changes will be deleted, Are you sure to load", "Error", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Button coming_button = sender as Button;
                    SavedSceneEntity saved_scene = saved_scene_list.Find(SavedSceneEntity => SavedSceneEntity.Scene_Name == coming_button.Tag.ToString());
                    scene_list.Clear();
                    for (int i = 0; i < saved_scene.task_list.Count; i++)
                    {
                        SceneTABLEUserControl new_scene_row = new SceneTABLEUserControl(saved_scene.task_list[i], this);
                        new_scene_row.DeleteTaskBtn.Click += new RoutedEventHandler(DeleteTaskBtn_Click);
                        new_scene_row.AddSubTaskBtn.Click += new RoutedEventHandler(AddSubTaskBtn_Click);

                        new_scene_row.Tag = saved_scene.task_list[i].Header;
                        scene_list.Add(new_scene_row);
                    }

                    SceneListView.ItemsSource = null;
                    SceneListView.Items.Clear();
                    SceneListView.ItemsSource = scene_list;
                }
            }
            else
            {
                Button coming_button = sender as Button;
                SavedSceneEntity saved_scene = saved_scene_list.Find(SavedSceneEntity => SavedSceneEntity.Scene_Name == coming_button.Tag.ToString());
                scene_list.Clear();
                for (int i = 0; i < saved_scene.task_list.Count; i++)
                {
                    SceneTABLEUserControl new_scene_row = new SceneTABLEUserControl(saved_scene.task_list[i], this);
                    new_scene_row.DeleteTaskBtn.Click += new RoutedEventHandler(DeleteTaskBtn_Click);
                    new_scene_row.AddSubTaskBtn.Click += new RoutedEventHandler(AddSubTaskBtn_Click);

                    new_scene_row.Tag = saved_scene.task_list[i].Header;
                    scene_list.Add(new_scene_row);
                }

                SceneListView.ItemsSource = null;
                SceneListView.Items.Clear();
                SceneListView.ItemsSource = scene_list;
            }
        }
        #endregion

        #region It contains click event of DeleteSelectScene ----> DeleteSelectScene_Click
        private void DeleteSelectScene_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("This is a permanent delete, are you sure to delete", "Error", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Button coming_button = sender as Button;
                string sql = "DELETE FROM Scene WHERE Scene_Name = '" + coming_button.Tag.ToString() + "' ;";
                Send_To_DataBase(sql);
                Gather_Data_From_DB();
                Fill_Saved_Scene_Table(saved_scene_list);
            }
        }
        #endregion

        #region It contains click event of ClearSceneListBtn ----> ClearSceneListBtn_Click
        private void ClearSceneListBtn_Click(object sender, RoutedEventArgs e)
        {
            scene_list.Clear();
            SceneListView.ItemsSource = null;
            SceneListView.Items.Clear();
        }
        #endregion 

        private void SceneListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

    }
}
