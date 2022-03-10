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
using DiningRoomBusinessLogic.BusinessLogics;
using DiningRoomDatabaseImplement.Implements;
using DiningRoomContracts.BindingModels;
using DiningRoomContracts.ViewModels;

namespace DiningRoomHornsHooves
{
    /// <summary>
    /// Логика взаимодействия для LunchWindow.xaml
    /// </summary>
    public partial class LunchWindow : Window
    {
        LunchLogic lunchLogic = new LunchLogic(new LunchStorage(), new OrderStorage());
        ProductLogic productLogic = new ProductLogic(new ProductStorage(), new CookStorage());
        public int Id { set { id = value; } }
        private int? id;
        public LunchWindow()
        {
            InitializeComponent();
        }
        private void LunchWindowLoad(object sender, RoutedEventArgs e)
        {
            var list = productLogic.Read(null);
            ReadyListBox.ItemsSource = list;
            ReadyListBox.SelectedItem = null;
            //OrdersComboBox.DisplayMemberPath = "Calorie";
            if (id.HasValue)
            {
                try
                {
                    var view = lunchLogic.Read(new LunchBindingModel { Id = id })?[0];
                    if (view != null)
                    {
                        PriceBox.Text = view.Price.ToString();
                        WeightBox.Text = view.Weight.ToString();
                        DatePicker.SelectedDate = view.Date;
                        var selectedList = productLogic.Read(null).Where(rec => view.LunchProduts.ContainsKey(rec.Id));
                        if (view.LunchProduts.Count != 0)
                        {
                            CountBox.Text = view.LunchProduts.First().Value.ToString();
                        }
                        SelectedListBox.ItemsSource = selectedList;
                        foreach(var i in selectedList)
                        {
                            ReadyListBox.Items.Remove(i);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void CreateClick(object sender, RoutedEventArgs e)
        {
            if (PriceBox.Text == "")
            {
                MessageBox.Show("Введите цену");
                return;
            }
            if (WeightBox.Text == "")
            {
                MessageBox.Show("Введите вес");
                return;
            }
            if (DatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату");
                return;
            }
            if (SelectedListBox.Items.Count == 0)
            {
                MessageBox.Show("Выберите продукты");
                return;
            }
            int price = Convert.ToInt32(PriceBox.Text);
            int weight = Convert.ToInt32(WeightBox.Text);
            int count = Convert.ToInt32(CountBox.Text);
            DateTime date = (DateTime)DatePicker.SelectedDate;
            Dictionary<int, int> lunchProducts = new Dictionary<int, int>();
            foreach(ProductViewModel i in SelectedListBox.Items)
            {
                lunchProducts.Add(i.Id, count);
            }
            lunchLogic.CreateOrUpdate(new LunchBindingModel
            {
                Id = id,
                Price = price,
                Weight = weight,
                Date = date,
                LunchProduts = lunchProducts
            });
            this.Close();
        }
    }
}
