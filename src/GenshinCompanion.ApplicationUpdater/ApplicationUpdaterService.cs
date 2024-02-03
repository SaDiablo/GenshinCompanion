using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoUpdaterDotNET;
using Octokit;

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
            string owner = "SaDiablo";
            string repositoryName = "GenshinCompanion";

            var client = new GitHubClient(new ProductHeaderValue($"{owner}.{repositoryName}"));
            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll(owner, repositoryName);
            Release latestRelease = releases.First(r => r.Draft);
            IReadOnlyList<ReleaseAsset> assets = await client.Repository.Release.GetAllAssets(owner, repositoryName, latestRelease.Id);
            int latestReleaseAssetId = assets.First(a => a.Name.Contains(repositoryName)).Id;
            ReleaseAsset asset = await client.Repository.Release.GetAsset(owner, repositoryName, latestReleaseAssetId);

            currentVersion = latestRelease.TagName.Substring(1);
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