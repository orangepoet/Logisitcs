using System;

using Logistics.Domain.Entities;

namespace Logistics.Domain.Repository {
    public interface IStockRep : IBaseRep<Stock> {
        dynamic GetStockView(string id);
    }
}