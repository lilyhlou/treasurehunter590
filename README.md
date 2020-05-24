##  Redirected Walking and Distractors in Small Physical Spaces
*Created for COMP 590: Intro to VR, Game Dev, & HCI*

[<img src="https://img.youtube.com/vi/Z2FioV4crCg/maxresdefault.jpg" width="50%">](https://youtu.be/Z2FioV4crCg)  
*Click for video*

Our study aims to discover the effectiveness of distractors for redirected walking in small spaces. We hypothesize that distractors will influence the player's movements throughout the game, such as by making them progress through the maze at a slower pace due to the moving targets compelling players to shoot them before continuing through the maze. In an ideal world, we would have used VR headsets to test to see how immersed an individual felt between distractors or not distractors; however, our study focuses on the impact of distractors in influencing behavior during the game.


Our experiment put users in an immersive game environment. Users navigate using WASD through a maze to find an exit while being confronted by moving targets along the way which players have to shoot. The game was developed in Unity using assets from the Unity Store. 

We experimented with redirected walking by applying a redirected walking script that adds a translational gain to user movements, mapping movements as larger in the virtual game world, and redirected walking which rotates the map in the virtual world. Another script created distractors when players walked too close to the predefined boundary. The boundary was determined by a predetermined Euclidean distance from the camera to the Tracking Origin. We determined if users left this area by using the collision hitbox of a hidden cube GameObject. In this game, moving targets were created to encourage players to turn their bodies and distort their direction in the physical game. If a player exited the predefined boundary, the player's movement would halt in the virtual game until they re-entered the boundary. 

We tested our hypothesis by having 4 users download a version of our program to their computers and test the game. Users were grouped into either a control condition, which did not include a distractor script, or a experimental condition which included moving targets. They were not told the purpose of the study or that their walking was redirected. We recorded the times it took for individuals to complete the maze to see how distractors we affecting a users progress through the maze.


