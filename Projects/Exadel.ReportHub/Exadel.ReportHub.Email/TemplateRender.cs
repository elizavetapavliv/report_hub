using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exadel.ReportHub.Email.Abstract;
using Exadel.ReportHub.ReportHub.Configs;
using Microsoft.Extensions.Options;
using Stubble.Core;
using Stubble.Core.Builders;
using Stubble.Core.Interfaces;

namespace Exadel.ReportHub.Email;

public class TemplateRender(IOptionsMonitor<TemplateConfig> templateConfig) : ITemplateRender
{
    private readonly string _folder = templateConfig.CurrentValue.TemplatesFolder;
    private readonly StubbleVisitorRenderer _renderer = new StubbleBuilder().Build();

    public async Task<string> RenderAsync(string templateName, object viewModel, CancellationToken cancellationToken)
    {
        var path = Path.Combine(_folder, templateName);
        var text = await File.ReadAllTextAsync(path, cancellationToken);
        return await _renderer.RenderAsync(text, viewModel);
    }
}
