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
using System.Data.SqlClient;

namespace UnityInterfaceSystem.ScenePages
{
    /// <summary>
    /// AddPieceUserControl.xaml etkileşim mantığı
    /// </summary>
    public partial class AddPieceUserControl : UserControl
    {
        /*-----------------------------------
         * Variable: (const string) sql_connection
         * Function: send_To_DataBase()
         * This variable helps to make connection
         * between system and database
         * You can take a look this link for taking help
         * https://docs.microsoft.com/tr-tr/dotnet/api/system.data.sqlclient.sqlconnection?view=dotnet-plat-ext-5.0
         * ---------------------------------*/
        private const string sql_connection = "SQL CONNECTION";

        private List<ModelsEntity> model_list = new List<ModelsEntity>();

        private List<PieceEntity> piece_list = new List<PieceEntity>();

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
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        #endregion

        public AddPieceUserControl()
        {
            InitializeComponent();
            Gather_Data_From_DB();
            ModelComboBox.ItemsSource = model_list;
        }

        #region Contains Click event of BtnApply ----> BtnApply_Click
        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            if (PieceNameWarning.Visibility == Visibility.Visible)
            {
                MessageBox.Show("Piece is already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (PieceNameINPUT.Text.Length == 0)
            {
                MessageBox.Show("Piece name can not be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (StartFrameWarning.Visibility == Visibility.Visible)
            {
                MessageBox.Show("Start Frame must be numeric(integer) value", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (StartFrameINPUT.Text.Length == 0)
            {
                MessageBox.Show("Start Frame field can not be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (FinishFrameWarning.Visibility == Visibility.Visible)
            {
                MessageBox.Show("Finish Frame must be numeric(integer) value", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (FinishFrameINPUT.Text.Length == 0)
            {
                MessageBox.Show("Finish Frame field can not be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string sql = "" +
                    "INSERT INTO Pieces(Name , Model , Start , Finish)" +
                    " VALUES('" + PieceNameINPUT.Text + "', '" + ModelComboBox.SelectedValue.ToString() + "', " + int.Parse(StartFrameINPUT.Text) + ", " + int.Parse(FinishFrameINPUT.Text) + ");";
                Send_To_DataBase(sql);
            }
        }
        #endregion

        #region Contains Click event of BtnClear ----> BtnClear_Click
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ModelComboBox.Text = "";
            PieceNameINPUT.Text = "";
            StartFrameINPUT.Text = "";
            FinishFrameINPUT.Text = "";
        }
        #endregion

        #region Contains Text Changed event of PieceNameINPUT ----> PieceNameINPUT_TextChanged
        private void PieceNameINPUT_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox coming_text = sender as TextBox;

            if (piece_list.Find(PieceEntity => PieceEntity.Name == coming_text.Text.ToString()) != null)
            {
                PieceNameWarning.Visibility = Visibility.Visible;
            }
            else
            {
                PieceNameWarning.Visibility = Visibility.Hidden;
            }
        }
        #endregion

        #region Contains Text Changed event of StartFrameINPUT ----> StartFrameINPUT_TextChanged
        private void StartFrameINPUT_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value;
            TextBox coming_text = sender as TextBox;

            if (!int.TryParse(coming_text.Text.ToString(), out value))
            {
                StartFrameWarning.Visibility = Visibility.Visible;
            }
            else
            {
                StartFrameWarning.Visibility = Visibility.Hidden;
            }
        }
        #endregion

        #region Contains Text Changed event of FinishFrameINPUT ----> FinishFrameINPUT_TextChanged
        private void FinishFrameINPUT_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value;
            TextBox coming_text = sender as TextBox;

            if (!int.TryParse(coming_text.Text.ToString(), out value))
            {
                FinishFrameWarning.Visibility = Visibility.Visible;
            }
            else
            {
                FinishFrameWarning.Visibility = Visibility.Hidden;
            }

        }
        #endregion 
    }
}
