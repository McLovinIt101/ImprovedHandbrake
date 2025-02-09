using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Threading.Tasks;

namespace ImprovedHandbrake
{
    public class ImprovedHandbrakeScript : BaseScript
    {
        private enum BrakeState { On = 1, Off = 0 }
        private BrakeState _currentBrakeState = BrakeState.On;
        private string _gearDisplay = "P";
        private readonly Color _parkColor = new Color(189, 12, 15);
        private readonly Color _driveColor = new Color(255, 255, 255);

        private const float MPHConversion = 2.236936f;
        private const float SpeedThreshold = 6.0f;
        private const Control HandbrakeControl = Control.VehicleHandbrake;

        private int _lastVehicle = 0;

        public ImprovedHandbrakeScript()
        {
            Tick += HandleInput;
            Tick += UpdateHandbrakeState;
            Tick += StationaryRoll;
            Tick += DrawGearIndicator;

            EventHandlers["handbrakeToggle"] += new Action(OnHandbrakeToggle);
        }

        private async Task HandleInput()
        {
            while (true)
            {
                await Delay(5);
                if (Game.IsControlJustPressed(1, HandbrakeControl))
                {
                    TriggerEvent("handbrakeToggle");
                }
            }
        }

        private void OnHandbrakeToggle()
        {
            var vehicle = GetPlayerVehicle();
            if (!IsValidVehicle(vehicle) || !IsPlayerDriver(vehicle)) return;

            if (_currentBrakeState == BrakeState.Off)
            {
                float speed = API.GetEntitySpeed(vehicle) * MPHConversion;

                if (speed < SpeedThreshold)
                {
                    SetBrakeState(BrakeState.On);
                    PlaySound("park");
                }
                else
                {
                    ShowNotification("~r~Slow down ~s~to enable handbrake.");
                }
            }
            else
            {
                SetBrakeState(BrakeState.Off);
                PlaySound("drive");
            }
        }

        private async Task UpdateHandbrakeState()
        {
            while (true)
            {
                await Delay(0);
                int currentVehicle = GetPlayerVehicle();
                if (IsValidVehicle(currentVehicle))
                {
                    _lastVehicle = currentVehicle;
                }

                if (IsValidVehicle(_lastVehicle))
                {
                    API.SetVehicleHandbrake(_lastVehicle, _currentBrakeState == BrakeState.On);
                }
            }
        }

        private async Task StationaryRoll()
        {
            while (true)
            {
                await Delay(100);
                if (!IsValidVehicle(_lastVehicle)) continue;

                bool isPlayerInVehicle = GetPlayerVehicle() == _lastVehicle;
                float speed = API.GetEntitySpeed(_lastVehicle);

                if (_currentBrakeState == BrakeState.Off && speed < 1.0f && !isPlayerInVehicle)
                {
                    API.SetVehicleForwardSpeed(_lastVehicle, 1.0f);
                }
            }
        }

        private async Task DrawGearIndicator()
        {
            while (true)
            {
                await Delay(0);
                var vehicle = GetPlayerVehicle();
                if (IsValidVehicle(vehicle) && IsPlayerDriver(vehicle))
                {
                    DrawText(
                        x: 0.6460f,
                        y: 1.4154f,
                        text: _gearDisplay,
                        color: _currentBrakeState == BrakeState.On ? _parkColor : _driveColor
                    );
                }
            }
        }

        private void SetBrakeState(BrakeState newState)
        {
            _currentBrakeState = newState;
            _gearDisplay = newState == BrakeState.On ? "P" : "D";
        }

        private void PlaySound(string soundName)
        {
            TriggerServerEvent("InteractSound_SV:PlayWithinDistance", 5, soundName, 0.09f);
        }

        private int GetPlayerVehicle() => API.GetVehiclePedIsIn(API.PlayerPedId(), false);
        private bool IsValidVehicle(int vehicle) => vehicle != 0 && API.DoesEntityExist(vehicle);
        private bool IsPlayerDriver(int vehicle) => API.GetPedInVehicleSeat(vehicle, -1) == API.PlayerPedId();

        private void ShowNotification(string message)
        {
            API.SetNotificationTextEntry("STRING");
            API.AddTextComponentSubstringPlayerName(message);
            API.DrawNotification(false, true);
        }

        private void DrawText(float x, float y, string text, Color color)
        {
            API.SetTextFont(4);
            API.SetTextProportional(false);
            API.SetTextScale(0.6f, 0.6f);
            API.SetTextColour(color.R, color.G, color.B, color.A);
            API.SetTextDropShadow();
            API.SetTextEdge(2, 0, 0, 0, 255);
            API.SetTextOutline();
            API.BeginTextCommandDisplayText("STRING");
            API.AddTextComponentSubstringPlayerName(text);
            API.EndTextCommandDisplayText(x, y);
        }
    }

    public struct Color
    {
        public int R { get; }
        public int G { get; }
        public int B { get; }
        public int A { get; }

        public Color(int r, int g, int b, int a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}