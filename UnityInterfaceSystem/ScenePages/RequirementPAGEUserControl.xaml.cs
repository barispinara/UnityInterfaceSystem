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
    /// RequirementPAGEUserControl.xaml etkileşim mantığı
    /// </summary>
    public partial class RequirementPAGEUserControl : UserControl
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

        private readonly List<PieceEntity> piece_list = new List<PieceEntity>();

        private readonly List<ReqEntity> req_list = new List<ReqEntity>();

        private int Req_Table_ID_Counter = 1;

        #region Gathering data part from database ----> Gather_Data_From_DB
        /*----------------------------------------------
         * Gather data from Database and adding into listes
         * {model_list , piece_list , req_list}
         * ---------------------------------------------------*/

        private void Gather_Data_From_DB()
        {


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
                    PieceEntity piece = new PieceEntity(Req_Table_ID_Counter, (string)reader["Name"], (string)reader["Model"], (int)reader["Start"], (int)reader["Finish"]);
                    Req_Table_ID_Counter++;
                    piece_list.Add(piece);
                }
                connection.Close();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }

            //Gather Requirement Data
            try
            {
                SqlConnection connection = new SqlConnection(sql_connection);
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM dbo.Requirements", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    ReqEntity req = new ReqEntity(Req_Table_ID_Counter, (string)reader["Main_Piece"], (string)reader["Req_Piece"]);
                    Req_Table_ID_Counter++;
                    req_list.Add(req);
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

        public RequirementPAGEUserControl(PieceEntity piece)
        {
            InitializeComponent();
            this.DataContext = piece;
            Gather_Data_From_DB();
            ReqListViewFiller(piece);
        }

        #region It provides to manage verticall scrolling with mouse wheel on ReqLists ----> ScrollViewer_PreviewMouseWheel
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
        #endregion

        #region Filling data on ReqList ----> ReqListViewFiller
        private void ReqListViewFiller(PieceEntity piece)
        {


            List<ReqEntity> filtered_req_list = req_list.Where((row) =>
            {
                return
                    (row.Main_Piece.ToString().Equals(piece.Name));
            }).ToList();



            for (int i = 0; i < filtered_req_list.Count; i++)
            {
                DockPanel dp = new DockPanel();
                ReqTABLEUserControl new_req_row = new ReqTABLEUserControl(new ReqEntity(i, filtered_req_list[i].Main_Piece, filtered_req_list[i].Req_Piece));
                dp.Children.Add(new_req_row);
                dp.LastChildFill = true;
                dp.Margin = new Thickness(0, 5, 0, 0);
                ReqListView.Items.Add(dp);
            }
        }
        #endregion 
    }
}
