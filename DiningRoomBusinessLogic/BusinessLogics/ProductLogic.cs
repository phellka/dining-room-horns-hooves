using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiningRoomContracts.ViewModels;
using DiningRoomContracts.StoragesContracts;
using DiningRoomContracts.BusinessLogicsContracts;
using DiningRoomContracts.BindingModels;

namespace DiningRoomBusinessLogic.BusinessLogics
{
    public class ProductLogic : IProductLogic
    {
        private readonly IProductStorage productStorage;
        private readonly ICookStorage cookStorage;
        public ProductLogic(IProductStorage productStorage, ICookStorage cookStorage)
        {
            this.productStorage = productStorage;
            this.cookStorage = cookStorage;
        }
        public List<ProductViewModel> Read(ProductBindingModel model)
        {
            if (model == null)
            {
                return productStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<ProductViewModel> { productStorage.GetElement(model) };
            }
            return productStorage.GetFilteredList(model);
        }
        public void Create(ProductBindingModel model)
        {
            productStorage.Insert(model);
        }
        public void AddCooks((int, List<int>) addedCooks)
        {
            var product = productStorage.GetElement(new ProductBindingModel { Id = addedCooks.Item1 });
            if (product == null)
            {
                throw new Exception("Продукт не найден");
            }
            productStorage.AddCooks(addedCooks);
        }
    }
}
