// <copyright file="Constant.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Infra
{
    /// <summary>
    /// 常量.
    /// </summary>
    public static class Constant
    {
        /// <summary>
        /// 不在业务逻辑中发生的日志，统一使用这个名称做日志命名.<br />
        /// 例如在模块类中的打印的日志.
        /// </summary>
        public const string BaseLoggerName = "MaomiAI";
    }
}