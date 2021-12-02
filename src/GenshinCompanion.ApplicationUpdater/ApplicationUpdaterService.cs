using AutoUpdaterDotNET;
using Octokit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GenshinCompanion.ApplicationUpdater
{
    public class ApplicationUpdaterService
    {
        public ApplicationUpdaterService()
        {
            ManifestReady += StartUpdate;
            GetLatestRelease();
        }

        public EventHandler ManifestReady;
        private const string ManifestURL = "https://github.com/SaDiablo/GenshinCompanion/releases";
        private string currentVersion;
        private string downloadURL;

        private async Task GetLatestRelease()
        {
            var owner = "SaDiablo";
            var repositoryName = "GenshinCompanion";

            var client = new GitHubClient(new ProductHeaderValue($"{owner}.{repositoryName}"));
            var releases = await client.Repository.Release.GetAll(owner, repositoryName);
            var latestrelease = releases.First(r => r.Draft == false);
            var assets = await client.Repository.Release.GetAllAssets(owner, repositoryName, latestrelease.Id);
            var latestreleaseassetid = assets.First(a => a.Name.Contains(repositoryName)).Id;
            var asset = await client.Repository.Release.GetAsset(owner, repositoryName, latestreleaseassetid);

            currentVersion = latestrelease.TagName.Substring(1);
            downloadURL = asset.BrowserDownloadUrl;
            ManifestReady.Invoke(this, EventArgs.Empty);
        }

        private void ParseUpdateInfo(ParseUpdateInfoEventArgs args)
        {
            args.UpdateInfo = new UpdateInfoEventArgs
            {
                CurrentVersion = currentVersion,
                DownloadURL = downloadURL,
            };
        }

        private void StartUpdate(object sender, EventArgs e)
        {
            AutoUpdater.ParseUpdateInfoEvent += ParseUpdateInfo;
            AutoUpdater.Start(ManifestURL);
        }
    }
}