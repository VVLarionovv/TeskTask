using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
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
using System.Data;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Windows.Controls.Primitives;

namespace TestTask
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString;
        public SqlDataReader dataReader;
        public SqlConnection connection;
        public SqlCommand command;
        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["TestDB.ConnectionString"].ConnectionString;
            DepartmentComboBox.Items.Add("Отдел тестирования");
            DepartmentComboBox.Items.Add("Отдел поддержки");
            DepartmentComboBox.Items.Add("Отдел социальной помощи");
            DepartmentComboBox.Items.Add("Отдел дизайна");
            ManagerComboBox.Items.Add("И.И.Иванов");
            ManagerComboBox.Items.Add("А.А.Алексеев");
            ManagerComboBox.Items.Add("В.В.Буканов");
            ManagerComboBox.Items.Add("Г.Г.Героев");
            loadDB();
        }
        public void loadDB()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string cmd = "SELECT IDUser AS[Номер пользователя], FIO AS ФИО, dateBirth AS ДР," +
                " Description AS Описание, Department AS Отдел, Director AS Руководитель, " +
                "istheheadof AS ДаОрНет from Users";
            SqlCommand createCommand = new SqlCommand(cmd, connection);
            createCommand.ExecuteNonQuery();

            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable("Users"); 
            dataAdp.Fill(dt);
            Info.ItemsSource = dt.DefaultView; 
            connection.Close();
        }
        private void addInfo(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("UsersInsert", connection);
            command.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            try
            {
                if (FIOTextBox.Text != "")
                {
                    command.Parameters.AddWithValue("@FIO", FIOTextBox.Text);
                }

                else
                {
                    MessageBox.Show("Поле FIO - пустое");
                }
                if (BirthDatePicker.Text != "")
                {
                    command.Parameters.AddWithValue("@dateBirth", Convert.ToDateTime(BirthDatePicker.Text));
                }
                else
                {
                    MessageBox.Show("Поле dateBirth - пустое");
                }
                if (DescriptionTextBox.Text != "")
                {
                    command.Parameters.AddWithValue("@Description", DescriptionTextBox.Text);
                }
                else
                {
                    MessageBox.Show("Поле Description - пустое");
                }

                if (DepartmentComboBox.Text != "")
                {
                    command.Parameters.AddWithValue("@Department", DepartmentComboBox.Text);
                }
                else
                {
                    MessageBox.Show("Поле Department - пустое");
                }


                if (ManagerComboBox.Text != "")
                {
                    command.Parameters.AddWithValue("@Director", ManagerComboBox.Text);
                }
                else
                {
                    MessageBox.Show("Поле Director - пустое ");
                }


                if (IsManagerCheckBox.IsChecked != false)
                {
                    command.Parameters.AddWithValue("@istheheadof",1);
                }
                else
                {
                    command.Parameters.AddWithValue("@istheheadof", 0);
                }
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          
           
        }

        private void editInfo(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Info.SelectedItem != null)
                {
                    SqlConnection connection = new SqlConnection(connectionString);
                    connection.Open();
                    SqlCommand command = new SqlCommand("Users_Update", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IDUser", Convert.ToInt32(ID.Text));
                    command.Parameters.Add("@FIO", SqlDbType.VarChar, 100).Value = FIOTextBox.Text;
                    command.Parameters.AddWithValue("@dateBirth", Convert.ToDateTime(BirthDatePicker.Text));
                    command.Parameters.AddWithValue("@Description", DescriptionTextBox.Text);
                    command.Parameters.AddWithValue("@Department", DepartmentComboBox.Text);
                    command.Parameters.AddWithValue("@Director", ManagerComboBox.Text);
                    if (IsManagerCheckBox.IsChecked != false)
                    {
                        command.Parameters.AddWithValue("@istheheadof", 1);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@istheheadof", 0);
                    }
                    command.ExecuteNonQuery();
                    connection.Close();
                    loadDB();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void deleteInfo(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ID.Text != "")
                {
                    SqlConnection connection = new SqlConnection(connectionString);
                    connection.Open();
                    SqlCommand command = new SqlCommand("Users_Delete", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IDUser", Convert.ToInt32(ID.Text));
                    command.ExecuteNonQuery();
                    connection.Close();
                    loadDB();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Pick_C(object sender, SelectionChangedEventArgs e)
        {
            if (Info.SelectedItem != null)
            {
                ID.Text = (Info.SelectedItem as DataRowView).Row.ItemArray[0].ToString();
                FIOTextBox.Text = (Info.SelectedItem as DataRowView).Row.ItemArray[1].ToString();
                BirthDatePicker.Text = (Info.SelectedItem as DataRowView).Row.ItemArray[2].ToString();
                DescriptionTextBox.Text = (Info.SelectedItem as DataRowView).Row.ItemArray[3].ToString();
                DepartmentComboBox.Text = (Info.SelectedItem as DataRowView).Row.ItemArray[4].ToString();
                ManagerComboBox.Text = (Info.SelectedItem as DataRowView).Row.ItemArray[5].ToString();
            }
        }
    }
}
