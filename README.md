# Lego-Tools-Unity-Asset
Unity Asset for Lego video game

[Documentation](https://kap35.gitbook.io/lego-tools/)

## Contents

Scripts :
- Quests
- GameManager
- PlayerController
- Coin System
- Life System
- Destruction System
- Build system

Prefabs :
- Player
- Coin
- Enemy
- QuickStart Scene
- Build object

## How to use

### Quests
Quest are a way to track the progression of the player in the game. There is two types of quest : the main quest and the side quest. The main quest is the quest that the player has to complete to win the game. The side quest are the quest that the player can complete to get some rewards. The player can have only one main quest at a time and can have multiple side quest at the same time.

To create a new quest, you have to insert in scene the Quest Prefab. In quest prefab you can find the Quest script. This script is editable in Inspector and you can set the name of the quest, the description of the quest and the reward of the quest. You can also set the type of the quest (Main or Side). You can also set the quest as completed or not completed. If the quest is completed, the quest will be completed when the scene is loaded. You can also set the quest type : 
- Collect : the player has to collect a specific number of item
- Kill : the player has to kill a specific number of enemy
- Build : the player has to build a specific number of object
- Destroy : the player has to destroy a specific number of object
- Talk : the player has to talk to a specific number of NPC
- Move : the player has to move on way points
- Interact : the player has to interact with a specific object (door, button, ...) that contains Interactable script
- Custom : the player has to do something custom


### GameManager
The GameManager is the script that manage the game. It is a singleton so you can access to it from everywhere in the game. It contains the list of all the quest in the game and the current quest (added automatically at the beginning of the scene). It is used to manage the progression of the player in the game, UI, interactions, ...

### PlayerController
The PlayerController is the script that manage the player. It contains the player movement, the player jump, the player gravity, the player rotation, the player animation, the player move.
