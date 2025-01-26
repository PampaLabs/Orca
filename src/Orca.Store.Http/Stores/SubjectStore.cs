using System.Globalization;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Http;

using Orca.AspNetCore.Endpoints;

namespace Orca.Store.Http;

/// <inheritdoc />
public class SubjectStore : ISubjectStore
{
    private const string endpoint = "subjects";

    private readonly SubjectRequestMapper _requestMapper = new();
    private readonly SubjectResponseMapper _responseMapper = new();

    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectStore"/> class.
    /// </summary>
    /// The <paramref name="httpClientFactory"/> is used to create an <see cref="HttpClient"/> configured with the name specified in <see cref="HttpStoreDefaults.HttpClientName"/>.
    public SubjectStore(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(HttpStoreDefaults.HttpClientName);
    }

    /// <inheritdoc />
    public async Task<Subject> FindByIdAsync(string subjectId, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{subjectId}";
        var response = await _httpClient.GetFromJsonAsync<SubjectResponse>(uri, cancellationToken);
        return _responseMapper.ToEntity(response);
    }

    /// <inheritdoc />
    public async Task<Subject> FindBySubAsync(string sub, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/sub/{sub}";
        var response = await _httpClient.GetFromJsonAsync<SubjectResponse>(uri, cancellationToken);
        return _responseMapper.ToEntity(response);
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> CreateAsync(Subject subject, CancellationToken cancellationToken)
    {
        var data = _requestMapper.FromEntity(subject);

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
    public async Task<AccessControlResult> UpdateAsync(Subject subject, CancellationToken cancellationToken)
    {
        var data = _requestMapper.FromEntity(subject);

        var uri = $"{endpoint}/{subject.Id}";
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
    public async Task<AccessControlResult> DeleteAsync(Subject subject, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{subject.Id}";
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
    public async Task<IList<Subject>> ListAsync(CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}";

#if NET8_0_OR_GREATER
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<SubjectResponse>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return await entities.ToListAsync(cancellationToken);
#else
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<SubjectResponse>>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return entities.ToList();
#endif
    }

    /// <inheritdoc />
    public async Task<IList<Subject>> SearchAsync(SubjectFilter filter, CancellationToken cancellationToken = default)
    {
        var query = QueryString.Empty;

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query.Add(nameof(filter.Name), filter.Name);
        }

        var uri = $"{endpoint}{query.ToUriComponent()}";

#if NET8_0_OR_GREATER
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<SubjectResponse>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return await entities.ToListAsync(cancellationToken);
#else
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<SubjectResponse>>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return entities.ToList();
#endif
    }

    /// <inheritdoc />
    public async Task<IList<Role>> GetRolesAsync(Subject subject, CancellationToken cancellationToken = default)
    {
        var roleMapper = new RoleResponseMapper();

        var uri = $"{endpoint}/{subject.Id}/roles";

#if NET8_0_OR_GREATER
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<RoleResponse>(uri, cancellationToken);
        var result = response.Select(roleMapper.ToEntity);

        return await result.ToListAsync(cancellationToken);
#else
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<RoleResponse>>(uri, cancellationToken);
        var result = response.Select(roleMapper.ToEntity);

        return result.ToList();
#endif
    }

    /// <inheritdoc />
    public async Task<AccessControlResult> AddRoleAsync(Subject subject, Role role, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{subject.Id}/roles/{role.Id}";
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
    public async Task<AccessControlResult> RemoveRoleAsync(Subject subject, Role role, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{subject.Id}/roles/{role.Id}";
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
