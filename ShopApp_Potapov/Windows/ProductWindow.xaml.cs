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
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {

        private ShopApp_PotapovEntities _context;
        private Products _product;
        private bool _isNew;

        public ProductWindow(ShopApp_PotapovEntities context, Products product)
        {
            InitializeComponent();

            _context = context;
            _product = product;
            _isNew = (product == null);

            LoadCategories();

            if (!_isNew)
            {
                LoadProductData();
                Title = "Редактирование товара";
            }
            else
            {
                Title = "Добавление товара";
            }
        }

        private void LoadCategories()
        {
            var categories = _context.Categories.ToList();

            cmbCategory.ItemsSource = categories;
            cmbCategory.DisplayMemberPath = "Name";
            cmbCategory.SelectedValuePath = "Id";

            // Если есть категории, выбираем первую
            if (categories.Any())
            {
                cmbCategory.SelectedIndex = 0;
            }
        }


        private void LoadProductData()
        {
            txtName.Text = _product.Name;
            cmbCategory.SelectedValue = _product.CategoryId;
            txtPrice.Text = _product.Price.ToString();
            txtQuantity.Text = _product.Quantity.ToString();
            txtDescription.Text = _product.Description ?? "";
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Валидация данных
            if (!ValidateData())
            {
                return;
            }

            if (_isNew)
            {
                // Создание нового товара
                _product = new Products();
                _context.Products.Add(_product);
            }

            // Заполнение полей
            _product.Name = txtName.Text.Trim();
            _product.CategoryId = (int)cmbCategory.SelectedValue;
            _product.Price = decimal.Parse(txtPrice.Text);
            _product.Quantity = int.Parse(txtQuantity.Text);
            _product.Description = txtDescription.Text.Trim();

            // Сохранение изменений
            _context.SaveChanges();

            MessageBox.Show(
                _isNew ? "Товар успешно добавлен!" : "Товар успешно обновлён!",
                "Успех",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            this.DialogResult = true;
            this.Close();
        }

        private bool ValidateData()
        {
            // Проверка названия
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название товара", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtName.Focus();
                return false;
            }

            if (txtName.Text.Trim().Length < 3)
            {
                MessageBox.Show("Название должно содержать не менее 3 символов", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtName.Focus();
                return false;
            }

            // Проверка категории
            if (cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbCategory.Focus();
                return false;
            }

            // Проверка цены
            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Введите цену товара", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPrice.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Некорректный формат цены", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPrice.Focus();
                return false;
            }

            if (price <= 0)
            {
                MessageBox.Show("Цена должна быть больше нуля", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPrice.Focus();
                return false;
            }

            // Проверка количества
            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Введите количество товара", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtQuantity.Focus();
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show("Некорректный формат количества", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtQuantity.Focus();
                return false;
            }

            if (quantity < 0)
            {
                MessageBox.Show("Количество не может быть отрицательным", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtQuantity.Focus();
                return false;
            }

            return true;
        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
