﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessengerApp;
using System.Text.Json;
using MessengerLibrary;

namespace Messenger
{
    /// <summary>
    /// Логика взаимодействия для chat.xaml
    /// </summary>
    public partial class chat : Window
    {
        public chat()
        {
            InitializeComponent();
        }
        
        private void ButtonGroup_Click(object sender, RoutedEventArgs e)
        {
            /*
            HttpWebRequest request = WebRequest.CreateHttp($"{MainWindow.config.Url_server}/api/Message/createchat");  
            request.Method = "POST";
            request.ContentType = "application/json";
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
               // writer.Write(JsonSerializer.Serialize(new ))
            }
            */
        }

        internal (DateTime, DateTime) timeFirstLastMessage;
        private void ButtonMessage_Click(object sender, RoutedEventArgs e)
        {
            string ans;
            HttpWebRequest request = WebRequest.CreateHttp($"{MainWindow.config.Url_server}/api/Message/sendmes/{MainWindow.answer.Chatnames_Id[0].Item1}"); // id чата, метод должен понимать из какого чата его запустили 
            request.Method = "POST";
            request.ContentType = "application/json";
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {//Beta здесь был
                writer.Write(JsonSerializer.Serialize(new Message(DateTime.Now.ToUniversalTime(), MessageBox.Text, MainWindow.answer.Iduser, MainWindow.answer.Nicknameuser)));
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    ans = reader.ReadToEnd();
                }
            }
            if (ans == "Ok pochta doshla")
            {
               
            } 

        }

        private void MessageBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Keyboard.Modifiers == ModifierKeys.None)
                {
                    e.Handled = true;
                    ButtonMessage_Click(MessageClick, null);
                }
            }
        }

        internal async void Check_new_message()
        {

            await Task.Run(() => 
            {
                while (true)
                {
                    Message_request();
                    MessageField.Dispatcher.Invoke(() =>
                    {

                    //Count.Text = $"{ MainWindow.messages.Count_members.ToString()} members";

                    foreach (Message mess in MainWindow.messages.Mess_list)
                        {
                            Draw(mess);
                        }

                    });
                    Task.Delay(MainWindow.config.Update_rate);

                }

            });


        }
        internal void  Message_request()
        {
            HttpWebRequest request = WebRequest.CreateHttp($"{MainWindow.config.Url_server}/api/Message/checknewmes/{MainWindow.answer.Chatnames_Id[0].Item1}"); // id чата, метод должен понимать из какого чата его запустили 
            request.Method = "POST";
            request.ContentType = "application/json";
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(JsonSerializer.Serialize(new Check_message_request(timeFirstLastMessage.Item2, MainWindow.answer.Iduser)));// Дописать время последнего сообщения
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    MainWindow.messages = IMainFunction.FromJson<CheckMessResponse>(reader.ReadToEnd()); 
                }
            }
            if (MainWindow.messages.Mess_list.Count != 0)
            {
                timeFirstLastMessage.Item2 = MainWindow.messages.Mess_list[0].TimeSend;
            }
        }

        private void GridDraw(Message _message)
        {    
        
        }
        
        internal void Draw(Message _message)
        {
            Grid mainGrid = new Grid()
            {
                Margin = new Thickness(10)
            };
            StackPanel imageStackPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,

            };
            StackPanel nameStackPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            StackPanel nameTimeStackPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
            };
            Image icon = new Image()
            {

            };
            TextBlock nickname = new TextBlock()
            {
                Text = _message.Sender_Nickname,
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                FontSize = 14,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5, 0, 0, 0),

            };
            TextBox message = new TextBox()
            {
                Width = 300,
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                Text = _message.Text,
                IsEnabled = false,
                FontSize = 26,
                Background = Brushes.Transparent,
                Foreground = Brushes.Aqua,
                BorderBrush = new SolidColorBrush(Color.FromRgb(49, 46, 43)),

            };
            TextBox time = new TextBox()
            {
                IsEnabled = false,
                Text = _message.TimeSend.ToLocalTime().ToString("HH:mm"),
                FontSize = 14,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Brushes.White,
                BorderBrush = new SolidColorBrush(Color.FromRgb(49, 46, 43)),
                Background = Brushes.Transparent,
                Margin = new Thickness(10, 0, 10, 0),
            };

            nameStackPanel.Children.Add(nickname);
            nameStackPanel.Children.Add(time);
            nameTimeStackPanel.Children.Add(nameStackPanel);
            nameTimeStackPanel.Children.Add(message);
            imageStackPanel.Children.Add(icon);
            imageStackPanel.Children.Add(nameTimeStackPanel);
            mainGrid.Children.Add(imageStackPanel);
            MessageField.Children.Add(mainGrid);
        }

        private void MessageBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Text== "Type your Message...")
            {
                MessageBox.Clear();
            }

        }
    }

}
