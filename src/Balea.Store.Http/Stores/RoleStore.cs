using System.Globalization;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Http;

using Balea.AspNetCore.Endpoints;

namespace Balea.Store.Http;

public class RoleStore : IRoleStore
{
    private const string endpoint = "roles";

    private readonly RoleRequestMapper _requestMapper = new();
    private readonly RoleResponseMapper _responseMapper = new();

    private readonly HttpClient _httpClient;

    public RoleStore(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(HttpStoreDefaults.HttpClientName);
    }

    public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{roleId}";
        var response = await _httpClient.GetFromJsonAsync<RoleResponse>(uri, cancellationToken);
        return _responseMapper.ToEntity(response);
    }

    public async Task<Role> FindByNameAsync(string roleName, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/name/{roleName}";
        var response = await _httpClient.GetFromJsonAsync<RoleResponse>(uri, cancellationToken);
        return _responseMapper.ToEntity(response);
    }

    public async Task<AccessControlResult> CreateAsync(Role role, CancellationToken cancellationToken)
    {
        var data = _requestMapper.FromEntity(role);

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

    public async Task<AccessControlResult> UpdateAsync(Role role, CancellationToken cancellationToken)
    {
        var data = _requestMapper.FromEntity(role);

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

    public async Task<IList<Role>> ListAsync(CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}";
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<RoleResponse>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return await entities.ToListAsync(cancellationToken);
    }

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

        if (filter.Subjects is not null)
        {
            foreach (var subject in filter.Subjects)
            {
                query.Add(nameof(filter.Subjects), subject);
            }
        }

        var uri = $"{endpoint}{query.ToUriComponent()}";
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<RoleResponse>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return await entities.ToListAsync(cancellationToken);
    }

    public async Task<IList<string>> GetSubjectsAsync(Role role, CancellationToken cancellationToken = default)
    {
        var uri = $"{endpoint}/{role.Id}/subjects";
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<string>(uri, cancellationToken);

        return await response.ToListAsync(cancellationToken);
    }

    public async Task<AccessControlResult> AddSubjectAsync(Role role, string subject, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{role.Id}/subjects/{subject}";
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

    public async Task<AccessControlResult> RemoveSubjectAsync(Role role, string subject, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{role.Id}/subjects/{subject}";
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
