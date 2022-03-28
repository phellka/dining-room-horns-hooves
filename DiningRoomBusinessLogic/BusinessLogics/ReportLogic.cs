using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiningRoomContracts.BusinessLogicsContracts;
using DiningRoomContracts.ViewModels;
using DiningRoomContracts.StoragesContracts;
using DiningRoomContracts.BindingModels;
using DiningRoomBusinessLogic.OfficePackage;
using DiningRoomBusinessLogic.OfficePackage.HelperEnums;
using DiningRoomBusinessLogic.OfficePackage.HelperModels;

namespace DiningRoomBusinessLogic.BusinessLogics
{
    public class ReportLogic : IReportLogic
    {
        private readonly ILunchStorage lunchStorage;
        private readonly ICutleryStorage cutleryStorage;
        private readonly ICookStorage cookStorage;
        private readonly IProductStorage productStorage;
        private readonly AbstractSaveToPdf saveToPdf;
        public ReportLogic(ILunchStorage lunchStorage, ICutleryStorage cutleryStorage, ICookStorage cookStorage, IProductStorage productStorage,
            AbstractSaveToPdf saveToPdf)
        {
            this.cookStorage = cookStorage;
            this.cutleryStorage = cutleryStorage;
            this.lunchStorage = lunchStorage;
            this.productStorage = productStorage;
            this.saveToPdf = saveToPdf;
        }
        public List<ReportLunchesPCView> GetLunchesPCView(ReportBindingModel model)
        {
            var list = new List<ReportLunchesPCView>();
            var lunches = lunchStorage.GetFilteredList(new LunchBindingModel
            {
                after = model.DateAfter,
                before = model.DateBefore
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
                var lunchOrders = lunch.LunchOrders.Keys.ToList();
                var lunchCutleries = cutleryStorage.GetFullList().Where(rec => lunchOrders.Contains(rec.CulteryOrder)).ToList();
                record.Cutleries = lunchCutleries;
                list.Add(record);
            }
            return list;
        }

        public void saveLunchesToPdfFile(ReportBindingModel model)
        {
            saveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список заказов",
                DateAfter = model.DateAfter.Value,
                DateBefore = model.DateBefore.Value,
                Lunches = GetLunchesPCView(model)
            });
        }
    }
}   
