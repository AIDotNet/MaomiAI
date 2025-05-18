// <copyright file="RabbitMQModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi.MQ;
using Maomi.MQ.EventBus;
using Maomi.MQ.Filters;
using Maomi.MQ.MediatR;
using MaomiAI.Infra;
using RabbitMQ.Client;
using System.Reflection;

namespace MaomiAI.Modules;

public class MessageModule : IModule
{
    private readonly SystemOptions _systemOptions;

    public MessageModule(SystemOptions systemOptions)
    {
        _systemOptions = systemOptions;
    }

    public void ConfigureServices(ServiceContext context)
    {
        if (!"RabbitMQ".Equals(_systemOptions.MessageStore.Mode, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        // todo: 根据是否使用本地消息队列来决定是否使用 RabbitMQ，以便注册模块
        /*
                 context.Services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(context.Modules.Select(x => x.Assembly).Distinct().ToArray());
            options.RegisterGenericHandlers = true;
        });
         */
        context.Services.AddMaomiMQ(
            (MqOptionsBuilder options) =>
            {
                options.WorkId = 1;
                options.AutoQueueDeclare = true;
                options.AppName = "MaomiAI";
                options.Rabbit = (ConnectionFactory options) =>
                {
                    options.Uri = new Uri(_systemOptions.MessageStore.RabbitMQ!);
                    options.ConsumerDispatchConcurrency = 100;
                    options.ClientProvidedName = Assembly.GetExecutingAssembly().GetName().Name;
                };
            },
            context.Modules.Select(x => x.Assembly).ToArray(),
            [new ConsumerTypeFilter(), new EventBusTypeFilter(), new MediatrTypeFilter()]);
    }
}
