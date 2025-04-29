// <copyright file="EmbeddingCoreModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Microsoft.Extensions.Logging;

namespace MaomiAI.Infra
{
    /// <summary>
    /// InfraConfigurationModule.
    /// </summary>
    public class EmbeddingCoreModule : IModule
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfraConfigurationModule"/> class.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public EmbeddingCoreModule(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(Constant.BaseLoggerName);
        }

        /// <inheritdoc/>
        public void ConfigureServices(ServiceContext context)
        {
        }
    }
}