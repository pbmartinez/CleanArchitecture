﻿
using Domain.Entities;
using Domain.IRepositories;
using Domain.UnitOfWork;

namespace Infrastructure.Domain.Repositories
{
    public partial class GatewayRepository : Repository<Gateway>, IGatewayRepository
    {
        public GatewayRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
