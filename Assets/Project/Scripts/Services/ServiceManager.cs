using System;
using System.Collections.Generic;
using Infrastructure.Core;
using Services.Audio;
using Services.Input;
using Services.SceneLoad;

namespace Services
{
    public class ServiceManager
    {
        private readonly Dictionary<Type, IService> _services;

        public ServiceManager(Game game)
        {
            _services = new Dictionary<Type, IService>()
            {
                [typeof(InputService)] = new InputService(),
                [typeof(AudioService)] = new AudioService(),
                [typeof(SceneService)] = new SceneService(game),
            };
        }

        public void RegisterService<T>() where T : class, IService
        {
            T service = Service<T>();
            service.RegisterService();
        }

        public T Service<T>() where T : class, IService =>
            _services[typeof(T)] as T;
    }
}