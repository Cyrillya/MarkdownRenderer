using System;
using Humanizer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MarkdownRenderer.Example;

public class MarkdownExample : ILoadable
{
    public void Load(Mod mod)
    {
        Main.QueueMainThreadAction(() =>
        {
            On_Main.DrawFPS += (orig, self) =>
            {
                orig(self);

                var text = MarkdownRenderer.ToMarkdownText(Text);
                text.MaxWidth = 600;
                text.Draw(Main.spriteBatch, new Vector2(100, 100));
            };
        });
    }

    public void Unload()
    {
    }

    public string Text => @"
# Config Preset
Access the new Preset Configurations feature in the ~~Configuration Control~~ Center

x^2^ x~3~ ++Underline++ ==Highlight==

Link: <https://github.com/ForOne-Club/ImproveGame/wiki>  
Auto Link: https://github.com/ForOne-Club/ImproveGame/wiki  
Bold Link: **<https://github.com/ForOne-Club/ImproveGame/wiki>**\
Delete Link: ~~<https://github.com/ForOne-Club/ImproveGame/wiki>~~\
Highlight Link: ==<https://github.com/ForOne-Club/ImproveGame/wiki>==
Superscript Link: ==~~**++^<https://github.com/ForOne-Club/ImproveGame/wiki>^++**~~==

![image](https://github.com/user-attachments/assets/1f873313-be5d-4758-9e7a-39acdd19fd9f)

## Adding a Preset
Click the ""Save as Preset"" button to save your current configuration as a custom preset.

![image](https://github.com/user-attachments/assets/9d438de0-ffd3-469b-af98-2ef9f876fb2e)

The three buttons on the right, from left to right, are ""Delete"", ""Rename"", and ""Apply"". When you click ""Apply"", your current config will **be overwritten by the preset and cannot be undone**. Therefore, it is strongly recommended to **save your current configuration using ""Save as Preset"" before applying a new preset**.

If you have custom presets, clicking ""Open Preset Folder"" will open a folder containing all your presets on your computer. Presets are stored as folders, so you can zip them up and share them with your friends.

## Built-in Presets
The mod currently comes with three built-in presets: ""Explorer"", ""Fighter"", and ""Vanilla Enjoyer"", to help players adjust their configs. You can choose the corresponding preset based on your gameplay, and then adjust individual options.

![image](https://github.com/user-attachments/assets/8d6046c1-983a-4707-b5cd-beaac122ff53)

### Explorer
Best for vanilla-like playthroughs. Explore while facing typical challenges with moderate assistance. Might work well with mods like Thorium or Spirit

If you want a more vanilla-based gameplay experience, this preset will be your best choice. In this preset, you will keep the buffs on death instead of having infinite buffs. ""OP"" features like Big Backpack and Weather Control is turned off. Most functions are adjusted to a relatively balanced level to let you have a decent gameplay while enjoying a vanilla-like difficulty

Note that this preset does not disable any mod items. Whether or not to enable mod items is up to your discretion, as some mod items may lack balance if not restricted.

### Fighter
Stop digging and start fighting! Skip most grindings. Might work well with mods like Calamity or Fargo's Souls

This preset includes most convenient features that significantly reduce most repetitive work. If you are playing Calamity's Death Mode or Fargo's Souls's Eternity Mode, this would be a nice option. Most configurations are quality of life, to make the gameplay focus more on progressing and defeating bosses

### Vanilla Enjoyer
Just like vanilla. Play with few QoL features. Mod items are made unobtainable. If you don't want most of the features of this mod and just want to use it to replace some small quality of life mods, this preset is recommended.
";
}
