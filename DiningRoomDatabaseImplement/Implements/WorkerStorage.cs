using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiningRoomDatabaseImplement.Models;
using DiningRoomContracts.BindingModels;
using DiningRoomContracts.StoragesContracts;
using DiningRoomContracts.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DiningRoomDatabaseImplement.Implements
{
    public class WorkerStorage : IWorkerStorage
    {
        public static string AutorizedWorker;
        public void Insert(WorkerBindingModel model)
        {
            using var context = new DiningRoomDatabase();
            if (context.Workers.FirstOrDefault(rec => rec.Login == model.Login) != null)
            {
                throw new Exception("Пользователь с таким логином уже существует");
            }
            context.Workers.Add(new Worker { Login = model.Login, Password = model.Password});
            context.SaveChanges();
        }
        public bool Login(WorkerBindingModel model)
        {
            using var context = new DiningRoomDatabase();
            if (!context.Workers.Contains(new Worker { Login = model.Login, Password = model.Password })) { 
                return false;
            }
            if (!context.Workers.Select(rec => rec.Login).Contains(model.Login))
            {
                return false;
            }
            if (context.Workers.FirstOrDefault(rec => rec.Login == model.Login).Password != model.Password)
            {
                return false;
            }
            AutorizedWorker = model.Login;
            return true;
        }
    }
}
