# VR Movement Exploration Project

## Description
This project aimed to explore different ways a player is able to move through the VR space via controller interactions.

## Implementations

- **Joystick**  
  This implementation was the most basic. It used the left joystick on the controllers to smoothly move the player forward, backward, etc., and the right joystick was used to smoothly rotate the main camera.

- **Driving**  
  This implementation used the right controller's trigger and main button to move the car forward and backward. The player is able to change direction of where the forward or backward velocity is being applied by looking in a new direction with the headset.

- **Running**  
  This implementation is very similar to the Driving implementation in that the direction of movement is changed by using the headset. However, to apply forward velocity to the camera, the user must move the two controllers in opposite directions as if they were running.

## Observations

There were two main observations that I had with the user tests pertaining to the running implementation, since this is the implementation I want to explore more in the future.

1. People typically wanted to make short, fast motions with the controllers to try and increase the speed. This seems like an intuitive way for the user to try and increase the speed, but my intention is to make the interaction feel as much like running as possible. I want to try and implement this so the length of each stride impacts the speed more rather than just purely the speed.

2. People were really interested in making the visuals feel more reactive to the running motion. A typical suggestion for what I could do to improve this is by adding some type of head-bobbing animation to the camera.

## Q/A

### Pre-Test Questions

- **How do you typically move through VR space?**  
  - Joysticks, Joysticks, Teleportation, Joysticks, Teleportation

- **Have you used controller motion rather than joysticks to move through VR space?**  
  - No, No, Yes, No, Yes

- **Have you used only the headset to control the direction that your player moves?**  
  - No, No, No, No, Yes

### Post-Test Questions

- **Which interaction felt the best/most intuitive to you?**  
  - Joysticks, Joysticks, Joysticks, Joysticks, Running

- **Which interaction was the most unique/entertaining to use?**  
  - Running, Running, Running, Running, Running

- **If the answer to the previous questions were different, what is a way the more unique interaction could be improved to be more intuitive?**  
  - Add head bobbing to running motion  
  - Make the collision detection on the side boxes better  
  - Make the controller speed correlate with the speed of the player  
  - Move the player in the direction the controllers are facing rather than the direction the headset is facing
