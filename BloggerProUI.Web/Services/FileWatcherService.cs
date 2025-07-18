using BloggerProUI.Web.Services;

namespace BloggerProUI.Web.Services
{
    public class FileWatcherService : BackgroundService
    {
        private readonly ILogger<FileWatcherService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWebHostEnvironment _environment;
        private FileSystemWatcher? _cssWatcher;
        private FileSystemWatcher? _jsWatcher;

        public FileWatcherService(
            ILogger<FileWatcherService> logger,
            IServiceProvider serviceProvider,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _environment = environment;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_environment.IsDevelopment())
            {
                await StartWatchingFiles(stoppingToken);
            }
        }

        private async Task StartWatchingFiles(CancellationToken cancellationToken)
        {
            try
            {
                var cssPath = Path.Combine(_environment.WebRootPath, "css");
                var jsPath = Path.Combine(_environment.WebRootPath, "js");

                if (Directory.Exists(cssPath))
                {
                    _cssWatcher = new FileSystemWatcher(cssPath, "*.css")
                    {
                        EnableRaisingEvents = true,
                        IncludeSubdirectories = true,
                        NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime
                    };

                    _cssWatcher.Changed += OnCssFileChanged;
                    _cssWatcher.Created += OnCssFileChanged;
                    _cssWatcher.Deleted += OnCssFileChanged;
                    _cssWatcher.Renamed += OnCssFileChanged;
                    
                    _logger.LogInformation("CSS file watcher started for: {Path}", cssPath);
                }

                if (Directory.Exists(jsPath))
                {
                    _jsWatcher = new FileSystemWatcher(jsPath, "*.js")
                    {
                        EnableRaisingEvents = true,
                        IncludeSubdirectories = true,
                        NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime
                    };

                    _jsWatcher.Changed += OnJsFileChanged;
                    _jsWatcher.Created += OnJsFileChanged;
                    _jsWatcher.Deleted += OnJsFileChanged;
                    _jsWatcher.Renamed += OnJsFileChanged;
                    
                    _logger.LogInformation("JS file watcher started for: {Path}", jsPath);
                }

                // Keep the service running
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in file watcher service");
            }
        }

        private void OnCssFileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                _logger.LogInformation("CSS file changed: {FileName}", e.Name);
                
                // Kısa bir delay ekle (dosya yazma işleminin tamamlanması için)
                Task.Delay(100).ContinueWith(_ =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var versioningService = scope.ServiceProvider.GetRequiredService<IAssetVersioningService>();
                    versioningService.UpdateCssVersion();
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing CSS file change: {FileName}", e.Name);
            }
        }

        private void OnJsFileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                _logger.LogInformation("JS file changed: {FileName}", e.Name);
                
                // Kısa bir delay ekle (dosya yazma işleminin tamamlanması için)
                Task.Delay(100).ContinueWith(_ =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var versioningService = scope.ServiceProvider.GetRequiredService<IAssetVersioningService>();
                    versioningService.UpdateJsVersion();
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing JS file change: {FileName}", e.Name);
            }
        }

        public override void Dispose()
        {
            _cssWatcher?.Dispose();
            _jsWatcher?.Dispose();
            base.Dispose();
        }
    }
}