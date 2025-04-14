using System.Diagnostics;

namespace MaomiAI.Infra.Diagnostics;

public static class ActivityHelper
{
    public static readonly ActivitySource ActivitySource = new ActivitySource("MaomiAI");
}
