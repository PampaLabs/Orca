using Microsoft.AspNetCore.Components;

namespace Balea.WebTemplate.Shared;

public interface ITab
{
    RenderFragment ChildContent { get; }
}