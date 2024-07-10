using MomAndChildren.Business;
using MomAndChildren.Data.Models;
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

namespace MomAndChildren.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for wProduct.xaml
    /// </summary>
    public partial class wProduct : Window
    {
        private readonly ProductBusiness _business;
        private readonly BrandBusiness _brandBusiness;
        private readonly CategoryBusiness _categoryBusiness;

        public wProduct()
        {
            InitializeComponent();
            this._business ??= new ProductBusiness();
            this._brandBusiness ??= new BrandBusiness();
            this._categoryBusiness ??= new CategoryBusiness();
            LoadGrdProducts();
            LoadGrdBrandAndCategory();
        }

        private async void LoadGrdBrandAndCategory()
        {
            var brands = (List<Brand>)(await _brandBusiness.GetBrandsAsync()).Data;
            var categories = (List<Category>)(await _categoryBusiness.GetCategoriesAsync()).Data;

            txtProductBrand.ItemsSource = brands;
            txtProductBrand.IsEnabled = true;
            txtProductCategory.ItemsSource = categories;
            txtProductCategory.IsEnabled = true;

        }

        private async void LoadGrdProducts()
        {
            var result = await _business.GetProductsWithNestedObjAsync();

            if (result.Status > 0 && result.Data != null)
            {
                grdProduct.ItemsSource = result.Data as List<Product>;
            }
            else
            {
                grdProduct.ItemsSource = new List<Product>();
            }
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            int idTmp = -1;
            int.TryParse(txtProductId.Text, out idTmp);
            try
            {

                int status = 0;
                if (chkIsActive.IsChecked == true) { status = 1; }
                else { status = 0; }

                var expireDate = DateTime.Parse(dpProductExpireDate.Text);
                var manufacturingDate = DateTime.Parse(dpProductManufacturingDate.Text);
                if (expireDate < manufacturingDate)
                {
                    MessageBox.Show("Expire date must be greater than manufacturing date", "Error");
                    return;
                }

                if (idTmp == 0)
                {
                    var product = new Product()
                    {
                        ProductName = txtProductName.Text,
                        Status = status,
                        ExpireDate = expireDate,
                        ManufacturingDate = manufacturingDate,
                        Quantity = int.Parse(txtProductQuantity.Text),
                        Price = double.Parse(txtProductPrice.Text),
                        Description = txtProductDescription.Text,
                        BrandId = int.Parse(txtProductBrand.Text),
                        CategoryId = int.Parse(txtProductCategory.Text)
                    };

                    var result = await _business.CreateProduct(product);
                    MessageBox.Show(result.Message, "Save");
                }
                else
                {
                    var item = await _business.GetProductByIdWithNestedObjAsync(idTmp);
                    var product = item.Data as Product;
                    product.ProductName = txtProductName.Text;
                    product.Status = status;
                    product.ExpireDate = expireDate;
                    product.ManufacturingDate = manufacturingDate;
                    product.Quantity = int.Parse(txtProductQuantity.Text);
                    product.Price = double.Parse(txtProductPrice.Text);
                    product.Description = txtProductDescription.Text;
                    product.BrandId = int.Parse(txtProductBrand.Text);
                    product.CategoryId = int.Parse(txtProductCategory.Text);

                    var result = await _business.UpdateProduct(product);
                    MessageBox.Show(result.Message, "Update");
                }

                txtProductId.Text = "";
                txtProductName.Text = "";
                chkIsActive.IsChecked = false;
                dpProductExpireDate.Text = "";
                dpProductManufacturingDate.Text = "";
                txtProductQuantity.Text = "";
                txtProductPrice.Text = "";
                txtProductDescription.Text = "";
                txtProductBrand.Text = "";
                txtProductCategory.Text = "";

                this.LoadGrdProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to exit?", "Exit", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.Hide();
            }
        }

        private async void grdProduct_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            string productId = btn.CommandParameter.ToString();

            //MessageBox.Show(currencyCode);

            if (!string.IsNullOrEmpty(productId))
            {
                if (MessageBox.Show("Do you want to delete this item?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var result = await _business.DeleteProduct(int.Parse(productId));
                    MessageBox.Show($"{result.Message}", "Delete");
                    this.LoadGrdProducts();
                }
            }
        }

        private async void grdProduct_MouseDouble_Click(object sender, RoutedEventArgs e)
        {
            DataGrid grd = sender as DataGrid;
            if (grd != null && grd.SelectedItems != null && grd.SelectedItems.Count == 1)
            {
                var row = grd.ItemContainerGenerator.ContainerFromItem(grd.SelectedItem) as DataGridRow;
                if (row != null)
                {
                    var item = row.Item as Product;
                    if (item != null)
                    {
                        var productResult = await _business.GetProductByIdWithNestedObjAsync(item.ProductId);

                        if (productResult.Data != null)
                        {
                            item = productResult.Data as Product;
                            txtProductId.Text = item.ProductId.ToString();
                            txtProductName.Text = item.ProductName;
                            dpProductExpireDate.Text = item.ExpireDate.ToString();
                            dpProductManufacturingDate.Text = item.ManufacturingDate.ToString();
                            txtProductQuantity.Text = item.Quantity.ToString();
                            txtProductBrand.Text = item.BrandId.ToString();
                            txtProductCategory.Text = item.CategoryId.ToString();
                            txtProductPrice.Text = item.Price.ToString();
                            txtProductDescription.Text = item.Description;
                            chkIsActive.IsChecked = Convert.ToBoolean(item.Status);
                        }
                    }
                }
            }
        }


    }
}
