using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace GiaTest2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
/// 
public class Info
{
    public string PartnerType { get; set; }
    public string PartnerName { get; set; }
    public string Director { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string LegalAddress { get; set; }
    public string Inn { get; set; }

    public Info(string partnertype, string partnername, string director, string email, string phone, string legaladdress, string inn)
    {
        this.PartnerType = partnertype;
        this.PartnerName = partnername;
        this.Director = director;
        this.Email = email;
        this.Phone = phone;
        this.LegalAddress = legaladdress;
        this.Inn = inn;
    }
}


public partial class MainWindow : Window
{
    public List<Info> InfoList = new List<Info>();
    public NpgsqlConnection Connection;
    public MainWindow()
    {
        Connection = new NpgsqlConnection("Host=localhost;port=5432;Username=postgres;Password=-=-=;Database=GIA_Test");
        Connection.Open();
        
        InitializeComponent();



            
    }
    private void ParashaClick(object obj, RoutedEventArgs e)
    {
        AddWindow newWinow = new AddWindow();
        newWinow.Show();
        //newWinow.OnClosed += (s, ev) => if(newWinow.IsAlterd == true) GetNewInfo(newWinow);
    }
    private void GetNewInfo(AddWindow newwindow)
    {
        Info info = new Info(
        newwindow.PartnertypeBox.Text,
        newwindow.PartnernameBox.Text,
        newwindow.DirectorBox.Text,
        newwindow.EmailBox.Text,
        newwindow.PhoneBox.Text,
        newwindow.AddressBox.Text,
        newwindow.InnBox.Text
            );
        SetPartners(info);
        GetPartners();
        this.MainListBox.Items.Refresh();


    }
    private void LoginClick(object obj, RoutedEventArgs e)
    {

        if (GetLoginInfo(LoginUsername.Text, LoginPasswd.Text))
        {
            LoginGrid.Visibility = Visibility.Hidden;
            MainGrid.Visibility = Visibility.Visible;
            GetPartners();
            this.MainListBox.ItemsSource = this.InfoList;
        }

    }
    private void SetPartners(Info info)
    {
        using var comm = new NpgsqlCommand("INSERT INTO partner (partnertype, partnername, director, email, phone, legaladdress, inn, rating) VALUES (@PartnerType, @PartnerName, @Director, @Email, @Phone, @Legaladdress, @Raiting, @INN)", this.Connection);
        comm.Parameters.AddWithValue("@PartnerName", info.PartnerName);
        comm.Parameters.AddWithValue("@PartnerType", info.PartnerType);
        comm.Parameters.AddWithValue("@Director", info.Director);
        comm.Parameters.AddWithValue("@Email", info.Email);
        comm.Parameters.AddWithValue("@Phone", info.Phone);
        comm.Parameters.AddWithValue("@LegalAddress", info.LegalAddress);
        comm.Parameters.AddWithValue("@INN", "0000000");
        comm.Parameters.AddWithValue("@Raiting", 10);

        comm.ExecuteNonQuery();

    }   
    private void GetPartners()
    {
        this.InfoList.Clear();
        using var comm = new NpgsqlCommand("SELECT * FROM Partner", this.Connection);
        using var reader = comm.ExecuteReader();

        while (reader.Read())
        {
            this.InfoList.Add(new Info(
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4),
                reader.GetString(5),
                reader.GetString(6),
                reader.GetString(7)

                ));
        }

    }
    private bool GetLoginInfo(string login, string passwd)
    {
        NpgsqlConnection GetUserConn = new NpgsqlConnection("Host=localhost;port=5432;Username=postgres;Password=-=-=;Database=GIA_Test2");
        GetUserConn.Open();

        using var comm = new NpgsqlCommand("SELECT * FROM public.users WHERE \"UserName\" = @Username AND \"UserPasswd\" = @UserPassword", GetUserConn);
        comm.Parameters.AddWithValue("@Username", login);
        comm.Parameters.AddWithValue("@UserPassword", passwd);
        using var reader = comm.ExecuteReader();

        while (reader.Read())
        {
            GetUserConn.Close();
            return true;
        }

        GetUserConn.Close();
        return false;
    }
    




}