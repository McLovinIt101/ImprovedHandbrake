# Improved Handbrake Script

## Overview

The Improved Handbrake Script is a custom script for FiveM that enhances the handbrake functionality in vehicles. This script allows players to manually control the handbrake, preventing the vehicle from rolling when parked. Additionally, it simulates vehicle creep when the handbrake is off and the player is outside the vehicle.

## Features

- **Manual Handbrake Control:** Players can toggle the handbrake using the space bar.
- **Vehicle Creep Simulation:** When the handbrake is off and the player is outside the vehicle, the vehicle will slowly move forward.
- **Gear Indicator:** Displays the current gear (P for Park, D for Drive) on the screen.
- **Sound Effects:** Plays sound effects when toggling the handbrake.
- **Notifications:** Displays notifications to the player when certain conditions are met (e.g., when the vehicle needs to slow down to enable the handbrake).

## Installation

1. Download the script files.
2. Place the files in your FiveM server's resource directory.
3. Add `start ImprovedHandbrake` to your server's `server.cfg` file.

## Usage

- **Toggle Handbrake:** Press the space bar to toggle the handbrake on or off.
- **Gear Indicator:** The gear indicator will display "P" when the handbrake is on and "D" when it is off.
- **Vehicle Creep:** When the handbrake is off and the player exits the vehicle, the vehicle will slowly move forward.

## Configuration

The script uses the following default settings:

- **Handbrake Control:** Space bar (mapped to `Control.VehicleHandbrake`)
- **Speed Threshold:** 6.0 MPH (vehicle must be below this speed to engage the handbrake)
- **Creep Speed:** 1.0 (the speed at which the vehicle moves forward when the handbrake is off)

## Contributing

Feel free to submit issues or pull requests to improve the script. Contributions are welcome!
