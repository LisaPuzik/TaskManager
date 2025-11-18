using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Kanban.Entities;
using Task = Kanban.Entities.Task;

namespace Kanban.BL
{
    public class SimpleConfigModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository<Task>>().To<DapperRepository>().InSingletonScope();

            Bind<ILogicCrud>().To<LogicCrud>();
            Bind<ILogicBL>().To<LogicBL>();
            Bind<ILogicAll>().To<LogicAll>();
        }
    }
}
