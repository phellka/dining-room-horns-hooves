using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiningRoomContracts.BusinessLogicsContracts;
using DiningRoomContracts.ViewModels;
using DiningRoomContracts.StoragesContracts;
using DiningRoomContracts.BindingModels;

namespace DiningRoomBusinessLogic.BusinessLogics
{
    public class ReportLogic : IReportLogic
    {
        private readonly ILunchStorage lunchStorage;
        private readonly ICutleryStorage cutleryStorage;
        private readonly ICookStorage cookStorage;
        private readonly IProductStorage productStorage;
        public ReportLogic(ILunchStorage lunchStorage, ICutleryStorage cutleryStorage, ICookStorage cookStorage, IProductStorage productStorage)
        {
            this.cookStorage = cookStorage;
            this.cutleryStorage = cutleryStorage;
            this.lunchStorage = lunchStorage;
            this.productStorage = productStorage;
        }
        public List<ReportLunchesPCView> GetLunchesPCView(DateTime after, DateTime before)
        {
            var list = new List<ReportLunchesPCView>();
            var lunches = lunchStorage.GetFilteredList(new LunchBindingModel
            {
                after = after,
                before = before
            });
            foreach (var lunch in lunches)
            {
                var record = new ReportLunchesPCView
                {
                    DateCreate = lunch.Date,
                    price = lunch.Price,
                    Weight = lunch.Weight,
                    Cooks = new List<CookViewModel>(),
                    Cutleries = new List<CutleryViewModel>()
                };
                var lunchProducts = lunch.LunchProduts.Keys.ToList().Select(rec => productStorage.GetElement(new ProductBindingModel { Id = rec }));
                var listCookIds = new List<int>();
                foreach (var elem in lunchProducts)
                {
                    listCookIds.AddRange(elem.ProductCooks.Keys.ToList());
                }
                record.Cooks = listCookIds.Distinct().ToList().Select(rec => cookStorage.GetElement(new CookBindingModel { Id = rec })).ToList();
                list.Add(record);
            }
            return list;
        }
    }
}   
