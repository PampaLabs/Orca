using Microsoft.AspNetCore.Components;

namespace Balea.WebTemplate.Components.Shared;

public interface ITab
{
    RenderFragment ChildContent { get; }
}