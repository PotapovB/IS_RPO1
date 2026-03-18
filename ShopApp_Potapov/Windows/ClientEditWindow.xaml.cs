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
    /// Логика взаимодействия для ClientEditWindow.xaml
    /// </summary>
    public partial class ClientEditWindow : Window
    {
        private ShopApp_PotapovEntities _context;
        private Clients _client;
        private Users _user;

        public ClientEditWindow(ShopApp_PotapovEntities context, Clients client, Users user)
        {
            InitializeComponent();
            _context = context;
            _client = client;
            _user = user;

            LoadData();
        }

        private void LoadData()
        {
            // Загружаем данные клиента
            txtFullName.Text = _client.FullName;
            txtPhone.Text = _client.Phone;
            txtEmail.Text = _client.Email;

            // Загружаем данные пользователя
            txtLogin.Text = _user.Login;

            // Устанавливаем текущую роль
            if (_user.RoleId == 2)
            {
                cmbRole.SelectedIndex = 1; // Продавец
            }
            else
            {
                cmbRole.SelectedIndex = 0; // Покупатель
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Введите телефон", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Сохраняем изменения клиента
            _client.FullName = txtFullName.Text.Trim();
            _client.Phone = txtPhone.Text.Trim();
            _client.Email = txtEmail.Text.Trim();

            // Сохраняем изменения роли пользователя
            if (cmbRole.SelectedItem is ComboBoxItem selectedRole)
            {
                int newRoleId = int.Parse(selectedRole.Tag.ToString());
                _user.RoleId = newRoleId;
            }

            _context.SaveChanges();

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
