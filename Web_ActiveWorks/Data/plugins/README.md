# ActiveWorks plugin catalog

The catalog root contains `plugins.json`. Each plugin is published as a folder under this directory. The folder must contain `plugin.json` and a ready ZIP package.

`plugins.json` example:

```json
{
  "plugins": [
    {
      "id": "PluginSoundNotify",
      "manifestPath": "PluginSoundNotify/plugin.json"
    }
  ]
}
```

Example:

```json
{
  "id": "PluginSoundNotify",
  "name": "Sound Notify",
  "version": "1.0.0",
  "description": "Sound notifications for ActiveWorks events.",
  "changelog": "Initial plugin package.",
  "publishedAtUtc": "2026-05-06T00:00:00Z",
  "packagePath": "PluginSoundNotify-1.0.0.0.zip",
  "files": [
    {
      "path": "PluginSoundNotify.dll",
      "targetRoot": "ProfilePlugins"
    },
    {
      "path": "NAudio.dll",
      "targetRoot": "Application"
    }
  ]
}
```

`ProfilePlugins` files are packaged for `Profiles/<user profile>/Plugins`.

`Application` files are packaged for the directory that contains `ActiveWorks.exe`.

`packagePath` is relative to the folder that contains `plugin.json`. Leave it empty when files are next to the manifest.
