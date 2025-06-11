using Maomi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaomiAI.Note;

[InjectModule<NoteApiModule>]
public class NoteCoreModule : IModule
{
    public void ConfigureServices(ServiceContext context)
    {
    }
}
