using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ShopApp_Potapov.Windows;

namespace ShopApp_Potapov.Pages
{
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        private ShopApp_PotapovEntities _context;
        private Users currentUser;

        private ObservableCollection<CartItem> _cartItems;

        public CartPage(ShopApp_PotapovEntities context, Users user)
        {
            InitializeComponent();
            _context = context;
            currentUser = user;

            _cartItems = new ObservableCollection<CartItem>();

            LoadProducts();
            dgCart.ItemsSource = _cartItems;
        }

        private void LoadProducts()
        {
            var products = _context.Products
                .Where(p => p.Quantity > 0)
                .ToList();

            cmbProducts.ItemsSource = products;
            cmbProducts.DisplayMemberPath = "Name";
            cmbProducts.SelectedValuePath = "Id";
        }

        private void BtnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (cmbProducts.SelectedItem is Products product)
            {
                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Введите корректное количество");
                    return;
                }

                if (quantity > product.Quantity)
                {
                    MessageBox.Show($"Недостаточно товара на складе. Доступно: {product.Quantity}");
                    return;
                }

                var existingItem = _cartItems.FirstOrDefault(i => i.ProductId == product.Id);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                    existingItem.Total = existingItem.Price * existingItem.Quantity;
                }
                else
                {
                    _cartItems.Add(new CartItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = quantity,
                        Total = product.Price * quantity
                    });
                }

                CalculateTotal();
            }
            else
            {
                MessageBox.Show("Выберите товар");
            }
        }

        private void BtnRemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is CartItem item)
            {
                _cartItems.Remove(item);
                CalculateTotal();
            }
        }

        private void CalculateTotal()
        {
            decimal total = _cartItems.Sum(i => i.Total);
            txtTotal.Text = $"{total:C}";
        }

        private void BtnCheckout_Click(object sender, RoutedEventArgs e)
        {
            if (_cartItems.Count == 0)
            {
                MessageBox.Show("Корзина пуста");
                return;
            }

            // Открываем окно выбора клиента
            //var checkoutWindow = new CheckoutWindow(_context, _cartItems, currentUser);
            //if (checkoutWindow.ShowDialog() == true)
            //{
            //    _cartItems.Clear();
            //    CalculateTotal();
            //    MessageBox.Show("Заказ оформлен!");
            //}
        }


    public class CartItem
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public decimal Total { get; set; }
        }
    }
}
