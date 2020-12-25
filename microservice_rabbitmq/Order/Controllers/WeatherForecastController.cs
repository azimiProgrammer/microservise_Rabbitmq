using EventBusEvent;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly ILogger<OrderController> _logger;
        private readonly EventBusRabbitMQProducer _eventBus;

        public OrderController(ILogger<OrderController> logger, EventBusRabbitMQProducer eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        [HttpPost]
        public async Task<ActionResult<long>> Post()
        {
            var result = await Task.Run(() =>
            {
                _eventBus.Publish<RegisterOrderEvent>(EventBusConstansts.RegisterOrderQueue, new RegisterOrderEvent
                {
                    Id = Guid.NewGuid(),
                    OrderDate = DateTime.Now
                });

                return 1l;
            });

            return Ok(result);
        }
    }

    



}
