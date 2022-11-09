using System.Diagnostics;

namespace fh_service_directory_api.api.Endpoints;

public class MinimalGeneralEndPoints
{
    public void RegisterMinimalGeneralEndPoints(WebApplication app)
    {
        app.MapGet("api/info", (IConfiguration configuration, ILogger < MinimalGeneralEndPoints> logger) =>
        {
            try
            {
                var assembly = typeof(WebMarker).Assembly;

                var creationDate = File.GetCreationTime(assembly.Location);
                var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

                string kvURL = configuration.GetValue<string>("KeyVaultConfig:KVUrl");
                string tenantId = configuration.GetValue<string>("KeyVaultConfig:TenantId");
                string clientId = configuration.GetValue<string>("KeyVaultConfig:ClientId");
                string clientSecretId = configuration.GetValue<string>("KeyVaultConfig:ClientSecretId");
                string useDbType = configuration.GetValue<string>("UseDbType");
                bool haveKeyVaultConfig = false;
                if (!string.IsNullOrEmpty(kvURL) && !string.IsNullOrEmpty(tenantId) && !string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientSecretId))
                {
                    haveKeyVaultConfig = true;
                }
                
                return Results.Ok($"Version: {version}, Last Updated: {creationDate}, Have Vault Config: {haveKeyVaultConfig}, Db Type: {useDbType}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting info (api). {exceptionMessage}", ex.Message);
                Debug.WriteLine(ex.Message);
                throw;
            }
        });
    }
}
