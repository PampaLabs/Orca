using Microsoft.AspNetCore.Components;

namespace Orca.WebTemplate.Components.Shared;

public interface ITab
{
    RenderFragment ChildContent { get; }
}