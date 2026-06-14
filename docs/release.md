# Release Process

This project uses Semantic Versioning for the NuGet package.

Preview versions use a prerelease suffix:

```text
0.1.0-preview.1
0.1.0-preview.2
0.2.0-preview.1
```

Stable versions use plain SemVer:

```text
1.0.0
1.1.0
2.0.0
```

## Before Release

1. Update the package version in `Directory.Build.props`.
2. Update `CHANGELOG.md`.
3. Update `README.md` and the package README at `src/TelegramGatewayNet/README.md` if the public API or supported methods changed.
4. Run:

```bash
dotnet test TelegramGatewayNet.sln
dotnet pack src/TelegramGatewayNet/TelegramGatewayNet.csproj --configuration Release --output artifacts/packages
```

5. Inspect the generated package:

```bash
unzip -l artifacts/packages/TelegramGatewayNet.<version>.nupkg
```

## GitHub Actions

CI runs on pull requests, pushes to `main`, and `v*` tags. It restores, builds, tests, packs the
package, and uploads the package artifacts.

Release automation runs when a `v*` tag is pushed. It creates a GitHub Release, attaches the
generated `.nupkg`/`.snupkg` artifacts, and publishes to NuGet.org when the repository secret
`NUGET_API_KEY` is configured.

Manual publishing workflows are also available:

```text
Publish NuGet
Publish GitHub Packages
```

For manual NuGet publishing, provide:

```text
git_ref: v0.1.0-preview.1
version: 0.1.0-preview.1
```

## Tagging

Use tags that match the package version prefixed with `v`:

```bash
git tag -a v0.1.0-preview.1 -m "TelegramGatewayNet 0.1.0-preview.1"
git push origin v0.1.0-preview.1
```

Preview tags such as `v0.1.0-preview.1` are marked as GitHub prereleases automatically.

## Repository Secrets

For NuGet.org publishing, configure a repository secret named:

```text
NUGET_API_KEY
```

GitHub Packages publishing uses the built-in `GITHUB_TOKEN` and needs no extra secret.
