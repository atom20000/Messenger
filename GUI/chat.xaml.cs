using System;
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

        internal (DateTime Time_first_message, DateTime Time_last_message) timeFirstLastMessage;
        private void ButtonMessage_Click(object sender, RoutedEventArgs e)
        {
            string ans = "";
            while(ans!= "Ok pochta doshla") 
            {
                HttpWebRequest request = WebRequest.CreateHttp($"{MainWindow.config.Url_server}/api/Message/sendmes/{MainWindow.answer.Chatnames_Id[0].ID_chat}"); // id чата, метод должен понимать из какого чата его запустили 
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
                    CheckMessResponse New_Message = Message_request();
                    MessageField.Dispatcher.Invoke(() =>
                    {
                        Count.Text = $"{ New_Message.Count_members.ToString()} members";
                        bool bottom = Scroll.VerticalOffset == Scroll.ScrollableHeight;

                        foreach (Message mess in New_Message.Mess_list)
                        {
                            Draw(mess);
                            MessageBox.Text = "";
                        }
                        if (bottom)
                        {
                            Scroll.ScrollToBottom();
                        }
                    });
                    Task.Delay(MainWindow.config.Update_rate);
                }
            });
        }
        internal CheckMessResponse Message_request()
        {
            CheckMessResponse New_Message;
            HttpWebRequest request = WebRequest.CreateHttp($"{MainWindow.config.Url_server}/api/Message/checknewmes/{MainWindow.answer.Chatnames_Id[0].ID_chat}"); // id чата, метод должен понимать из какого чата его запустили 
            request.Method = "POST";
            request.ContentType = "application/json";
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(JsonSerializer.Serialize(new Check_message_request(timeFirstLastMessage.Time_last_message, MainWindow.answer.Iduser)));// Дописать время последнего сообщения
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    New_Message = IMainFunction.FromJson<CheckMessResponse>(reader.ReadToEnd());
                }
            }
            if (New_Message.Mess_list.Count != 0)
            {
                timeFirstLastMessage.Time_last_message = New_Message.Mess_list[0].TimeSend;
            }
            return New_Message;
        }

        private void GridDraw(Message _message)
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
            MessageField.Children.Insert(1, mainGrid);

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

        private void MessageBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Text == "Type your Message...")
            {
                MessageBox.Clear();
            }

        }

        private void HistoryMess_Click(object sender, RoutedEventArgs e)
        {
            HttpWebRequest request = WebRequest.CreateHttp($"{MainWindow.config.Url_server}/api/Message/oldmes/{MainWindow.answer.Chatnames_Id[0].ID_chat}");
            request.Method = "POST";
            request.ContentType = "application/json";
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(new Check_message_request( timeFirstLastMessage.Time_first_message, MainWindow.answer.Iduser).ToJson());
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string a = reader.ReadToEnd();
                    MainWindow.messages = JsonSerializer.Deserialize<CheckMessResponse>(a, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                }
            }
            MainWindow.messages.Mess_list.Reverse();
            foreach (Message mess in MainWindow.messages.Mess_list)
            {
                GridDraw(mess);
            }
            MainWindow.messages.Mess_list.Reverse();
            if (MainWindow.messages.Mess_list.Count!=0)
            {
                timeFirstLastMessage.Time_first_message = MainWindow.messages.Mess_list[0].TimeSend;
            }
            
        }
        private void Chat_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HttpWebRequest request = WebRequest.CreateHttp($"{MainWindow.config.Url_server}/api/Authentication/singout");
            request.Method = "POST";
            request.ContentType = "application/json";
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(new Sing_out_request(MainWindow.answer.Nicknameuser, MainWindow.answer.Chatnames_Id.ConvertAll(mem => mem.ID_chat), DateTime.Now.ToUniversalTime()).ToJson());
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        }

        private void MessageBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Text == "")
            {
                MessageBox.Text = "Type your Message...";
            }
        }
    }
}
