using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaomiAI.Chat.Shared.Helpers;

public static class ChatKeyHelper
{
    public static string GetChatKey(Guid chatId)
    {
        return $"chat:{chatId}";
    }
}
