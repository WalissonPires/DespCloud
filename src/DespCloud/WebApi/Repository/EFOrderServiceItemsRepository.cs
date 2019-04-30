using Base.Domain.Contracts;
using Base.Domain.Entities;
using Base.WebApp.Contracts.Network;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database;
using WebApi.Database.Entities;

namespace WebApi.Repository
{
    public class EFOrderServiceItemsRepository : EFRepositoryBase
    {
        private const string CONTEXT_GENERETOR_ID = "OrderServiceItems";                

        private IQueryable<OrderDetailEty> OrdersNoTracking => _context.OrderDetails.AsNoTracking()
                .Include(x => x.Service)
                .Include(x => x.Vehicle);

        public EFOrderServiceItemsRepository(UnitWork unitWork) : base(unitWork)
        {
        }

        public OrderDetail GetById(int id)
        {
            var item = OrdersNoTracking.Where(x => x.CompanyId == _info.CompanyId && x.Id == id).SingleOrDefault();

            if (item == null)
                throw new AppValidationException("Item da ordem de serviço não encontrado");

            return _unitWork.Mapper.Map<OrderDetail>(item);
        }

        public List<OrderDetail> Get(FilterParams filterParams)
        {
            var items = OrdersNoTracking.Where(x => x.CompanyId  == _info.CompanyId).Select(x => _unitWork.Mapper.Map<OrderDetail>(x)).ToList();

            return items;
        }

        public OrderDetail Create(OrderDetail orderItem)
        {
            var dbOrder = _context.Orders.Where(x => x.CompanyId == _info.CompanyId && x.Id == orderItem.OrderId).SingleOrDefault();
            if (dbOrder == null)
                throw new AppValidationException($"Ordem de serviço nº ${orderItem.OrderId} não encontrada");

            if (dbOrder.Status != OrderStatusEty.Opened)
                throw new AppValidationException($"Não é possível adicionar itens a uma ordem que não esteja aberta");

            if (!_unitWork.HasTransaction())
                throw new Exception("A transaction must be started");

            var dbOrderItem = _unitWork.Mapper.Map<OrderDetailEty>(orderItem);
            dbOrderItem.Id = _unitWork.GenereteId(CONTEXT_GENERETOR_ID);
            dbOrderItem.CompanyId = _info.CompanyId;
            dbOrderItem.Total = dbOrderItem.Honorary + dbOrderItem.Rate + dbOrderItem.PlateCard + dbOrderItem.Other;
            dbOrderItem.Service = null;            
            //dbOrderItem.CreateAt = DateTimeOffset.Now;

            _context.OrderDetails.Add(dbOrderItem);
            _context.SaveChanges();

            dbOrder.Total = _context.OrderDetails.Where(x => x.CompanyId == _info.CompanyId && x.OrderId == dbOrder.Id).Sum(x => x.Total);
            _context.SaveChanges();

            dbOrderItem = OrdersNoTracking.Where(x => x.CompanyId == _info.CompanyId && x.Id == dbOrderItem.Id).Single();

            return _unitWork.Mapper.Map<OrderDetail>(dbOrderItem);
        }

        public OrderDetail Delete(int id)
        {
            var dbOrderItem = _context.OrderDetails.Include(x => x.Order).Where(x => x.CompanyId == _info.CompanyId && x.Id == id).SingleOrDefault();
            if (dbOrderItem == null)
                throw new AppValidationException("Item da ordem de serviço não encontrado");
            
            if (dbOrderItem.Order.Status != OrderStatusEty.Opened)
                throw new AppValidationException($"Não é possível adicionar itens a uma ordem que não esteja aberta");

            if(!_unitWork.HasTransaction())
                throw new Exception("A transaction must be started");

            _context.OrderDetails.Remove(dbOrderItem);
            _context.SaveChanges();

            dbOrderItem.Order.Total = _context.OrderDetails.Where(x => x.CompanyId == _info.CompanyId && x.OrderId == dbOrderItem.Order.Id).Sum(x => x.Total);
            _context.SaveChanges();

            return _unitWork.Mapper.Map<OrderDetail>(dbOrderItem);
        }

        public OrderDetail Update(OrderDetail orderItem)
        {
            var dbOrder = _context.Orders.Where(x => x.CompanyId == _info.CompanyId && x.Id == orderItem.OrderId).SingleOrDefault();
            if (dbOrder == null)
                throw new AppValidationException($"Ordem de serviço nº ${orderItem.OrderId} não encontrada");

            if (dbOrder.Status != OrderStatusEty.Opened)
                throw new AppValidationException($"Não é possível adicionar itens a uma ordem que não esteja aberta");

            if (!_unitWork.HasTransaction())
                throw new Exception("A transaction must be started");

            var dbOrderItem = _context.OrderDetails.Where(x => x.CompanyId == _info.CompanyId && x.Id == orderItem.Id).SingleOrDefault();
            if (dbOrderItem == null)
                throw new AppValidationException("Item da ordem de serviço não encontrado");

            var _OrderItem = _unitWork.Mapper.Map<OrderDetailEty>(orderItem);

            dbOrderItem.Honorary = _OrderItem.Honorary;
            dbOrderItem.Rate = _OrderItem.Rate;
            dbOrderItem.PlateCard = _OrderItem.PlateCard;
            dbOrderItem.Other = _OrderItem.Other;
            dbOrderItem.Total = _OrderItem.Honorary + _OrderItem.Rate + _OrderItem.PlateCard + _OrderItem.Other;                        
            _context.SaveChanges();

            dbOrder.Total = _context.OrderDetails.Where(x => x.CompanyId == _info.CompanyId && x.OrderId == dbOrder.Id).Sum(x => x.Total);
            _context.SaveChanges();

            _OrderItem = OrdersNoTracking.Where(x => x.CompanyId == _info.CompanyId && x.Id == _OrderItem.Id).Single();

            return _unitWork.Mapper.Map<OrderDetail>(_OrderItem);
        }
    }
}
