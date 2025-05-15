using System.Text.RegularExpressions;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Web.Framework.Infrastructure.Extensions;
using Nop.Web.Framework.Mvc.Routing;
using Newtonsoft.Json;

namespace Nop.Web.Framework.Infrastructure;

/// <summary>
/// Represents object for the configuring common features and middleware on application startup
/// </summary>
public partial class NopCommonStartup : INopStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        //add options feature
        services.AddOptions();

        //add distributed cache
        services.AddDistributedCache();

        //add HTTP session state feature
        services.AddHttpSession();

        //add default HTTP clients
        services.AddNopHttpClients();

        //add anti-forgery
        services.AddAntiForgery();

        //add theme support
        services.AddThemes();

        //add routing
        services.AddRouting(options =>
        {
            //add constraint key for language
            options.ConstraintMap[NopRoutingDefaults.LanguageParameterTransformer] = typeof(LanguageParameterTransformer);
        });
    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
        //check whether requested page is keep alive page
        application.UseKeepAlive();

        //check whether database is installed
        application.UseInstallUrl();

        //use HTTP session
        application.UseSession();

        //use request localization
        application.UseNopRequestLocalization();

        //configure PDF
        application.UseNopPdf();

        this.RunPreScriptAsync();
    }

    private async void RunPreScriptAsync()
    {
        var _fileProvider = EngineContext.Current.Resolve<INopFileProvider>();
        string directoryPath = _fileProvider.MapPath(@"wwwroot/MiscellaneousFile");
        // Check if the directory exists
        if (_fileProvider.DirectoryExists(directoryPath))
        {
            var _logger = EngineContext.Current.Resolve<Services.Logging.ILogger>();
            // Get all files in the directory
            string[] files = _fileProvider.GetFiles(directoryPath);
            try
            {
                if (files?.Any() ?? false)
                {
                    files = files.OrderBy(x => _fileProvider.GetFileNameWithoutExtension(x)).ToArray();
                    var _dataProvider = EngineContext.Current.Resolve<INopDataProvider>();
                    if (!await _dataProvider.DatabaseExistsAsync())
                    {
                        return;
                    }
                    // Loop through each file and read its text
                    foreach (string filePath in files)
                    {
                        if (filePath.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var fileContent = await _fileProvider.ReadAllTextAsync(filePath, Encoding.Default);
                            fileContent = fileContent.Trim();
                            var fileContentList = Regex.Split(fileContent, @"^\s*GO\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                            foreach (var content in fileContentList)
                            {
                                var trimedContent = content?.Trim();
                                if (!string.IsNullOrEmpty(trimedContent))
                                {

                                    try
                                    {
                                        await _dataProvider.ExecuteNonQueryAsync(trimedContent);
                                    }
                                    catch (Exception ex)
                                    {
                                        await _logger.WarningAsync($"Error on running script. Path: {filePath}, Content: {trimedContent},\n{ex.Message}", ex);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _logger.WarningAsync($"Error on running script at: {ex.Message}", ex);
            }
        }
    }


    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 100; //common services should be loaded after error handlers
}