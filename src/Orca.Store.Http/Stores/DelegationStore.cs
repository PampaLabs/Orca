using System.Globalization;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Http;

using Orca.AspNetCore.Endpoints;

namespace Orca.Store.Http;

public class DelegationStore : IDelegationStore
{
    private const string endpoint = "delegations";

    private readonly DelegationRequestMapper _requestMapper = new ();
    private readonly DelegationResponseMapper _responseMapper = new ();

    private readonly HttpClient _httpClient;

    public DelegationStore(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(HttpStoreDefaults.HttpClientName);
    }

    public async Task<Delegation> FindByIdAsync(string delegationId, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{delegationId}";
        var response = await _httpClient.GetFromJsonAsync<DelegationResponse>(uri, cancellationToken);
        return _responseMapper.ToEntity(response);
    }

    public async Task<Delegation> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/subject/{subject}";
        var response = await _httpClient.GetFromJsonAsync<DelegationResponse>(uri, cancellationToken);
        return _responseMapper.ToEntity(response);
    }

    public async Task<AccessControlResult> CreateAsync(Delegation delegation, CancellationToken cancellationToken)
    {
        var data = _requestMapper.FromEntity(delegation);

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

    public async Task<AccessControlResult> UpdateAsync(Delegation delegation, CancellationToken cancellationToken)
    {
        var data = _requestMapper.FromEntity(delegation);

        var uri = $"{endpoint}/{delegation.Id}";
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

    public async Task<AccessControlResult> DeleteAsync(Delegation delegation, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{delegation.Id}";
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

    public async Task<IList<Delegation>> ListAsync(CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}";

#if NET8_0_OR_GREATER
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<DelegationResponse>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return await entities.ToListAsync(cancellationToken);
#else
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<DelegationResponse>>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return entities.ToList();
#endif
    }

    public async Task<IList<Delegation>> SearchAsync(DelegationFilter filter, CancellationToken cancellationToken = default)
    {
        var query = QueryString.Empty;

        if (!string.IsNullOrEmpty(filter.Who))
        {
            query.Add(nameof(filter.Who), filter.Who);
        }

        if (!string.IsNullOrEmpty(filter.Whom))
        {
            query.Add(nameof(filter.Whom), filter.Whom);
        }

        if (filter.From.HasValue)
        {
            query.Add(nameof(filter.From), filter.From.Value.ToString(CultureInfo.InvariantCulture));
        }

        if (filter.To.HasValue)
        {
            query.Add(nameof(filter.To), filter.To.Value.ToString(CultureInfo.InvariantCulture));
        }

        if (filter.Enabled.HasValue)
        {
            query.Add(nameof(filter.Enabled), filter.Enabled.Value.ToString(CultureInfo.InvariantCulture));
        }

        var uri = $"{endpoint}{query.ToUriComponent()}";

#if NET8_0_OR_GREATER
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<DelegationResponse>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return await entities.ToListAsync(cancellationToken);
#else
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<DelegationResponse>>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return entities.ToList();
#endif
    }
}
