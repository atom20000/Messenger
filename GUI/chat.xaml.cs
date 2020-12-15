using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
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

        private void ButtonMessage_Click(object sender, RoutedEventArgs e)
        {
            string ans;
            HttpWebRequest request = WebRequest.CreateHttp($"{MainWindow.config.Url_server}/api/Message/sendmes/{MainWindow.answer.Chatnames_Id[0].Item1}"); // id чата, метод должен понимать из какого чата его запустили 
            request.Method = "POST";
            request.ContentType = "application/json";
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {//Beta здесь был
                writer.Write(JsonSerializer.Serialize(new Message(DateTime.Now, MessageBox.Text, MainWindow.answer.Iduser, MainWindow.answer.Nicknameuser)));
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
                /*ChatMessage.Text = MessageBox.Text;
                ChatMessage.HorizontalAlignment = HorizontalAlignment.Stretch;
                //ChatMessage.Background = new Brush(Color.FromRgb());
                ChatMessage.Background = Brushes.Aqua;
                ChatMessage.Width = 100;
                ChatMessage.FontSize = 20;
                ChatMessage.Margin = new Thickness(30, 30, 1000, 1000);
                ChatMessage.IsEnabled = false;
                ChatMessageBlock.Text = DateTime.Now.TimeOfDay.ToString();
                ChatMessageBlock.VerticalAlignment = VerticalAlignment.Bottom;
                ChatMessageBlock.FontSize = 10;
                */
                Grid mainGrid = new Grid();
                StackPanel imageStackPanel = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,

                };
                StackPanel nameStackPanel = new StackPanel()
                {
                Orientation = Orientation.Vertical

                };
                StackPanel messageStackPanel = new StackPanel()
                {
                    Orientation=Orientation.Horizontal,

                };
                Image icon = new Image()
                {

                };
                TextBlock nickname = new TextBlock()
                {
                    Text = "oleg"
                };
                TextBox message = new TextBox()
                {
                    Width = 300,
                    AcceptsReturn = true,
                    Text = MessageBox.Text,
                    IsEnabled=false
                    
                };
                TextBox time = new TextBox()
                {
                    IsEnabled=false
                };

                messageStackPanel.Children.Add(message);
                messageStackPanel.Children.Add(time);
                nameStackPanel.Children.Add(nickname);
                nameStackPanel.Children.Add(messageStackPanel);
                imageStackPanel.Children.Add(icon);
                imageStackPanel.Children.Add(nameStackPanel);
                mainGrid.Children.Add(imageStackPanel);
                MessageField.Children.Add(mainGrid);


            } 

            /*while (!false)
			{
                // обновляй историю сообщений
                Thread.Sleep(10000 какое-то время );
            */
			
            
        }

    }

}
