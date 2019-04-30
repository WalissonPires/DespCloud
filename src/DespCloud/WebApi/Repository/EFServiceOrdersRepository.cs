using Base.Domain.Contracts;
using Base.Domain.Entities;
using Base.WebApp.Contracts.Network;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database.Entities;

namespace WebApi.Repository
{
    public class EFServiceOrdersRepository : EFRepositoryBase
    {
        private const string CONTEXT_GENERETOR_ID = "ServiceOrders";
        
        public IQueryable<OrderServiceEty> OrdersNoTracking => _unitWork.Context.Orders.AsNoTracking()
                .Include(x => x.Client)
                .Include(x => x.Items)
                .Include("Items.Service")
                .Include("Items.Vehicle");


        public EFServiceOrdersRepository(UnitWork unitWork) : base(unitWork)
        {
        }



        public OrderService GetById(int id)
        {
            var order = OrdersNoTracking.Where(x => x.CompanyId == _info.CompanyId && x.Id == id).SingleOrDefault();

            return _unitWork.Mapper.Map<OrderService>(order);
        }

        public List<OrderService> Get(FilterParams filterParams)
        {
            var orders = OrdersNoTracking.Where(x => x.CompanyId == _info.CompanyId).Select(x => _unitWork.Mapper.Map<OrderService>(x)).ToList();

            return orders;
        }

        public OrderService Create(OrderService orderService)
        {
            var dbOrder = _unitWork.Mapper.Map<OrderServiceEty>(orderService);
            dbOrder.Id = _unitWork.GenereteId(CONTEXT_GENERETOR_ID);
            dbOrder.CompanyId = _info.CompanyId;
            dbOrder.CreateAt = DateTimeOffset.Now;

            _unitWork.Context.Orders.Add(dbOrder);
            _unitWork.Context.SaveChanges();

            dbOrder = OrdersNoTracking.Where(x => x.CompanyId == _info.CompanyId && x.Id == dbOrder.Id).Single();

            return _unitWork.Mapper.Map<OrderService>(dbOrder);
        }

        public OrderService Update(OrderService orderService)
        {
            var _Order = _unitWork.Mapper.Map<OrderServiceEty>(orderService);
            var dbOrder = _unitWork.Context.Orders.Where(x => x.Id == _Order.Id).SingleOrDefault();            

            if (dbOrder == null)
                throw new AppValidationException("Ordem de serviço não encontrada");

            var isClosing = dbOrder.Status != OrderStatusEty.Closed && _Order.Status == OrderStatusEty.Closed;

            dbOrder.Status = _Order.Status;            

            if (isClosing)
            {
                dbOrder.ClosedAt = DateTimeOffset.Now;
            }

            _unitWork.Context.SaveChanges();

            _Order = OrdersNoTracking.Where(x => x.CompanyId == _info.CompanyId && x.Id == _Order.Id).Single();

            return _unitWork.Mapper.Map<OrderService>(_Order);
        }
    }
}
