using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database;

namespace WebApi.Repository
{
    public abstract class EFRepositoryBase
    {       
        protected readonly UnitWork _unitWork;
        protected readonly AppDbContext _context;
        protected readonly UnitWorkInfo _info;

        public EFRepositoryBase(UnitWork unitWork)
        {
            _unitWork = unitWork;
            _context = _unitWork.Context;
            _info = _unitWork.Info;
        }
    }
}
