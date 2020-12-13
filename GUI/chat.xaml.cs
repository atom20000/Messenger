using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            HttpWebRequest request = WebRequest.CreateHttp("http://56410c0b1e23.ngrok.io/api/Message/createchat");
            request.Method = "POST";
            request.ContentType = "application/json";
        }
        
    }
  
}
