﻿using Application.Constants;
using Application.Dtos;
using Application.IAppServices;
using Domain.Interfaces;
using Domain.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace WebApi.Controllers
{
    
    [ApiController]
    [Route("api/gateway")]
    public class GatewayController : ApiBaseController<GatewayDto, Guid>
    {
        
        public GatewayController(IGatewayAppService appService, ILogger<GatewayController> logger, IPropertyCheckerService propertyCheckerService) 
            : base(appService, logger, propertyCheckerService)
        {
            //Includes = new () { nameof(GatewayDto.Peripherals), nameof(GatewayDto.Brand) };
        }

        [HttpGet("{gatewayId}/validation-errors")]
        public async Task<ActionResult<List<string>>> ValidationErrors(Guid? gatewayId, CancellationToken cancellationToken)
        {
            var validationErrors = new List<string>();

            if (gatewayId == null || gatewayId.Value == Guid.Empty)
            {
                validationErrors.Add("gateway id is not valid");
            }
            else
            {
                var gateway = await AppService.GetAsync(gatewayId.Value, Includes,cancellationToken);

                if (gateway == null)
                    validationErrors.Add("gateway does not exist");
                else
                {
                    if (gateway.Peripherals.Count >= 10)
                        validationErrors.Add(string.Format(Resource.validation_MaxPeriphelsAllowed, GatewayPeripherals.MAX_PERIPHERALS_ALLOWED_PER_GATEWAY));
                }

            }

            return validationErrors;
        }
    }
    
}
