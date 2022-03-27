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
using DiningRoomContracts.BusinessLogicsContracts;

namespace DiningRoomHornsHooves
{
    /// <summary>
    /// Логика взаимодействия для ReportLunchesWindow.xaml
    /// </summary>
    public partial class ReportLunchesWindow : Window
    {
        private readonly IReportLogic reportLogic;
        public ReportLunchesWindow(IReportLogic reportLogic)
        {
            InitializeComponent();
            this.reportLogic = reportLogic;
            DataGridTextColumn textColumnDate = new DataGridTextColumn();
            textColumnDate.Header = "Дата создания";
            textColumnDate.Binding = new Binding("dateCreate");
            DataGrid.Columns.Add(textColumnDate);
            DataGridTextColumn textColumnWeight = new DataGridTextColumn();
            textColumnWeight.Header = "Вес";
            textColumnWeight.Binding = new Binding("weight");
            DataGrid.Columns.Add(textColumnWeight);
            DataGridTextColumn textColumnPrice = new DataGridTextColumn();
            textColumnPrice.Header = "Цена";
            textColumnPrice.Binding = new Binding("price");
            DataGrid.Columns.Add(textColumnPrice);
            DataGridTextColumn textColumnCutleryName = new DataGridTextColumn();
            textColumnCutleryName.Header = "Название прибора";
            textColumnCutleryName.Binding = new Binding("cutleryName");
            DataGrid.Columns.Add(textColumnCutleryName);
            DataGridTextColumn textColumnCutleryCount = new DataGridTextColumn();
            textColumnCutleryCount.Header = "Количество приборов";
            textColumnCutleryCount.Binding = new Binding("cutleryCount");
            DataGrid.Columns.Add(textColumnCutleryCount);
            DataGridTextColumn textColumnCookName = new DataGridTextColumn();
            textColumnCookName.Header = "Имя повара";
            textColumnCookName.Binding = new Binding("cookName");
            DataGrid.Columns.Add(textColumnCookName);
        }
        private class itemLucnh
        {
            public string dateCreate { get; set; }
            public string weight { get; set; }
            public string price { get; set; }
            public string cutleryName { get; set; }
            public string cutleryCount { get; set; }
            public string cookName { get; set; }
        }
        private void ShowClick(object sender, RoutedEventArgs e)
        {
            if (DatePickerAfter.SelectedDate == null || DatePickerBefore.SelectedDate == null ||
                DatePickerAfter.SelectedDate >= DatePickerBefore.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка");
                return;
            }
            try
            {
                var dict = reportLogic.GetLunchesPCView((DateTime)DatePickerAfter.SelectedDate, (DateTime)DatePickerBefore.SelectedDate);
                if (dict != null)
                {
                    DataGrid.Items.Clear();
                    foreach (var elem in dict)
                    {
                        DataGrid.Items.Add(new itemLucnh() { 
                        dateCreate = elem.DateCreate.ToShortDateString(),
                        weight = elem.Weight.ToString(),
                        price = elem.price.ToString()});
                        foreach(var cook in elem.Cooks)
                        {
                            DataGrid.Items.Add(new itemLucnh()
                            {
                                cookName = cook.Name
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
    }
}
