# Insomnia

"Insomnia" is an RPG Shooter game. The player takes on the role of the sole survivor of a military base in a desert area that has fallen to the zombie virus. The player has to complete a series of missions to evacuate the base, including exploration missions, transport missions and missions to protect the facility from the progressive waves of attacks. Players will need to keep an eye on their character attributes as well as use the props they receive and analyse the terrain to destroy the massive hordes of zombies surrounding the military base.

This document describes the gameplay, mechanics and provides a walkthrough of the game.

## Win and Failure conditions

**Win condition**: Evacuation of the base by plane (final mission)

**Failure condition**: Character dies or data transfer machine is destroyed (mission 2)

## Key-binds

| **Function**                             | **Key**                    |
| ---------------------------------------- | -------------------------- |
| Movement                                 | WASD                       |
| Fire                                     | LMB                        |
| Aim                                      | RMB                        |
| Jump                                     | Space                      |
| Toggle Inventory and Player Status Panel | TAB                        |
| Toggle Task(Quest) Panel                 | T                          |
| Open Menu                                | ESC                        |
| Switch Weapon Mode                       | Q                          |
| Force Reload Magazine                    | R                          |
| Use Equipped Item                        | G                          |
| Quick equip next item in inventory       | Left Arrow and Right Arrow |
| Switch camera view distance              | O                          |

## Character attributes system

The player's character attributes have a significant impact on the game experience and influence the player's decisions at all times.

### Health

Players will lose varying amounts of Health Point when attacked by zombies and will die when they reach 0. Players can restore their health by using medical supplies.

### Stamina

Stamina is constantly consumed when moving and jumping, and can be quickly restored by resting in place. Players will suffer varying degrees of slowdown when Stamina is below 10 and 0. This means that players will need to plan their paths and tactics with a view to whether their current level of maximum stamina can support their actions and tactics. Players can restore their stamina by using medical supplies.

- 10% Stamina: slight slowdown
- 2% Stamina and below: severe slowdown

### Weight

All items in the game are weighted. When the total weight of the items in the player's backpack exceeds the maximum weight of the player's current level, the player will be slowed down and will use up stamina faster. This means that when you find a new item, you will have to prioritise it based on your current mission requirements, your character's maximum weight and your tactics, and then make an assessment and decision on whether to discard some of the items or whether to overload them

- 80% weight: slight slowdown
- 100% weight: moderate slowdown
- 120% and above: severe slowdown and double stamina drain

### Level

Players will receive the following bonuses when they level up:

- Increased HP Limit
- Increased weight limit
- Increased stamina limit
- Instantly restores a small amount of life and stamina

Some missions in the game require you to carry high weight items (e.g. fuel drums for refuelling planes) or require you to enter high risk areas (airfields with high levels of zombies). Depending on your level and character status, you can choose whether or not you want to clear out the zombie hordes and do simple tasks to collect tactical items to improve your success rate in the final mission before doing these difficult tasks.

## Status Effects System

The player's character will receive status effects (buffs/debuffs) as a result of active or passive events during the game, each with a different value and duration. The following is a list of the status effects that exist in the current game version：

| **Name**        | **Trigger conditions**                                              | **Effects**                                                          | **Duration**                                                |
| --------------- | ------------------------------------------------------------------- | -------------------------------------------------------------------- | ----------------------------------------------------------- |
| Change Mag      | Changing magazines                                                  | \-10% speed                                                          | 2s                                                          |
| Poison          | Only has a small chance of appearing when attacked by walkers       | \-2 HP（per second）                                                   | 5s                                                          |
| Heal            | Consuming small med box                                             | \+2.5 HP(per second)                                                 | 2s                                                          |
| Heal (Advanced) | Consuming large med box                                             | \+10 HP(per second) and removes poison effect                        | 5s                                                          |
| Overweight      | Total weight of items carried is higher than the maximum load value | Severe reduction in movement speed and increased stamina consumption | Continues to exist until the load falls below the maximum   |
| LowStamina      | Stamina value below 2%                                              | Severe reduction in movement speed                                   | Persists until stamina is restored to a critical level (2%) |

## Sounds

