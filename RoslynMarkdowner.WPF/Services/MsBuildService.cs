using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Locator;

namespace RoslynMarkdowner.WPF.Services
{
    public class MsBuildService
    {
        public Dictionary<string, VisualStudioInstance> GetInstances()
            => MSBuildLocator.QueryVisualStudioInstances()
                .Select((instance, index) =>
                    new KeyValuePair<string, VisualStudioInstance>($"{instance.Name} - {instance.Version}", instance))
                .ToDictionary(kp => kp.Key, kp => kp.Value);
    }
}
