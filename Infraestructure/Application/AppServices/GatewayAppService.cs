﻿using Application.Dtos;
using Application.IAppServices;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Domain.IRepositories;
using Domain.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Specifications;
using Application.IValidator;
using Application.Exceptions;

namespace Infrastructure.Application.AppServices
{
    public partial class GatewayAppService : IGatewayAppService
    {
        private readonly IGatewayRepository _GatewayRepository;
        private readonly IMapper _mapper;
        private readonly IEntityValidator _entityValidator;

        public GatewayAppService(IGatewayRepository GatewayRepository, IMapper mapper, IEntityValidator entityValidator)
        {
            _GatewayRepository = GatewayRepository ?? throw new ArgumentNullException(nameof(GatewayRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _entityValidator = entityValidator ?? throw new ArgumentNullException(nameof(entityValidator));
        }

        public async Task<bool> AddAsync(GatewayDto item, CancellationToken cancellationToken = default)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _GatewayRepository.AddAsync(_mapper.Map<Gateway>(item), cancellationToken);
                commited = await _GatewayRepository.UnitOfWork.CommitAsync(cancellationToken);
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }

        public async Task<List<GatewayDto>> FindAllBySpecificationPatternAsync(Specification<GatewayDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null, CancellationToken cancellationToken = default)
        {
            return _mapper.Map<List<GatewayDto>>(
                await _GatewayRepository.FindAllByExpressionAsync(_mapper.MapExpression<Expression<Func<Gateway, bool>>>(
                        specification == null ? a => true : specification.ToExpression()), includes, order, cancellationToken));
        }

        public async Task<int> FindCountBySpecificationPatternAsync(Specification<GatewayDto>? specification = null, CancellationToken cancellationToken = default)
        {
            var count = await _GatewayRepository.FindCountByExpressionAsync(specification == null ? a => true : specification.MapToExpressionOfType<Gateway>(), cancellationToken);
            return count;
        }

        public async Task<GatewayDto?> FindOneBySpecificationPatternAsync(Specification<GatewayDto>? specification = null, List<string>? includes = null, CancellationToken cancellationToken = default)
        {
            var item = await _GatewayRepository.FindOneByExpressionAsync(specification == null ? a => true : specification.MapToExpressionOfType<Gateway>(), includes, cancellationToken);
            return _mapper.Map<GatewayDto>(item);
        }

        public async Task<List<GatewayDto>> FindPageBySpecificationPatternAsync(Specification<GatewayDto>? specification = null, List<string>? includes = null, Dictionary<string, bool>? order = null, int pageSize = 0, int pageGo = 0, CancellationToken cancellationToken = default)
        {
            return _mapper.Map<List<GatewayDto>>(
                await _GatewayRepository.FindPageByExpressionAsync(specification == null ? a => true : specification.MapToExpressionOfType<Gateway>(), includes ?? new List<string>(), order ?? new Dictionary<string, bool>(), pageSize, pageGo, cancellationToken));
        }


        public GatewayDto Get(Guid id, List<string>? includes = null)
        {
            return _mapper.Map<GatewayDto>(_GatewayRepository.Get(id, includes));
        }

        public async Task<List<GatewayDto>> GetAllAsync(List<string>? includes = null, Dictionary<string, bool>? order = null, CancellationToken cancellationToken = default)
        {
            var items = await _GatewayRepository.GetAllAsync(includes, order, cancellationToken);
            var dtoItems = _mapper.Map<List<GatewayDto>>(items.ToList());
            return dtoItems;
        }

        public async Task<GatewayDto> GetAsync(Guid id, List<string>? includes = null, CancellationToken cancellationToken = default)
        {
            return _mapper.Map<GatewayDto>(await _GatewayRepository.GetAsync(id, includes, cancellationToken));
        }

        public async Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var item = await _GatewayRepository.GetAsync(id, cancellationToken: cancellationToken);
            if (item == null)
                throw new ArgumentException("Itme does not exist");
            await _GatewayRepository.DeleteAsync(item, cancellationToken);
            var commited = await _GatewayRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            return commited > 0;
        }

        public async Task<bool> UpdateAsync(GatewayDto item, CancellationToken cancellationToken = default)
        {
            int commited;
            if (_entityValidator.IsValid(item))
            {
                await _GatewayRepository.UpdateAsync(_mapper.Map<Gateway>(item), cancellationToken);
                commited = await _GatewayRepository.UnitOfWork.CommitAsync(cancellationToken);
            }
            else
                throw new ApplicationValidationErrorsException(_entityValidator.GetInvalidMessages(item));
            return commited > 0;
        }

    }
}