The sounds made by the zombies in the game are 3D stereoscopic, meaning that the player can determine the size, distance and location of the zombies and make tactical changes by the location and density of their footsteps and hissing sounds.

## Terrain

There is a wide variety of terrain and advanced movement techniques (such as jumping back and forth between the top floors of two buildings) can be used to reduce the risk of being approached by walkers when surrounded by them. In addition, there are locations that require special terrain and movement techniques to access.

## Game Map

![map-overview.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/E1AF7874-1D06-4985-AD04-F7EFC2194502_2/w3EoJOWFq7Rv8oexzNIGhInViUdAWM8wuZxV6Zz45Voz/map-overview.png)

## Item

There are several item spawn points in the game, which will continue to generate different types of items at their location at different times. Players need to equip an item first (click on the equip button in the inventory panel or use the left and right arrow keys to quickly equip next available item in bag) before deploy or consume item by using the item (G key).

| **Name**   | **Description**                                                                                                               | **Effects**                                                                   | **Weight** |
| ---------- | ----------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------- | ---------- |
| Land Mine  | The item will detonate and cause ranged damage when touched by any zombies                                                    | Deals widespread damage                                                       | 20         |
| Audio Mine | The item will actively attract walkers around it to approach and attack, and will trigger an explosion effect upon any attack | Attracts zombies within a certain range to approach and deal extensive damage | 10         |
| Small Med  | Minor restore HP                                                                                                              | Status Effect： Heal                                                           | 5          |
| Large Med  | Restores a large amount of HP and removes the effects of poisoning                                                            | Status Effect:  Heal (Advanced)                                               | 30         |
| Gas Can    | A mission prop that can be used to refuel the aircraft                                                                        | Required item for task                                                        | 150        |

#### Item spawn point distribution map

![map-item-spawn-location.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/29AB8CB5-7158-4B8B-9E0A-17F1A433BD26_2/VIlZ9Uj7mqmTvxxiy3DSzwKfcdGs0E12nYHILg8jNU0z/map-item-spawn-location.png)

## AI

### AI Type

There are different types of AI in the game, each with different movement speeds, attack styles and damage values.

| **Zombie Type** | **Description**                                                                                                     |
| --------------- | ------------------------------------------------------------------------------------------------------------------- |
| Default Zombie  | Medium movement speed <br>Low damage value <br>Low HP <br>Unable to attack while moving <br>No footsteps            |
| Army Zombie     | High movement speed<br>High damage value<br>Medium HP<br>Can attack while moving<br>Has the sound of footsteps      |
| Biosuit Zombie  | Low movement speed<br>Medium damage value<br>High HP<br>Unable to attack while moving<br>Has the sound of footsteps |

### AI generation and population growth system

There are multiple AI spawn points on the game map, which will continue to spawn AI until a set maximum number is generated. The AI population growth is linear, i.e. the density of zombies is much thinner in the early stages of the game compared to the later stages. Advanced players with some experience in the game can choose to prioritise exploring locations and transport missions in the early stages of the game.

In addition, the action logic of the AI generated at different birth points varies.

| **Movement Type** | **Description**                                                                                                         |
| ----------------- | ----------------------------------------------------------------------------------------------------------------------- |
| Stationary        | Spawned zombies will remain standing in their current position until they spot the player or are attacked by the player |
| Dynamic           | Spawned zombies will wander around within a defined area                                                                |
| Waypoint          | Spawned zombies will patrol a pre-planned path                                                                          |

#### AI spawn point distribution map

![map-ai-spawn-point.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/22E18B33-D0C9-4E65-BD7C-A6010B8C5D66_2/xcfAYtuHUziS5Kw721matnY1gHOVMUFbGW3Q36N0oZEz/map-ai-spawn-point.png)

## Weapon

The player can use the heavy machine gun and the grenade launcher by switching weapons (Q button) in the game, with a maximum of 30 rounds per magazine. The magazine can be reloaded by tapping the R button. In addition, the system will automatically reload ammunition when the magazine is empty. It is important to note that if the number of rounds required for the current weapon is greater than the number of rounds left in the player's clip, the player will need to manually reload rounds (R button) to activate the next round

