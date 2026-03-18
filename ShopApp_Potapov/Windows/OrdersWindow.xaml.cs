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
using System.Windows.Shapes;

namespace ShopApp_Potapov.Windows
{
    /// <summary>
    /// Логика взаимодействия для OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        public string NewStatus { get; private set; }

        public OrdersWindow(string currentStatus)
        {
            InitializeComponent();

            switch (currentStatus)
            {
                case "Оформлен":
                    rbCreated.IsChecked = true;
                    break;
                case "Отправлен":
                    rbShipped.IsChecked = true;
                    break;
                case "Доставлен":
                    rbDelivered.IsChecked = true;
                    break;
                default:
                    rbCreated.IsChecked = true;
                    break;
            }

            Title = $"Изменение статуса заказа (текущий: {currentStatus})";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (rbCreated.IsChecked == true)
            {
                NewStatus = "Оформлен";
            }
            else if (rbShipped.IsChecked == true)
            {
                NewStatus = "Отправлен";
            }
            else if (rbDelivered.IsChecked == true)
            {
                NewStatus = "Доставлен";
            }
            else
            {
                MessageBox.Show("Выберите статус", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
