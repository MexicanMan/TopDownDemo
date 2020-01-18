# TopDownDemo
 Small top-down shooter gallery.
 
## Description
 Shoot as many targets as you can! There are two types of weapons at your disposal: usual bullets (to use press "space") and self-guided rockets (to use press "r"). But take into account, rockets have a longer timeout to reload than bullets! To move your character (which is tank btw) use arrows or WASD. Good luck!

![Game process](https://media.giphy.com/media/hrjekFMJZWtZE0NJuu/giphy.gif)

## Architecture
 This is an attempt to make own simple realisation of ECS pattern with the help of native Unity tools. I use GameObjects itself instead of Entities and Components are MonoBehaviour scripts. Systems and Entities store in EntityManager.
