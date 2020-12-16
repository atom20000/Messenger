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
                    Text = MainWindow.answer.Nicknameuser,
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    FontSize = 14,
                    VerticalAlignment=VerticalAlignment.Center,
                    Margin = new Thickness(5, 0, 0, 0)
                };
                TextBox message = new TextBox()
                {
                    Width = 300,
                    AcceptsReturn = true,
                    TextWrapping = TextWrapping.Wrap,
                    Text = MessageBox.Text,
                    IsEnabled=false,
                    FontSize=26,   
                    Background = Brushes.Transparent,
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(49, 46, 43)),
                    
                };
                TextBox time = new TextBox()
                {
                    IsEnabled = false,
                    Text = DateTime.Now.ToString("HH:mm"),
                    FontSize=14,
                    VerticalAlignment=VerticalAlignment.Center, 
                    Foreground=Brushes.White,
                    BorderBrush = new SolidColorBrush(Color.FromRgb(49, 46, 43)),
                    Background = Brushes.Transparent,
                    Margin=new Thickness(10, 0, 10, 0),
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

            /*while (!false)
			{
                // обновляй историю сообщений
                Thread.Sleep(10000 какое-то время );
            */
			
            
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
    }

}
