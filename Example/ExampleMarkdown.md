# Config Preset
Access the new Preset Configurations feature in the ~~Configuration Control~~ Center


详细列表请到模组配置或更新日志中查看

1. ### 物品最大堆叠等辅助模组普遍功能
2. 非时装饰品也可放置在时装栏（启用模组直接生效）
3. > 城镇NPC入住机制^修改^: 夜晚入住、图鉴解锁后无视条件入住
4. > > 空间魔杖、建筑魔杖、法爆魔杖和钓鱼机等大大优化游戏体验的模组物品
5. 渔夫任务无冷却、控制墓碑是否掉落等辅助模组功能的整合
6. 随身增益、随身制作站等节省时间的功能
7. 与同队好友共享无尽增益、制作站等专为联机设置的功能
8. 含100格的超大背包，再也不用担心旗帜和药水放在哪了
9. 药水袋与旗帜盒，更方便地将你的药水旗帜整合在一个物品，节省空间

x^2^ x~3~ ++Underline++ ==Highlight==

Link: <https://github.com/ForOne-Club/ImproveGame/wiki>  
Auto Link: https://github.com/ForOne-Club/ImproveGame/wiki  
Bold Link: **<https://github.com/ForOne-Club/ImproveGame/wiki>**\
Delete Link: ~~<https://github.com/ForOne-Club/ImproveGame/wiki>~~\
Highlight Link: ==<https://github.com/ForOne-Club/ImproveGame/wiki>==\

Ultra-emphasis Link: ==~~**^[这是一个很长的超链接文本它太长了以至于要换行但是他的修饰很多不知道能不能正常显示如果能正常显示那真的是太好了总之先看看吧](https://github.com/ForOne-Club/ImproveGame/wiki "我也是做这个项目才知道原来html还有title这玩意")^**~~==

* These assets are usually accessible in arrays found in the `Terraria.GameContent` static classes, such as `TextureAssets`, if needed.
* > If you have extra textures, such as special effects for an NPC or Projectile, you can request them in `SetStaticDefaults` or one of the other **Load hooks.**
  * List Level 2
    * ## List Level 3 Heading
      * Vanilla doesn't preload all textures. For Item, NPC, Projectile, Background, and some other types of content unloaded Assets are populated into the fields of the `TextureAssets` class. Using these textures require first calling the respective `Main.instance.LoadX` method to ensure that they are loaded.
  * 的
    * > 3的这是一个==Quote+List==
  * dd23
* 444

> These assets are usually accessible in arrays found in the Terraria.GameContent static classes, such as TextureAssets, if needed.
> 2
> 3
> > 1
> > 2
> > 3
> > > 1
> > > 2
> > > 3

![image](https://github.com/user-attachments/assets/1f873313-be5d-4758-9e7a-39acdd19fd9f)

## Adding a Preset
Click the "Save as Preset" button to save your current configuration as a custom preset.

![image](https://github.com/user-attachments/assets/9d438de0-ffd3-469b-af98-2ef9f876fb2e)

The three buttons on the right, from left to right, are "Delete", "Rename", and "Apply". When you click "Apply", your current config will **be overwritten by the preset and cannot be undone**. Therefore, it is strongly recommended to **save your current configuration using "Save as Preset" before applying a new preset**.

If you have custom presets, clicking "Open Preset Folder" will open a folder containing all your presets on your computer. Presets are stored as folders, so you can zip them up and share them with your friends.

## Built-in Presets
The mod currently comes with three built-in presets: "Explorer", "Fighter", and "Vanilla Enjoyer", to help players adjust their configs. You can choose the corresponding preset based on your gameplay, and then adjust individual options.

![image](https://github.com/user-attachments/assets/8d6046c1-983a-4707-b5cd-beaac122ff53)

### Explorer
Best for vanilla-like playthroughs. Explore while facing typical challenges with moderate assistance. Might work well with mods like Thorium or Spirit

If you want a more vanilla-based gameplay experience, this preset will be your best choice. In this preset, you will keep the buffs on death instead of having infinite buffs. "OP" features like Big Backpack and Weather Control is turned off. Most functions are adjusted to a relatively balanced level to let you have a decent gameplay while enjoying a vanilla-like difficulty

Note that this preset does not disable any mod items. Whether or not to enable mod items is up to your discretion, as some mod items may lack balance if not restricted.

### Fighter
Stop digging and start fighting! Skip most grindings. Might work well with mods like Calamity or Fargo's Souls

This preset includes most convenient features that significantly reduce most repetitive work. If you are playing Calamity's Death Mode or Fargo's Souls's Eternity Mode, this would be a nice option. Most configurations are quality of life, to make the gameplay focus more on progressing and defeating bosses

### Vanilla Enjoyer
Just like vanilla. Play with few QoL features. Mod items are made unobtainable. If you don't want most of the features of this mod and just want to use it to replace some small quality of life mods, this preset is recommended.