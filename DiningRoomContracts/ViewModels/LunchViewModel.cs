using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DiningRoomContracts.ViewModels
{
    public class LunchViewModel
    {
        public int Id { get; set; }
        [DisplayName("Стоимость обеда")]
        public int Price { get; set; }
        [DisplayName("Вес обеда")]
        public int Weight { get; set; }
        [DisplayName("Дата обеда")]
        public DateTime Date { get; set; }
        public string WorkerLogin { get; set; }
        public Dictionary<int, int> LunchOrders { get; set; }
        public Dictionary<int, int> LunchProduts { get; set; }
    }
}
