﻿using System.Globalization;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Http;

using Orca.AspNetCore.Endpoints;

namespace Orca.Store.Http;

/// <inheritdoc />
public class PermissionStore : IPermissionStore
{
    private const string endpoint = "permissions";

    private readonly PermissionDataMapper _mapper = new();

    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionStore"/> class.
    /// </summary>
    /// The <paramref name="httpClientFactory"/> is used to create an <see cref="HttpClient"/> configured with the name specified in <see cref="HttpStoreDefaults.HttpClientName"/>.
    public PermissionStore(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(HttpStoreDefaults.HttpClientName);
    }

    /// <inheritdoc />
    public async Task<Permission> FindByIdAsync(string permissionId, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{permissionId}";
        var response = await _httpClient.GetFromJsonAsync<PermissionResponse>(uri, cancellationToken);
        return _mapper.FromResponse(response);
    }

    /// <inheritdoc />
    public async Task<Permission> FindByNameAsync(string permissionName, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/name/{permissionName}";
        var response = await _httpClient.GetFromJsonAsync<PermissionResponse>(uri, cancellationToken);
        return _mapper.FromResponse(response);
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> CreateAsync(Permission permission, CancellationToken cancellationToken)
    {
        var data = _mapper.ToRequest(permission);

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

    /// <inheritdoc />
    public async Task<AccessControlResult> UpdateAsync(Permission permission, CancellationToken cancellationToken)
    {
        var data = _mapper.ToRequest(permission);

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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<IList<Role>> GetRolesAsync(Permission permission, CancellationToken cancellationToken = default)
    {
        var roleMapper = new RoleDataMapper();

        var uri = $"{endpoint}/{permission.Id}/roles";

#if NET8_0_OR_GREATER
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<RoleResponse>(uri, cancellationToken);
        var result = response.Select(roleMapper.FromResponse);

        return await result.ToListAsync(cancellationToken);
#else
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<RoleResponse>>(uri, cancellationToken);
        var result = response.Select(roleMapper.FromResponse);

        return result.ToList();
#endif
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> AddRoleAsync(Permission permission, Role role, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{permission.Id}/roles/{role.Id}";
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

    /// <inheritdoc />
    public async Task<AccessControlResult> RemoveRoleAsync(Permission permission, Role role, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{permission.Id}/roles/{role.Id}";
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

    /// <inheritdoc />
    public async Task<IList<Permission>> ListAsync(CancellationToken cancellationToken = default)
    {
        var uri = $"{endpoint}";

#if NET8_0_OR_GREATER
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<PermissionResponse>($"{endpoint}", cancellationToken);
        var entities = response.Select(item => _mapper.FromResponse(item));

        return await entities.ToListAsync(cancellationToken);
#else
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<PermissionResponse>>($"{endpoint}", cancellationToken);
        var entities = response.Select(item => _mapper.FromResponse(item));

        return entities.ToList();
#endif
    }

    /// <inheritdoc />
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

#if NET8_0_OR_GREATER
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<PermissionResponse>(uri, cancellationToken);
        var entities = response.Select(item => _mapper.FromResponse(item));

        return await entities.ToListAsync(cancellationToken);
#else
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<PermissionResponse>>(uri, cancellationToken);
        var entities = response.Select(item => _mapper.FromResponse(item));

        return entities.ToList();
#endif
    }
}