| **Weapon type**   | Description                                                                                                                                                                                                                              |
| ----------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Heavy machine gun | Single point of damage<br>Each shot consumes 1 round and deals 20 points of damage.                                                                                                                                                      |
| Grenade launcher  | AOE damage<br>Each shot costs 5 rounds. Given the time limit of the demo, the grenades can currently instant kill all AI. i plan to add grenade-only bullets to limit the damage level and rarity of grenades in subsequent development. |

## Task (Quest)

There are currently 7 quests in the game, which will guide the player throughout the map and determine the ultimate victory conditions. The details for each quests can be viewed in the in-game quest panel (activated by pressing T keyboard)

![Task Flow.drawio.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/495033C4-8389-4CB4-97A6-9432EE1272ED_2/Kwj5PLPIIHcnI20x8yy6upYyfscYcHdHS0onydQpzDoz/Task%20Flow.drawio.png)

In the core mission (Task#2), the player has to defend against 4 rounds of attacks ranging from low to high intensity. After successfully defending each round, the player has a free buffer during which they can collect and use different types of tactical items (e.g. mines) to prepare for the next defence.

#### Quest Location Map

![map-quest-location.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/6ECCF062-BD22-4118-A3AC-C2CC346B06BB_2/QqNEvVBJVoTEskbSdg798Zb2jxztY4hDa1h5VAEyFBQz/map-quest-location.png)

## Gameplay walkthrough

### Part 1: Main menu

When you open the game, you will first be taken to the Main Menu screen. Players can click on the "Instruction" button to enter the Key-binds introduction page or click on "Start" to start a new game. Player can press ESC to toggle menu once entered the game scene.

![Main Menu.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/00B3BCE6-03D1-45EA-9B45-55E7AFAEC856_2/o53nj2oi0CmqVIwD9uy2P6ugxzs24CCK10Uxh2ahRxYz/Main%20Menu.png)

![Instruction.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/BF1BC1E4-89A6-4E60-95A5-DA8838FE3B12_2/C1PbxwT31VXa24MmSWVerpqUUngHIKHio1h5bDT72SAz/Instruction.png)

### Part 2: Overview of UI

The quest page is automatically displayed when you enter the game and you can open or close it by pressing the T key.

![First Quest Panel.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/01D641BF-C881-46B3-BF29-8F7C9792ACEB_2/Pchj5j2yLF6z2p1qKgQQrSdbLe93cSBnzE3UsJSDgB8z/First%20Quest%20Panel.png)

The player can see the character's current HP, stamina, ammunition, weapon mode and equipped items through the UI

![UI Intro.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/9E6B9268-04CB-49C3-B550-361189AFB5CF_2/ZH6vPs3sxu8krm2JzswHpBKQMklMZ5PqJIn8qm8jd08z/UI%20Intro.png)

You can open the inventory panel and the character stats panel by clicking the TAB button. After collecting the items (collision item models), you can see in the item list how many of that type of item you have and how much weight it takes up. At the same time, you can see the upper limits of character’s various attributes in the character stats panel on the right.

If you want to use an item, for example Land Mine, in order to create a sense of realism you need to do the following steps.

1. Equip the item. You can equip the selected item by clicking on the "Equip" button in the item list or by quickly equipping the next item you have by clicking Left Arrow or Right Arrow button.
2. After equipping an item, you can see the currently equipped item in the picture slot in the bottom right corner of the screen and you can use it by pressing the G button (e.g. to take a pill or place a mine).

Note: Items that can be collected in the game will have a green glowing effect, while tactical items that have been used or deployed by the player (e.g. mines) will have a red glowing effect.

![UI.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/A1C5A95F-7293-4940-979F-9F72B9BE14E1_2/IDErA3TZvVDMvxw5gCx8Pzgoi9thn5cPxD0HD193ZQUz/UI.png)

![Place mine.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/0FC8A5CB-D716-4EF8-9CCF-8ECC90BB288C_2/5qMPKzm3ykgeJPbqLxMPgCHmzWqjBWLHex8v3Iwl91Uz/Place%20mine.png)

The message alert screen is located on the right side of the screen and is only displayed when the player receives a new message

![Notification.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/331403C1-AA17-4F7D-BE72-9D0C79618374_2/HxMFxyXMjKdc5ZxQVun9AarvUGw9fMkuLpxZWYlh0R4z/Notification.png)

The list of status effects is located at the bottom centre of the screen and is only displayed when the player currently has an active Buff or Debuff

![Screen Shot 2022-05-04 at 04.32.10.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/7EB9B04F-C616-403D-A874-54161599133C_2/2VtRzXHonSszDpx5uhgL2EtGlE6h9RbUFZFVoJWHtXcz/Screen%20Shot%202022-05-04%20at%2004.32.10.png)

The player can aim by pressing the right mouse button, which activates the aiming icon and pulls the lens closer.

![Screen Shot 2022-05-04 at 05.03.31.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/F81A0892-7961-4B15-B9D3-5204BC685967_2/vC9TKea6SVUzzZdaoD01n3aMkHxasOIKyelnbkz8tHQz/Screen%20Shot%202022-05-04%20at%2005.03.31.png)

### Part 3: Task 1 and Task 2

You can find the signal transmitter at the task 1 location marked in the task location distribution map, which is located on the roof of the barracks with the antenna. You will see a yellow glow at the mission site and the 3d text of the mission title.

When entering the interior of the room, the player can switch the camera distance to ensure a clear view (press O).

![Switch View.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/E70363D9-9A56-48E9-BCAA-F7A339E99DC4_2/iAL6BQ8MMqx50A26Cd0TDwIxqPF2RK8qmvr1WPuDWJEz/Switch%20View.png)

![Quest Location.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/03432D5E-A042-4E33-AE7C-3EF3409C68CF_2/HAsGMImGYAvJuBXNZwhu1vWTzGuRGoxh65CTbxxABxEz/Quest%20Location.png)

The core mission of the game, Task2, will be activated automatically after the player completes Task1.

In the core mission (Task#2), the player has to defend against 4 rounds of low to high intensity zombie attacks and keep the signal transmitter from being destroyed (500 HP). The player is given a free buffer after each successful round, during which they can collect and deploy different types of tactical items (such as mines and sound mines) to prepare for the next round.

In defending against a horde of zombies, players can use the surrounding terrain and use advanced movement to reduce the risk of being surrounded by zombies. For example, you can jump and shoot between the roofs of two buildings to disorientate the zombies, or you can stand on a damaged tank and strafe the zombies around you, which will slow down the time it takes for the zombies to get to you.

Given the time constraints of the demo, it is recommended that players switch weapons to the grenade launcher to quickly eliminate the zombie horde.

Note: The grenade launcher will require special bullets to be used in subsequent development of the game, and more weapons will be added in future.

![Weapon-mode-2.png](https://res.craft.do/user/full/af721848-35de-2345-9299-280788f71cab/doc/B1F3F318-4CD3-41C7-93C9-7B20575B0466/1F36A179-1DB9-4CDD-ACBB-724A8CAA2B42_2/S7mBZtUhGycMFYBVLUp2zWpOqira1YMFPnpWopADjoYz/Weapon-mode-2.png)

Players will have a number of active missions at this point, but most of these will require players to have a high character attribute value to complete the mission safely, for example

- The airfield is very densely packed with zombies and players may need to be of a sufficient level (with a high stamina value) to ensure that they do not run out of stamina in the middle of a shot.
- The oil drum weighs 150kg, so low level player who carry the drum will be unable to run because they are overweight and will be at risk of being surrounded by zombies.

In addition, advanced players can complete the mission ahead of time by using mines wisely, positioning themselves flexibly and shooting zombies at specific locations to level up quickly. This also brings a richer and more flexible gameplay.

### Part 4: Completing the game

After completing the core mission, the player can complete the remaining map exploration and cargo delivery missions by following the task location distribution map, AI spawn points distribution map and item spawn points distribution map in this document to assist.

After the player has successfully delivered the oil drums to the plane, the player needs to access the mission site at the head of the plane to evacuate, i.e. the success condition of the game is achieved.


