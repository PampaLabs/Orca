using System.Globalization;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Http;

using Balea.AspNetCore.Endpoints;

namespace Balea.Store.Http;

public class PermissionStore : IPermissionStore
{
    private const string endpoint = "permissions";

    private readonly PermissionRequestMapper _requestMapper = new();
    private readonly PermissionResponseMapper _responseMapper = new();

    private readonly HttpClient _httpClient;

    public PermissionStore(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(HttpStoreDefaults.HttpClientName);
    }

    public async Task<Permission> FindByIdAsync(string permissionId, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{permissionId}";
        var response = await _httpClient.GetFromJsonAsync<PermissionResponse>(uri, cancellationToken);
        return _responseMapper.ToEntity(response);
    }

    public async Task<Permission> FindByNameAsync(string permissionName, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/name/{permissionName}";
        var response = await _httpClient.GetFromJsonAsync<PermissionResponse>(uri, cancellationToken);
        return _responseMapper.ToEntity(response);
    }

    public async Task<AccessControlResult> CreateAsync(Permission permission, CancellationToken cancellationToken)
    {
        var data = _requestMapper.FromEntity(permission);

        var uri = $"{endpoint}";
        var response = await _httpClient.PostAsJsonAsync(uri, data, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return AccessControlResult.Success;
        }
        else
        {
            return AccessControlResult.Failed(new AccessControlError { Code = response.StatusCode.ToString() });
        }
    }

    public async Task<AccessControlResult> UpdateAsync(Permission permission, CancellationToken cancellationToken)
    {
        var data = _requestMapper.FromEntity(permission);

        var uri = $"{endpoint}/{permission.Id}";
        var response = await _httpClient.PutAsJsonAsync(uri, data, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return AccessControlResult.Success;
        }
        else
        {
            return AccessControlResult.Failed(new AccessControlError { Code = response.StatusCode.ToString() });
        }
    }

    public async Task<AccessControlResult> DeleteAsync(Permission permission, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{permission.Id}";
        var response = await _httpClient.DeleteAsync(uri, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return AccessControlResult.Success;
        }
        else
        {
            return AccessControlResult.Failed(new AccessControlError { Code = response.StatusCode.ToString() });
        }
    }

    public async Task<IList<string>> GetRolesAsync(Permission permission, CancellationToken cancellationToken = default)
    {
        var uri = $"{endpoint}/{permission.Id}/roles";
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<string>(uri, cancellationToken);

        return await response.ToListAsync(cancellationToken);
    }

    public async Task<AccessControlResult> AddRoleAsync(Permission permission, string roleName, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{permission.Id}/roles/{roleName}";
        var response = await _httpClient.PostAsync(uri, null, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return AccessControlResult.Success;
        }
        else
        {
            return AccessControlResult.Failed(new AccessControlError { Code = response.StatusCode.ToString() });
        }
    }

    public async Task<AccessControlResult> RemoveRoleAsync(Permission permission, string roleName, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{permission.Id}/roles/{roleName}";
        var response = await _httpClient.DeleteAsync(uri, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return AccessControlResult.Success;
        }
        else
        {
            return AccessControlResult.Failed(new AccessControlError { Code = response.StatusCode.ToString() });
        }
    }

    public async Task<IList<Permission>> ListAsync(CancellationToken cancellationToken = default)
    {
        var uri = $"{endpoint}";
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<PermissionResponse>($"{endpoint}", cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return await entities.ToListAsync(cancellationToken);
    }

    public async Task<IList<Permission>> SearchAsync(PermissionFilter filter, CancellationToken cancellationToken = default)
    {
        var query = QueryString.Empty;

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query.Add(nameof(filter.Name), filter.Name);
        }

        if (!string.IsNullOrEmpty(filter.Description))
        {
            query.Add(nameof(filter.Description), filter.Description);
        }

        if (filter.Roles is not null)
        {
            foreach (var role in filter.Roles)
            {
                query.Add(nameof(filter.Roles), role);
            }
        }

        var uri = $"{endpoint}{query.ToUriComponent()}";
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<PermissionResponse>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return await entities.ToListAsync(cancellationToken);
    }
}
