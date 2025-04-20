using System.Globalization;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Http;

using Orca.AspNetCore.Endpoints;

namespace Orca.Store.Http;

/// <inheritdoc />
public class RoleStore : IRoleStore
{
    private const string endpoint = "roles";

    private readonly RoleDataMapper _mapper = new();

    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleStore"/> class.
    /// </summary>
    /// The <paramref name="httpClientFactory"/> is used to create an <see cref="HttpClient"/> configured with the name specified in <see cref="HttpStoreDefaults.HttpClientName"/>.
    public RoleStore(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(HttpStoreDefaults.HttpClientName);
    }

    /// <inheritdoc />
    public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{roleId}";
        var response = await _httpClient.GetFromJsonAsync<RoleResponse>(uri, cancellationToken);
        return _mapper.FromResponse(response);
    }

    /// <inheritdoc />
    public async Task<Role> FindByNameAsync(string roleName, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/name/{roleName}";
        var response = await _httpClient.GetFromJsonAsync<RoleResponse>(uri, cancellationToken);
        return _mapper.FromResponse(response);
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> CreateAsync(Role role, CancellationToken cancellationToken)
    {
        var data = _mapper.ToRequest(role);

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
    public async Task<AccessControlResult> UpdateAsync(Role role, CancellationToken cancellationToken)
    {
        var data = _mapper.ToRequest(role);

        var uri = $"{endpoint}/{role.Id}";
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
    public async Task<AccessControlResult> DeleteAsync(Role role, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{role.Id}";
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
    public async Task<IList<Role>> ListAsync(CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}";

#if NET8_0_OR_GREATER
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<RoleResponse>(uri, cancellationToken);
        var entities = response.Select(item => _mapper.FromResponse(item));

        return await entities.ToListAsync(cancellationToken);
#else
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<RoleResponse>>(uri, cancellationToken);
        var entities = response.Select(item => _mapper.FromResponse(item));

        return entities.ToList();
#endif
    }

    /// <inheritdoc />
    public async Task<IList<Role>> SearchAsync(RoleFilter filter, CancellationToken cancellationToken = default)
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

        if (filter.Enabled.HasValue)
        {
            query.Add(nameof(filter.Enabled), filter.Enabled.Value.ToString(CultureInfo.InvariantCulture));
        }

        if (filter.Mappings is not null)
        {
            foreach (var mapping in filter.Mappings)
            {
                query.Add(nameof(filter.Mappings), mapping);
            }
        }

        var uri = $"{endpoint}{query.ToUriComponent()}";

#if NET8_0_OR_GREATER
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<RoleResponse>(uri, cancellationToken);
        var entities = response.Select(item => _mapper.FromResponse(item));

        return await entities.ToListAsync(cancellationToken);
#else
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<RoleResponse>>(uri, cancellationToken);
        var entities = response.Select(item => _mapper.FromResponse(item));

        return entities.ToList();
#endif
    }

    /// <inheritdoc />
    public async Task<IList<Subject>> GetSubjectsAsync(Role role, CancellationToken cancellationToken = default)
    {
        var subjectMapper = new SubjectDataMapper();

        var uri = $"{endpoint}/{role.Id}/subjects";

#if NET8_0_OR_GREATER
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<SubjectResponse>(uri, cancellationToken);
        var result = response.Select(subjectMapper.FromResponse);

        return await result.ToListAsync(cancellationToken);
#else
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<SubjectResponse>>(uri, cancellationToken);
        var result = response.Select(subjectMapper.FromResponse);

        return result.ToList();
#endif
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> AddSubjectAsync(Role role, Subject subject, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{role.Id}/subjects/{subject.Id}";
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
    public async Task<AccessControlResult> RemoveSubjectAsync(Role role, Subject subject, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{role.Id}/subjects/{subject.Id}";
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
    public async Task<IList<Permission>> GetPermissionsAsync(Role role, CancellationToken cancellationToken = default)
    {
        var permissionMapper = new PermissionDataMapper();

        var uri = $"{endpoint}/{role.Id}/permissions";

#if NET8_0_OR_GREATER
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<PermissionResponse>(uri, cancellationToken);
        var result = response.Select(permissionMapper.FromResponse);

        return await result.ToListAsync(cancellationToken);
#else
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<PermissionResponse>>(uri, cancellationToken);
        var result = response.Select(permissionMapper.FromResponse);

        return result.ToList();
#endif
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> AddPermissionAsync(Role role, Permission permission, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{permission.Id}/permissions/{role.Id}";
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
    public async Task<AccessControlResult> RemovePermissionAsync(Role role, Permission permission, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{permission.Id}/permissions/{role.Id}";
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
}
