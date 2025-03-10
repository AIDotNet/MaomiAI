// <copyright file="Mediator.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace MaomiAI.User.Core;

/// <summary>
/// 中介者实现.
/// </summary>
public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="serviceProvider">服务提供者.</param>
    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        
        var handler = _serviceProvider.GetService(handlerType) 
            ?? throw new InvalidOperationException($"No handler registered for {requestType.Name}");
        
        var method = handlerType.GetMethod("Handle") 
            ?? throw new InvalidOperationException($"Handle method not found on {handlerType.Name}");
        
        return await (Task<TResponse>)method.Invoke(handler, new object[] { request, cancellationToken })!;
    }

    /// <inheritdoc/>
    public async Task Send(IRequest request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);
        
        var handler = _serviceProvider.GetService(handlerType) 
            ?? throw new InvalidOperationException($"No handler registered for {requestType.Name}");
        
        var method = handlerType.GetMethod("Handle") 
            ?? throw new InvalidOperationException($"Handle method not found on {handlerType.Name}");
        
        await (Task)method.Invoke(handler, new object[] { request, cancellationToken })!;
    }

    /// <inheritdoc/>
    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        var notificationType = notification.GetType();
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notificationType);
        
        var handlers = _serviceProvider.GetServices(handlerType);
        var tasks = new List<Task>();
        
        foreach (var handler in handlers)
        {
            var method = handlerType.GetMethod("Handle") 
                ?? throw new InvalidOperationException($"Handle method not found on {handlerType.Name}");
            
            tasks.Add((Task)method.Invoke(handler, new object[] { notification, cancellationToken })!);
        }
        
        await Task.WhenAll(tasks);
    }
} 