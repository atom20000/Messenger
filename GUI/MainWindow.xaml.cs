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
using System.Net;
using System.IO;
using System.Text.Json;
using MessengerLibrary;

namespace MessengerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:5000/api/Message/100500");
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                MessageBox.Show(reader.ReadToEnd(), "GET", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:5000/api/Message");
            request.Method = "POST";
            request.ContentType = "application/json";
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(JsonSerializer.Serialize(new Message(DateTime.Now, "gjghjkdf", "jjgjgjgjgjg")));
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                MessageBox.Show(reader.ReadToEnd(), "POST", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            /* if (LoginBox.Text == "admin" && PasswordBox.Password == "12345")
             {
                 Console.WriteLine("good");
             }
             else
             {
                 LoginMessageBlock.Text = "Wrong login or password!";
                 LoginMessageBlock.Visibility = Visibility.Visible;
             */
            /*HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://55bc3afdaab6.ngrok.io/api/Authentication/reg");
            request.Method = "POST";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(JsonSerializer.Serialize(new User("login", "password", "nickname"))); 
            }
            */

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://55bc3afdaab6.ngrok.io/api/Authentication/auth");
            request.Method = "POST";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(JsonSerializer.Serialize(new List<string>() {LoginBox.Text, PasswordBox.Password}));
            }

        }
    }
}
