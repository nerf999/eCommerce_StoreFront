using eCommerce.MAUI.ViewModels;
using Microsoft.Maui.Storage;
using Amazon.Library.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using eCommerce.Library.DTO;
using Amazon.Library.Services;

namespace eCommerce.MAUI.Views
{
    public partial class InventoryView : ContentPage
    {
        public InventoryView()
        {
            InitializeComponent();
            BindingContext = new InventoryViewModel();
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//MainPage");
        }

        private void AddClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Product");
        }

        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            (BindingContext as InventoryViewModel)?.Refresh();
        }

        private void EditClicked(object sender, EventArgs e)
        {
            (BindingContext as InventoryViewModel)?.Edit();
        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            (BindingContext as InventoryViewModel)?.Delete();
        }

        private void SearchClicked(object sender, EventArgs e)
        {
            (BindingContext as InventoryViewModel)?.Search();
        }

        private void OnReadFileButtonClicked(object sender, EventArgs e)
        {
            ReadAndProcessFile();
        }

        public void ReadAndProcessFile()
        {
            
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Productfile.csv");

            if (File.Exists(filePath))
            {
                try
                {
                    var fileContent = File.ReadAllText(filePath);
                    ProcessCSV(fileContent);
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"File reading failed: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
        }

        private async void ProcessCSV(string fileContent)
        {
            
            var lines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var columns = line.Split(',');
                
                var name = columns[0].Trim();
                var description = columns[1].Trim();
                var price = decimal.Parse(columns[2].Trim());
                var quantity = int.Parse(columns[3].Trim());

                
                var product = new Product
                {
                    Name = name,
                    Description = description,
                    Price = price,
                    Quantity = quantity
                };

                var productDTO = new ProductDTO(product);

                
                var addedOrUpdatedProduct = await (BindingContext as InventoryViewModel)?.AddOrUpdate(productDTO);

                
            }
        }
    }
}
