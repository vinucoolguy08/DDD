using Maersk.SCM.DeliveryPlanning.Application.Commands;
using Maersk.SCM.DeliveryPlanning.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maersk.SCM.DeliveryPlanning.API.Controllers
{
    [Route("api/delivery-plans")]
    [ApiController]
    public class DeliveryPlansController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IDeliveryPlanQueries _deliveryPlanQueries;

        public DeliveryPlansController(IMediator mediator, IDeliveryPlanQueries deliveryPlanQueries)
        {
            _mediator = mediator;
            _deliveryPlanQueries = deliveryPlanQueries;
        }

        [HttpPost()]
        public async Task<IActionResult> PostAsync([FromBody] CreateNewDeliveryPlanCommand command)
        {
            var deliveryPlanId = await _mediator.Send(command);

            return CreatedAtAction("Get", new { deliveryPlanId }, command);
        }

        [HttpGet("{deliveryPlanId}")]
        public async Task<IActionResult> GetAsync(Guid deliveryPlanId)
        {
            var deliveryPlan = await _deliveryPlanQueries.GetDeliveryPlanAsync(deliveryPlanId);

            if (deliveryPlan == null)
            {
                return NotFound();
            }

            return Ok(deliveryPlan);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] DeliveryPlanFilter filter)
        {
            var result = await _deliveryPlanQueries.GetDeliveryPlansSummaryAsync(filter);

            return Ok(result);
        }

        [HttpPut("{deliveryPlanId}")]
        public async Task<IActionResult> UpdateAsync(Guid deliveryPlanId, UpdateDeliveryPlanCommand command)
        {
            command.DeliveryPlanId = deliveryPlanId;
            await _mediator.Send(command);

            return Ok();
        }
    }
}
