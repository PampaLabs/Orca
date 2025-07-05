using System.Globalization;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Http;

using Orca.AspNetCore.Endpoints;

namespace Orca.Store.Http;

/// <inheritdoc />
public class SubjectStore : ISubjectStore
{
    private const string endpoint = "subjects";

    private readonly SubjectDataMapper _mapper = new();

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
        return _mapper.FromResponse(response);
    }

    /// <inheritdoc />
    public async Task<Subject> FindBySubAsync(string sub, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/sub/{sub}";
        var response = await _httpClient.GetFromJsonAsync<SubjectResponse>(uri, cancellationToken);
        return _mapper.FromResponse(response);
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> CreateAsync(Subject subject, CancellationToken cancellationToken)
    {
        var data = _mapper.ToRequest(subject);

        var uri = $"{endpoint}";
        var response = await _httpClient.PostAsJsonAsync(uri, data, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return AccessManagementResult.Success;
        }
        else
        {
            return AccessManagementResult.Failed(new AccessManagementError { Code = response.StatusCode.ToString() });
        }
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> UpdateAsync(Subject subject, CancellationToken cancellationToken)
    {
        var data = _mapper.ToRequest(subject);

        var uri = $"{endpoint}/{subject.Id}";
        var response = await _httpClient.PutAsJsonAsync(uri, data, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return AccessManagementResult.Success;
        }
        else
        {
            return AccessManagementResult.Failed(new AccessManagementError { Code = response.StatusCode.ToString() });
        }
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> DeleteAsync(Subject subject, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{subject.Id}";
        var response = await _httpClient.DeleteAsync(uri, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return AccessManagementResult.Success;
        }
        else
        {
            return AccessManagementResult.Failed(new AccessManagementError { Code = response.StatusCode.ToString() });
        }
    }

    /// <inheritdoc />
    public async Task<IList<Subject>> ListAsync(CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}";

#if NET8_0_OR_GREATER
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<SubjectResponse>(uri, cancellationToken);
        var entities = response.Select(item => _mapper.FromResponse(item));

        return await entities.ToListAsync(cancellationToken);
#else
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<SubjectResponse>>(uri, cancellationToken);
        var entities = response.Select(item => _mapper.FromResponse(item));

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
        var entities = response.Select(item => _mapper.FromResponse(item));

        return await entities.ToListAsync(cancellationToken);
#else
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<SubjectResponse>>(uri, cancellationToken);
        var entities = response.Select(item => _mapper.FromResponse(item));

        return entities.ToList();
#endif
    }

    /// <inheritdoc />
    public async Task<IList<Role>> GetRolesAsync(Subject subject, CancellationToken cancellationToken = default)
    {
        var roleMapper = new RoleDataMapper();

        var uri = $"{endpoint}/{subject.Id}/roles";

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
    public async Task<AccessManagementResult> AddRoleAsync(Subject subject, Role role, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{subject.Id}/roles/{role.Id}";
        var response = await _httpClient.PostAsync(uri, null, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return AccessManagementResult.Success;
        }
        else
        {
            return AccessManagementResult.Failed(new AccessManagementError { Code = response.StatusCode.ToString() });
        }
    }

    /// <inheritdoc />
    public async Task<AccessManagementResult> RemoveRoleAsync(Subject subject, Role role, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{subject.Id}/roles/{role.Id}";
        var response = await _httpClient.DeleteAsync(uri, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return AccessManagementResult.Success;
        }
        else
        {
            return AccessManagementResult.Failed(new AccessManagementError { Code = response.StatusCode.ToString() });
        }
    }
}
