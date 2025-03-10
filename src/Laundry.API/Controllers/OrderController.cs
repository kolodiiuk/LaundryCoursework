using AutoMapper;
using Laundry.API.Dto;
using Laundry.Domain.Contracts.Services;
using Laundry.Domain.Entities;
using Laundry.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Laundry.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrderController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<Order>>> GetAllOrdersAsync()
    {
        try
        {
            var orders = await _orderService.GetAllOrdersAsync();
            if (orders == null)
            {
                return Ok(new List<Order>());
            }

            return Ok(orders);
        }
        catch (Exception e)
        {
            return BadRequest(new ProblemDetails() { Title = $"Problem getting all orders" });
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<Order>>> GetUserOrdersAsync(int userId)
    {
        try
        {
            var orders = await _orderService.GetUserOrdersAsync(userId);
            if (orders == null)
            {
                return Ok(new List<Order>());
            }

            return Ok(orders);
        }
        catch (Exception e)
        {
            return BadRequest(new ProblemDetails() { Title = $"Problem getting user {userId} orders" });
        }
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrderAsync(int id)
    {
        try
        {
            var order = await _orderService.GetOrderAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
        catch (Exception e)
        {
            return BadRequest(new ProblemDetails() { Title = $"Problem getting an {nameof(Order)} {id}" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Order>> PlaceOrder([FromForm] Laundry.Domain.Utils.CreateOrderDto orderDto)
    {
        if (orderDto == null)
        {
            return BadRequest(new ProblemDetails() { Title = "Invalid order data" });
        }

        try
        {
            var order = await _orderService.PlaceOrderAsync(orderDto);

            return CreatedAtRoute(new { Id = order.Id }, order);
        }
        catch (Exception e)
        {
            return BadRequest(new ProblemDetails() { Title = "Problem placing an order" });
        }
    }

    [HttpPut]
    public async Task<ActionResult> UpdateOrder(Order order)
    {
        try
        {
            await _orderService.UpdateOrderAsync(order);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new ProblemDetails() { Title = $"Problem  updating an order {order.Id}" });
        }
    }

    [HttpGet("orderItems/{orderId:int}")]
    public async Task<ActionResult<List<OrderItem>>> GetOrderItems(int orderId)
    {
        if (orderId < 0)
        {
            return BadRequest(new ProblemDetails() 
                { Title = $"Invalid orderId: {orderId}" });
        }
        try
        {
            var orderItems = await _orderService.GetOrderItemsAsync(orderId);
            if (orderItems == null)
            {
                return Ok(new List<OrderItem>());
            }

            return Ok(orderItems);
        }
        catch (Exception e)
        {
            return BadRequest(new ProblemDetails()
                { Title = $"Problem getting order items for order {orderId}" });
        }
    }
}
