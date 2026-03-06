# Ursur, the Tyrant

A hero mod, introducing Ursur, a Bear warrior who leverages Bleed and Chill on himself to increase his defenses and his attacks.

This currently does not include any events or quests related to Ursur. This will be updated in the future with a future mod release.

A couple of notes:
## Notes:
- I understand that things are going to be janky at times, and there are definitely bugs that will be worked out
- Any ideas you have will be greatly appreciated, please share them :) !
- This character has not been properly balanced. They might be very overpowered or underpowered. Let me know what you think and I can tone them up/down to be in line with other heroes.
- **What to do if Ursur is not unlocked:** Due to some jankiness of the way the code works, Ursur is unlocked only for the profile that is open when you launch the game (and for new profiles). So if they aren't unlocked in the correct profile, switch to that profile, close the game and re-open it and they will be unlocked. I'll fix this in the future, but most people won't notice it. You can also just use the profile editor to fix it.
- There are **no character events** for Ursur at this time. This is intentional as I am going to be releasing another mod in the next few months that will include loads of events for them along with some additional sub-zones.
- I don't know how to add hinting to enchantments, so the tooltips in the Tome of Knowledge won't be fully updated. These will be updated one day.
- There is a bug with one of his Traits (Bristly Hide) not updating properly. I am working on this. The trait should still function reasonably well.

This mod relies on [Obeliskial Content](https://across-the-obelisk.thunderstore.io/package/meds/Obeliskial_Content/).


<details>
<summary>Starting Deck/Card/Item</summary>
Starting Item: Wee Tyrant
Starting Card: Freezing Fortitude

![Wee Tyrant](https://github.com/binbinmods/Ursur/blob/main/Assets/weetyrant.png?raw=true)
![Freezing Fortitude](https://github.com/binbinmods/Ursur/blob/main/Assets/freezingfortitude.png?raw=true)

Starting Deck:
- 1 Freezing Fortitude
- 3 intercepts
- 2 spiked shield
- 3 shield bash
- 2 spiked ball
- 2 leap slam
- 2 maul

![Maul](https://github.com/binbinmods/Ursur/blob/main/Assets/maul.png?raw=true)
![Leap Slam](https://github.com/binbinmods/Ursur/blob/main/Assets/leapslam.png?raw=true)
![Spiked Ball](https://github.com/binbinmods/Ursur/blob/main/Assets/spikedball.png?raw=true)

</details>

<details>
<summary>Traits</summary>

### Level 1
Ursine Blood: Start each combat with 1 extra Energy. Whenever you play a Defense, suffer 2 Bleed. Whenever you play an Attack, suffer 2 Chill. 

### Level 2

![Resilience](https://github.com/binbinmods/Ursur/blob/main/Assets/resilience.png?raw=true)
Resilience. When you play a Defense, Draw 1, Gain Fortify + fury. [3 charges]

![Bellowing Blows](https://github.com/binbinmods/Ursur/blob/main/Assets/bellowingblows.png?raw=true)
Bellowing Blows: When you deal damage with a hit, gain taunt, block, draw a card, suffer bleed and chill. [4 charges]

### Level 3

Bristly Hide: When you gain Taunt or Fortify, gain twice as many Thorns. +1 Taunt charge for every 25 stacks of Bleed. +1 Fortify charge for every 25 stacks of Chill.
Bear With It: At the start of each turn, reduce the cost of Attacks by 1 for every 18 Bleed on Ursur. Reduce the cost of all Defenses by 25 for every 25 Chill.

### Level 4

![Grizzled Claws](https://github.com/binbinmods/Ursur/blob/main/Assets/grizzledclaws.png?raw=true)
Grizzled Claws: Thorns +1. When you play an Attack, Gain thorns, and Add a Vicious Slash or a Brutal Slash to your hand. [4 charges]
Vicious Slash: Deal X Slashing, Apply X Bleed, Suffer X Chill. Where X = Bleed\*0.5. Dispel your Bleed. Gain 1 Fortify.
Brutal Slash: Deal X Blunt. Apply X Chill, Suffer X Bleed. Where X = Chill\*0.5. Dispel your Chill. Gain 2 Taunt.

![Best Defense](https://github.com/binbinmods/Ursur/blob/main/Assets/bestdefense.png?raw=true)
Best Defense: When you play a defense add a random attack to your hand, costs 0 and vanish. 5x uses

### Level 5

Unbearable: When you play an Attack, gain 1 Thorns. When you play a Defense, gain 1 Fury. 
Bearly Noticeable: Chill +1. Bleed +1. Chill on Ursur increases all resistances by 0.25% per stack. Bleed on Ursur increases all damage by 1.5% per stack.

</details>


## Installation (manual)

1. Install [Obeliskial Essentials](https://across-the-obelisk.thunderstore.io/package/meds/Obeliskial_Essentials/) and [Obeliskial Content](https://across-the-obelisk.thunderstore.io/package/meds/Obeliskial_Content/).
2. Click _Manual Download_ at the top of the page.
3. In Steam, right-click Across the Obelisk and select _Manage_->_Browse local files_.
4. Extract the archive into the game folder. Your _Across the Obelisk_ folder should now contain a _BepInEx_ folder and a _doorstop\_libs_ folder.
5. Run the game. If everything runs correctly, you will see this mod in the list of registered mods on the main menu.
6. Press F5 to open/close the Config Manager and F1 to show/hide mod version information.
7. Note: I am not certain about these install instructions. In the worst case, just copy _TheWiseWolf.dll_ into the _BepInEx\plugins_ folder, and the _Ursur_ folder (the one with the subfolders containing the json files) into _BepInEx\config\Obeliskial\_importing_

## Installation (automatic)

1. Download and install [Thunderstore Mod Manager](https://www.overwolf.com/app/Thunderstore-Thunderstore_Mod_Manager) or [r2modman](https://across-the-obelisk.thunderstore.io/package/ebkr/r2modman/).
2. Click **Install with Mod Manager** button on top of the page.
3. Run the game via the mod manager.

## Support

This has been updated for version 1.4.

Hope you enjoy it and if have any issues, ping me in Discord or make a post in the **modding #support-and-requests** channel of the [official Across the Obelisk Discord](https://discord.gg/across-the-obelisk-679706811108163701).

## Donation

Please do not donate to me. If you wish to support me, I would prefer it if you just gave me feedback. 