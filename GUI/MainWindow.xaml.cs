﻿using System;
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
            if(Login.Text=="Sign in")
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://91bce97e71b7.ngrok.io/api/Authentication/auth");
                request.Method = "POST";
                request.ContentType = "application/json";
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(JsonSerializer.Serialize(new List<string>() { LoginBox.Text, PasswordBox.Password }));
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Authanswer answer;
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        answer = JsonSerializer.Deserialize<Authanswer>(reader.ReadToEnd());
                    }
                }
                if ((answer.Nicknameuser == "Login not found") || (answer.Nicknameuser == "Password invalid"))
                {
                    WarningBlock.Visibility = Visibility.Visible;
                    WarningBlock.Text = answer.Nicknameuser;
                }
                else
                {
                    //надо чтобы информация сервера отображалась в главном окне
                }
            }
            if(Login.Text == "Sign up")
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://91bce97e71b7.ngrok.io/api/Authentication/reg");
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
                    Authanswer answer;
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            answer = JsonSerializer.Deserialize<Authanswer>(reader.ReadToEnd());
                        }
                    }
                    if ((answer.Nicknameuser == "This login is registered") || (answer.Nicknameuser == "This nickname is busy"))
                    {
                        WarningBlock.Visibility = Visibility.Visible;
                        WarningBlock.Text = answer.Nicknameuser;
                    }
                    else
                    {
                        WarningBlock.Visibility = Visibility.Visible;
                        WarningBlock.Text = answer.Nicknameuser;
                        //надо чтобы информация сервера отображалась в главном окне
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



    }
}
