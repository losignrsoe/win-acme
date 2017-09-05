﻿using LetsEncrypt.ACME.Simple.Services;
using System.Linq;
using System;

namespace LetsEncrypt.ACME.Simple.Plugins.TargetPlugins
{
    class IISBinding : IISPlugin, ITargetPlugin
    {
        string ITargetPlugin.Name
        {
            get
            {
                return nameof(IISBinding);
            }
        }

        Target ITargetPlugin.Default(Options options)
        {
            return null;
        }

        Target ITargetPlugin.Aquire(Options options)
        {
            var targets = GetTargets().
                Select(x => new InputService.Choice<Target>(x) { description = x.Host }).
                ToList();
            return Program.Input.ChooseFromList("Choose binding", targets);
        }

        Target ITargetPlugin.Refresh(Options options, Target scheduled)
        {
            var match = GetTargets().FirstOrDefault(binding => binding.Host == scheduled.Host);
            if (match != null)
            {
                UpdateWebRoot(scheduled, match);
                return scheduled;
            }
            return null;
        }
    }
}
