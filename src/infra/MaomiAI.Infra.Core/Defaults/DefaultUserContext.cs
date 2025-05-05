// <copyright file="DefaultUserContext.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Infra.Defaults;

/// <summary>
/// 默认上下文.
/// </summary>
[InjectOnScoped(InjectScheme.OnlyBaseClass)]
public class DefaultUserContext : UserContext
{
}
