using NorthWind.DataAccess;
using NorthWind.Repositories;
using NorthWind.Service;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NorthWind
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly EmployeeService _employeeService;
        private readonly CustomerService _customerService;
        public MainWindow()
        {
            InitializeComponent();
           _productService = new ProductService();
           _categoryService = new CategoryService();
            _employeeService = new EmployeeService();
            _customerService = new CustomerService();

        }

        private void Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ((ComboBoxItem)((ComboBox)sender).SelectedItem).Content.ToString();
            switch (selectedItem)
            {
                case "Product":
                    loadProductStatistics();
                    break;
                case "Categories":
                    loadCategoryStatistics();
                    break;
                case "Employee":
                    loadEmployeeStatistics();
                    break;
                case "Customer":
                    loadCustomerStatistics();
                    break;
            }

        }    
        private void loadProductStatistics() {
            var data = _productService.GetProductStatistics();
            dgStatistics.ItemsSource = data;
        }
        private void loadCategoryStatistics()
        {
            var data =  _categoryService.GetCategoriesStaistics();
            dgStatistics.ItemsSource = data;
        }
        private void loadEmployeeStatistics()
        {
            var data = _employeeService.GetEmployeeStatistics();
            dgStatistics.ItemsSource = data;
        }
        private void  loadCustomerStatistics()
        {
            var data = _customerService.GetCustomerStatistics();
            dgStatistics.ItemsSource = data;
        }

        private async void LoadDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgStatistics.ItemsSource == null)
            {
                MessageBox.Show("Không có dữ liệu để export");
                return;
            }

            if (cbStatistics.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn loại dữ liệu");
                return;
            }

            txtStatus.Text = "Exporting...";

            var data = dgStatistics.ItemsSource.Cast<object>().ToList();

            string selectedName = cbStatistics.Text;
            string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"{selectedName}_{timeStamp}.json";

            string folderPath = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = System.IO.Path.Combine(folderPath, fileName);

            await Task.Run(() =>
            {
                string json = JsonSerializer.Serialize(
                    data,
                    new JsonSerializerOptions { WriteIndented = true }
                );

                File.WriteAllText(fullPath, json);
            });

            txtStatus.Text = "Export completed";

            MessageBox.Show($"Đã export dữ liệu ra file:\n{fileName}");
        }


    }
}
