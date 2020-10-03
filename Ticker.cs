using System;
using System.Collections.Generic;

namespace Ticker
{
    public class Ticker
    {
        private const double Gravity = 9.8;
        
        private readonly int _ballsCount = 1;
        private readonly double _mass = 1;
        private readonly double _radius = 1;
        private readonly double _bounce = 0.55;
        private readonly double _threadLength = 32.0;
        private readonly double _viscosity = 0.001;

        public Ticker(int ballsCount, double mass, double radius, double bounce, double threadLength, double viscosity)
        {
            _ballsCount = ballsCount;
            _mass = mass;
            _radius = radius;
            _bounce = bounce;
            _threadLength = threadLength;
            _viscosity = viscosity;
            Console.WriteLine("Маятник успешно создан.");
        }

        public string MakeTest()
        {
            var raisedBallsCount = Program.ReadParameter("Количество поднятных шаров", 1, _ballsCount);
            var height = Program.ReadParameter("Высота", 0, _threadLength);
            var count = 0;
            var energy = raisedBallsCount * _mass * height * Gravity;
            var lastSpinEnergy = energy;
            while (energy > 0.01)
            {
                energy = GetEnergyAfterMoving(energy);
                energy = GetEnergyAfterCollision(energy, raisedBallsCount);
                energy = GetEnergyAfterMoving(energy);
                energy = GetEnergyAfterMoving(energy);
                energy = GetEnergyAfterCollision(energy, raisedBallsCount);
                energy = GetEnergyAfterMoving(energy);
                if (Math.Abs(lastSpinEnergy - energy) < 0.01)
                    return "Бесконечный цикл";

                count++;
            }

            return count.ToString();
        }
        
        private double GetEnergyAfterMoving(double energy)
        {
            var maxVelocity = VelocityFromEnergy(energy);
            var frictionForce = 6 * Math.PI * maxVelocity / 2 * _viscosity * _radius;
            var height = energy / _mass / Gravity;
            var angle = Math.Acos(1 - height / _threadLength);
            var path = angle * _threadLength;
            energy -= frictionForce * path;
            return energy;
        }

        private double GetEnergyAfterCollision(double energy, int raisedBalls)
        {
            var velocity = VelocityFromEnergy(energy);
            energy -= (1 - _bounce * _bounce) * raisedBalls * (_ballsCount - raisedBalls) * _mass * velocity *
                      velocity / _ballsCount / 2;

            return energy;
        }

        private double VelocityFromEnergy(double energy)
        {
            return Math.Sqrt(energy * 2 / _mass);
        }
    }
}