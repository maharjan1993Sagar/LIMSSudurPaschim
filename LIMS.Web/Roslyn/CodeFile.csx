#! "netcoreapp3.1"
#r "LIMS.Core"
#r "LIMS.Framework"
#r "LIMS.Services"
#r "LIMS.Web"

using System;
using LIMS.Domain.Messages;
using LIMS.Domain.Orders;
using LIMS.Services.Events;
using LIMS.Services.Notifications.Messages;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LIMS.Core;

/* Sample code to add new token message (message email) to the order */

public class OrderTokenTest : INotificationHandler<EntityTokensAddedEvent<Order>>
{
    public Task Handle(EntityTokensAddedEvent<Order> eventMessage, CancellationToken cancellationToken)
    {
        //in message templates you can put new token {{AdditionalTokens["NewOrderNumber"]}}
        eventMessage.LiquidObject.AdditionalTokens.Add("NewOrderNumber", $"{eventMessage.Entity.CreatedOnUtc.Year}/{eventMessage.Entity.OrderNumber}");
        return Task.CompletedTask;
    }

}



