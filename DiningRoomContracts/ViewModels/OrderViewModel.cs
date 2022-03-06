using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DiningRoomContracts.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        [DisplayName("Каллорийность заказа")]
        public int Calorie { get; set; }
        [DisplayName("Пожелания к заказу")]
        public String Wishes { get; set; }
        public string WorkerLogin { get; set; }
    }
}
