# CharAddressableTools
> This en readme use DeepSeek-V3 to translate

[Chinese](README.md)
[English](README-EN.md)

Unity's Addressable system is quite useful by itself, especially for standalone games.
However, when you need to use it for hot updates in live online games, you'll find that many aspects of the logic are poorly documented, and some features are even designed in an overly abstract manner.
For example, certain version upgrades can cause errors in AddressableSettings, UI panels may not adapt properly, and after using AddressablesContentState.bin to check for updated resources, additional dependent resources might be incorrectly marked as "needing updates" but are actually bundled locally (and also listed as local in the manifest)...

# Available Tools
## ACSExporter
This tool exports the addressablesContentState manifest—used by Addressables to track resource changes—into JSON format. This allows for further comparison operations, such as using WinMerge to compare it with a previous AddressablesContentState file.

To use it, go to Tools/CharSui/Export AddressableContentState to JSON in the Unity editor, then sequentially select the content to be decompiled and the export path.
https://./img.jpg

## RemoteConfigCheck
If the configuration of an Addressables group is set to allow local updates for remote-related content, the remote manifest will attempt to fetch packages from the address of the build machine, which can lead to failures.