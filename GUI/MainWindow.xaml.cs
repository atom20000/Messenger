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
using Messenger;

namespace MessengerApp
{
    public struct Config
    {
        public string Url_server { get; set; }
        public int Update_rate { get; set; }
        public int Size_vertical { get; set; }
        public int Size_horizontal { get; set; }
        public bool Auth_in_file { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize<Config>(this, new JsonSerializerOptions() {WriteIndented=true});
        }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static Config config;
        internal static Authanswer answer;
        internal static CheckMessResponse messages;

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if(Login.Text=="Sign in")
            {
                HttpWebRequest request = WebRequest.CreateHttp($"{config.Url_server}/api/Authentication/auth");
                request.Method = "POST";
                request.ContentType = "application/json";
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(JsonSerializer.Serialize(new List<string>() { LoginBox.Text, PasswordBox.Password }));
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string a = reader.ReadToEnd();
                        answer = JsonSerializer.Deserialize<Authanswer>(a, new JsonSerializerOptions() {PropertyNameCaseInsensitive=true});
                    }
                }
                if ((answer.Nicknameuser == "Login not found") || (answer.Nicknameuser == "Password invalid"))
                {
                    WarningBlock.Visibility = Visibility.Visible;
                    WarningBlock.Text = answer.Nicknameuser;
                }
                else
                {
                    chat _chat = new chat();
                    this.Close();
                    _chat.Show();
                    request = WebRequest.CreateHttp($"{config.Url_server}/api/Message/oldmes/{answer.Chatnames_Id[0].ID_chat}");
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(new Check_message_request(new DateTime(),answer.Iduser).ToJson());
                    }
                    response = (HttpWebResponse)request.GetResponse();
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string a = reader.ReadToEnd();
                            messages = JsonSerializer.Deserialize<CheckMessResponse>(a, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                        }
                    }
                    _chat.Count.Text = $"{ messages.Count_members.ToString()} members";
                    foreach (Message mess in MainWindow.messages.Mess_list)
                    {
                        _chat.Draw(mess);
                    }
                    _chat.timeFirstLastMessage.Time_first_message = messages.Mess_list[0].TimeSend;
                    _chat.timeFirstLastMessage.Time_last_message = messages.Mess_list[^1].TimeSend;
                    _chat.Check_new_message();
                }
            }
            if(Login.Text == "Sign up")
            {
                HttpWebRequest request = WebRequest.CreateHttp($"{config.Url_server}/api/Authentication/reg");
                request.Method = "POST";
                request.ContentType = "application/json";
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if((LoginBox.Text!="Login") && (PasswordBox.Password != "Password"))
                {
                    using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(JsonSerializer.Serialize(new User(LoginBox.Text, PasswordBox.Password, NicknameBox.Text)));
                    }
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string a = reader.ReadToEnd();
                            answer = Authanswer.FromJson(a);
                        }
                    }
                    if ((answer.Nicknameuser == "This login is registered") || (answer.Nicknameuser == "This nickname is busy"))
                    {
                        WarningBlock.Visibility = Visibility.Visible;
                        WarningBlock.Text = answer.Nicknameuser;
                    }
                    if (answer.Nicknameuser == "Davai cherez auth")
                    {
                        request = WebRequest.CreateHttp($"{config.Url_server}/api/Authentication/auth");
                        request.Method = "POST";
                        request.ContentType = "application/json";
                        //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                        {
                            writer.Write(JsonSerializer.Serialize(new List<string>() { LoginBox.Text, PasswordBox.Password }));
                        }
                        response = (HttpWebResponse)request.GetResponse();

                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                string a = reader.ReadToEnd();
                                answer = JsonSerializer.Deserialize<Authanswer>(a, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                            }
                        }
                        if ((answer.Nicknameuser == "Login not found") || (answer.Nicknameuser == "Password invalid"))
                        {
                            WarningBlock.Visibility = Visibility.Visible;
                            WarningBlock.Text = answer.Nicknameuser;
                        }
                        else
                        {                           
                            chat _chat = new chat();
                            _chat.Show();
                            this.Close();
                            request = WebRequest.CreateHttp($"{config.Url_server}/api/Message/oldmes/{answer.Chatnames_Id[0].ID_chat}");
                            request.Method = "POST";
                            request.ContentType = "application/json";
                            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                            {
                                writer.Write(new Check_message_request(new DateTime(), answer.Iduser).ToJson());
                            }
                            response = (HttpWebResponse)request.GetResponse();
                            using (Stream stream = response.GetResponseStream())
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    string a = reader.ReadToEnd();
                                    messages = JsonSerializer.Deserialize<CheckMessResponse>(a, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                                }
                            }
                            _chat.Count.Text = $"{ messages.Count_members.ToString()} members";
                            foreach (Message mess in MainWindow.messages.Mess_list)
                            {
                                _chat.Draw(mess);
                            }
                            _chat.timeFirstLastMessage.Time_first_message = messages.Mess_list[0].TimeSend;
                            _chat.timeFirstLastMessage.Time_last_message = messages.Mess_list[^1].TimeSend;
                            _chat.Check_new_message();
                            
                        }
                    }
                }
                else
                {
                    WarningBlock.Visibility = Visibility.Visible;
                    WarningBlock.Text = "Michalich zverb";
                }
            }           
        }
        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            NicknameBox.Visibility=Visibility.Visible;
            Login.Text = "Sign up";
            SignupButton.Visibility = Visibility.Collapsed;
            SigninButton.Visibility = Visibility.Visible;
        }

        private void SigninButton_Click(object sender, RoutedEventArgs e)
        {
            NicknameBox.Visibility = Visibility.Collapsed;
            Login.Text = "Sign in";
            SigninButton.Visibility = Visibility.Collapsed;
            SignupButton.Visibility = Visibility.Visible;
        }

        private void MouseDoubleLog(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text=="Login")
            {
                LoginBox.Clear();
            }
        }

        private void MouseDoublePass(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == "Password")
            {
                PasswordBox.Clear();
            }
        }

        private void MouseDoubleNick(object sender, RoutedEventArgs e)
        {
            if (NicknameBox.Text=="Nickname")
            {
                NicknameBox.Clear();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                config = JsonSerializer.Deserialize<Config>(File.ReadAllText("config.json"));

            }
            catch
            {
                config = new Config()
                {
                    Url_server = "http://localhost:5000",
                    Auth_in_file = false,
                    Login = null,
                    Password = null,
                    Size_horizontal = 1920,
                    Size_vertical = 1080,
                    Update_rate = 1000
                };
                File.WriteAllText(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "config.json"), config.ToString());
            }
        }
    }
}
