// <copyright file="UserContext.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.Net;

namespace MaomiAI.Infra.Models;

public class ClientInfo
{
    public string IP { get; set; }
    public string UserAgent { get; set; }
}
